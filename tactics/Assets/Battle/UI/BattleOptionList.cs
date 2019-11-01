using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BattleOptionList : GenericOptionList<BattleOption>
{
    public const int maxNumberOptionsShown = 6; // TODO something with this????

    public RectTransform DescriptionBox;
    public RectTransform HeightController;

    public override int Index
    {
        get
        {
            return base.Index;
        }

        set
        {
            base.Index = value;
            ShowDescription(Current.Description);
        }
    }


    protected override void Start()
    {
        base.Start();

        if (transform.GetSiblingIndex() > 0)
        {
            Transform priorMenuTransform = transform.parent.GetChild(transform.GetSiblingIndex() - 1);
            BattleOptionList priorMenu = priorMenuTransform.GetComponent<BattleOptionList>();

            if (priorMenu != null)
            {
                float h = 8f + (9.5f * m_Options.Count);
                float y = priorMenu.HeightController.sizeDelta.y +
                    8f + (19f * (priorMenu.m_Options.Count - priorMenu.Index)) - 9.5f;
                    /*priorMenu.optionListBox.sizeDelta.y +
                    priorMenu.m_Options[priorMenu.Index].GetComponent<RectTransform>().anchoredPosition.y;*/

                HeightController.sizeDelta = new Vector2(0f, Mathf.Max(6f, Mathf.Round(y - h)));
            }
        }

        DescriptionBox.GetComponent<Image>().color = Settings.TextBoxColor;
    }

    protected override void Update()
    {
        base.Update();

        if (transform.parent.childCount > transform.GetSiblingIndex() + 1)
        {
            DescriptionBox.gameObject.SetActive(false);
        }
        else
        {
            DescriptionBox.gameObject.SetActive(true);
            DescriptionBox.position = new Vector3(0f, m_Options[Index].transform.position.y);
            DescriptionBox.anchoredPosition = new Vector3(-10f,
                Mathf.Max(6f, Mathf.Round(DescriptionBox.anchoredPosition.y - (0.5f * DescriptionBox.sizeDelta.y))));
        }
    }


    public void ShowDescription(string[] description)
    {
        DescriptionBox.gameObject.SetActive(true);

        Text[] descriptionLabels = DescriptionBox.GetComponentsInChildren<Text>();
        for (int k = 0; k < descriptionLabels.Length; ++k)
        {
            if (k < description.Length)
            {
                descriptionLabels[k].gameObject.SetActive(true);
                descriptionLabels[k].text = description[k].Trim();
            }
            else descriptionLabels[k].gameObject.SetActive(false);
        }
    }

    public void HideDescription()
    {
        DescriptionBox.gameObject.SetActive(false);
    }
}
