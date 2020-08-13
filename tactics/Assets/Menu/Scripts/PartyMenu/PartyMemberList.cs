using UnityEngine;

public class PartyMemberList : GenericOptionScrollingList<PartyMemberOption>
{
    public GenericOption backButton;

    protected override void Awake()
    {
        foreach (PlayerCharacter character in Campaign.Current.Party)
        {
            Add(Instantiate(OptionPrefab, List)).PartyMember = character;
        }

        base.Awake();
    }

    protected override void OnEnable()
    {
        Interactable = true;
        backButton.Highlighted = false;

        base.OnEnable();

        if (m_Index == 0)
        {
            scrollRect.horizontalScrollbar.value = 0f;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Vertical"))
        {
            if (backButton.Highlighted && Input.GetAxis("Vertical") < 0f)
            {
                backButton.Highlighted = false;
                Current.Highlighted = true;
                Interactable = true;
            }
            else if (!backButton.Highlighted && Input.GetAxis("Vertical") > 0f)
            {
                backButton.Highlighted = true;
                Current.Highlighted = false;
                Interactable = false;
            }
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            Current.Highlighted = false;
            backButton.Highlighted = true;
            backButton.Select();
            m_Index = 0;
        }
        else if (Input.GetButtonDown("Submit") && backButton.Highlighted)
        {
            backButton.Select();
            m_Index = 0;
        }
    }
}