using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PartyMemberOption : GenericAnimateOption
{
    private Character m_PartyMember;
    public Character PartyMember
    {
        get
        {
            return m_PartyMember;
        }

        set
        {
            m_PartyMember = value;

            Portrait.sprite = m_PartyMember.Sprite.Portrait;

            NameLabel.text = m_PartyMember.Name;
            TitleLabel.text = m_PartyMember.Title;
        }
    }

    public Image Portrait;

    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI TitleLabel;

    public override void Select()
    {


        base.Select();
    }
}
