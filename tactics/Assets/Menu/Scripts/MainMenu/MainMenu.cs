using System.Collections.Generic;
using UnityEngine;

public class MainMenu : GenericOptionList<GenericOption>
{
    public GameObject pressAnyKeyToContinue;

    public override void Reset()
    {
        if (SaveGameIO.SavedGames.Count > 0)
        {
            m_Options[1].Enabled = true;
            m_Index = 1;
        }
        else
        {
            m_Options[1].Enabled = false;
            m_Index = 0;
        }

        Current.Highlighted = true;
        base.Reset();
    }

    protected override void OnEnable()
    {
        SaveGameIO.Current = null;
        Campaign.Current = null;
    }

    protected override void Update()
    {
        if (!Interactable && pressAnyKeyToContinue != null)
        {
            if (Input.anyKeyDown && pressAnyKeyToContinue.activeInHierarchy)
            {
                Destroy(pressAnyKeyToContinue);
                pressAnyKeyToContinue = null;

                List.gameObject.SetActive(true);
                Interactable = true;
                
                m_Options = new List<GenericOption>(GetComponentsInChildren<GenericOption>());
                //m_Options.Add(GetComponentInChildren<???Option>());
                Reset();
            }
        }
        else
        {
            base.Update();
        }
    }
}
