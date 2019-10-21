using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BattleMenuUI : MonoBehaviour
{
    public Animator optionPrefab;
    public RectTransform optionListBox;
    public RectTransform descriptionBox;
    public RectTransform heightController;

    private List<Animator> m_Options = new List<Animator>();
    public int Index = 0;

    public int maxNumberOptionsShown = 6;

    void Start()
    {
        if (transform.GetSiblingIndex() > 0)
        {
            Transform priorMenuTransform = transform.parent.GetChild(transform.GetSiblingIndex() - 1);
            BattleMenuUI priorMenu = priorMenuTransform.GetComponent<BattleMenuUI>();

            if (priorMenu != null)
            {
                float h = 8f + (9.5f * m_Options.Count);
                float y = priorMenu.heightController.sizeDelta.y +
                    8f + (19f * (priorMenu.m_Options.Count - priorMenu.Index)) - 9.5f;
                    /*priorMenu.optionListBox.sizeDelta.y +
                    priorMenu.m_Options[priorMenu.Index].GetComponent<RectTransform>().anchoredPosition.y;*/

                heightController.sizeDelta = new Vector2(0f, Mathf.Max(6f, Mathf.Round(y - h)));
            }
        }
    }

    void Update()
    {
        if (transform.parent.childCount > transform.GetSiblingIndex() + 1)
        {
            descriptionBox.gameObject.SetActive(false);
        }
        else
        {
            descriptionBox.gameObject.SetActive(true);
            descriptionBox.position = new Vector3(0f, m_Options[Index].transform.position.y);
            descriptionBox.anchoredPosition = new Vector3(-10f,
                Mathf.Max(6f, Mathf.Round(descriptionBox.anchoredPosition.y - (0.5f * descriptionBox.sizeDelta.y))));
        }
    }


    public void ClearOptions()
    {
        foreach (Animator anim in m_Options)
            Destroy(anim.gameObject);
        m_Options.Clear();
    }

    public void Add(bool enabled, params string[] labels)
    {
        Animator anim = Instantiate(optionPrefab, optionListBox);

        if (m_Options.Count == 0)
            Index = 0;

        m_Options.Add(anim);

        Text[] labelText = anim.GetComponentsInChildren<Text>();

        for (int k = 0; k < labelText.Length; ++k)
        {
            if (k < labels.Length)
                labelText[k].text = labels[k];
            else
                labelText[k].gameObject.SetActive(false);

            if (!enabled)
                labelText[k].color = Color.gray;
        }
    }

    public void ScrollUp()
    {
        if (Index > 0)
        {
            m_Options[Index--].SetBool("Highlighted", false);
            m_Options[Index].SetBool("Highlighted", true);
        }
    }

    public void ScrollDown()
    {
        if (Index < m_Options.Count - 1)
        {
            m_Options[Index++].SetBool("Highlighted", false);
            m_Options[Index].SetBool("Highlighted", true);
        }
    }

    public void Reset()
    {
        m_Options[Index].SetBool("Highlighted", true);
    }


    public void ShowDescription(string[] description)
    {
        descriptionBox.gameObject.SetActive(true);

        Text[] descriptionLabels = descriptionBox.GetComponentsInChildren<Text>();
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
        descriptionBox.gameObject.SetActive(false);
    }
}
