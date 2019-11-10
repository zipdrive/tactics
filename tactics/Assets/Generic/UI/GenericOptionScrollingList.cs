using UnityEngine;
using UnityEngine.UI;

public class GenericOptionScrollingList<T> : GenericOptionList<T> where T : GenericOption
{
    public ScrollRect scrollRect;
    public Vector2 buffer;

    public override int Index
    {
        get
        {
            return base.Index;
        }

        set
        {
            base.Index = value;

            if (scrollRect.horizontal)
            {
                float viewMin = scrollRect.viewport.position.x;
                float viewWidth = scrollRect.viewport.rect.width;
                float viewMax = viewMin + viewWidth;
                RectTransform optTrans = Current.GetComponent<RectTransform>();

                float optMax = optTrans.position.x + optTrans.sizeDelta.x;

                if (optMax > viewMax - buffer.x)
                {
                    float dist = optMax - viewMax + buffer.x;
                    scrollRect.horizontalScrollbar.value += dist / (scrollRect.content.rect.width - viewWidth);
                }
                else if (optTrans.position.x < viewMin + buffer.x)
                {
                    float dist = viewMin + buffer.x - optTrans.position.x;
                    scrollRect.horizontalScrollbar.value -= dist / (scrollRect.content.rect.width - viewWidth);
                }
            }

            if (scrollRect.vertical)
            {
                float viewMin = scrollRect.viewport.position.y;
                float viewHeight = scrollRect.viewport.rect.height;
                float viewMax = viewMin + viewHeight;
                RectTransform optTrans = Current.GetComponent<RectTransform>();

                float optMax = optTrans.position.y + optTrans.sizeDelta.y;

                if (optMax > viewMax - buffer.y)
                {
                    float dist = optMax - viewMax + buffer.y;
                    scrollRect.verticalScrollbar.value += dist / (scrollRect.content.rect.height - viewHeight);
                }
                else if (optTrans.position.y < viewMin + buffer.y)
                {
                    float dist = viewMin + buffer.y - optTrans.position.y;
                    scrollRect.verticalScrollbar.value -= dist / (scrollRect.content.rect.height - viewHeight);
                }
            }
        }
    }

    public override void Clear()
    {
        base.Clear();

        if (scrollRect.horizontal)
            scrollRect.horizontalScrollbar.value = 0f;
        if (scrollRect.vertical)
            scrollRect.verticalScrollbar.value = 0f;
    }
}
