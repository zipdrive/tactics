using UnityEngine;

public class GenericOptionFixedSizeList<T> : GenericOptionList<T> where T : GenericOption
{
    public int maxVisibleItems;

    public GameObject lowIndexArrow;
    public GameObject highIndexArrow;

    private int m_LowestVisibleIndex = 0;
    protected int LowestVisibleIndex
    {
        get
        {
            return m_LowestVisibleIndex;
        }

        set
        {
            if (value <= 1)
                m_LowestVisibleIndex = 0;
            else
                m_LowestVisibleIndex = value;

            float d = 1f / maxVisibleItems;
            float c;
            if (m_LowestVisibleIndex == 0)
            {
                lowIndexArrow.SetActive(false);
                c = 1f;
            }
            else
            {
                lowIndexArrow.SetActive(true);
                c = 1f - d;
            }

            int numVisibleItems;
            if (m_Options.Count <= maxVisibleItems)
            {
                highIndexArrow.SetActive(false);
                numVisibleItems = m_Options.Count;
            }
            else if (m_LowestVisibleIndex == m_Options.Count - maxVisibleItems)
            {
                highIndexArrow.SetActive(false);
                numVisibleItems = maxVisibleItems - 1;
            }
            else
            {
                highIndexArrow.SetActive(true);
                numVisibleItems = maxVisibleItems - (m_LowestVisibleIndex == 0 ? 1 : 2);
            }

            for (int k = m_LowestVisibleIndex - 1; k >= 0; --k)
            {
                m_Options[k].gameObject.SetActive(false);
            }
            for (int k = 0; k < numVisibleItems; ++k)
            {
                m_Options[m_LowestVisibleIndex + k].gameObject.SetActive(true);

                RectTransform trans = m_Options[m_LowestVisibleIndex + k].GetComponent<RectTransform>();
                trans.anchorMax = new Vector2(trans.anchorMax.x, c);
                c -= d;
                trans.anchorMin = new Vector2(trans.anchorMin.x, c);
            }
            for (int k = m_LowestVisibleIndex + numVisibleItems; k < m_Options.Count; ++k)
            {
                m_Options[k].gameObject.SetActive(false);
            }
        }
    }

    public override int Index
    {
        get
        {
            return base.Index;
        }

        set
        {
            if (m_Options.Count > maxVisibleItems 
                && (value < LowestVisibleIndex || value >= LowestVisibleIndex + maxVisibleItems))
            {
                LowestVisibleIndex = value;
            }

            base.Index = value;
        }
    }
}
