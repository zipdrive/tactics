using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PartyMemberOption : MenuSetPageOption
{
    private PlayerCharacter m_PartyMember;
    public PlayerCharacter PartyMember
    {
        get
        {
            return m_PartyMember;
        }

        set
        {
            m_PartyMember = value;

            Portrait.sprite = m_PartyMember.BaseCharacter.Sprite.Portrait;

            NameLabel.text = m_PartyMember.BaseCharacter.Name;
            TitleLabel.text = m_PartyMember.BaseCharacter.Title;
        }
    }

    public Image Portrait;

    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI TitleLabel;

    public override void Select()
    {
        CharacterMenu.Character = m_PartyMember;

        base.Select();
    }
}
