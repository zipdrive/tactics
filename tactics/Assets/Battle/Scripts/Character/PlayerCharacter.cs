using System;
using System.Collections.Generic;
using System.Xml;

public class PlayerCharacter : Character
{
    Weapon m_Primary;
    Weapon m_Secondary;
    Equipment m_Body;
    Equipment m_Head;
    Equipment m_Accessory;

    public override Weapon PrimaryWeapon
    {
        get
        {
            return m_Primary;
        }

        set
        {
            if (m_Primary != null) PlayerInventory.Increment(m_Primary);
            
            if (value != null && value.Type == WeaponType.Bow && m_Secondary != null)
            {
                PlayerInventory.Increment(m_Secondary);
                m_Secondary = null;
            }

            m_Primary = value;
        }
    }

    public override Weapon SecondaryWeapon
    {
        get
        {
            if (m_Primary != null && m_Primary.Type == WeaponType.Bow)
                return m_Primary;
            return m_Secondary;
        }

        set
        {
            // TODO
        }
    }

    public override Equipment Body
    {
        get
        {
            return m_Body;
        }

        set
        {
            m_Body = value;
        }
    }

    public override Equipment Head
    {
        get
        {
            return m_Head;
        }

        set
        {
            m_Head = value;
        }
    }

    public override Equipment Accessory
    {
        get
        {
            return m_Accessory;
        }

        set
        {
            m_Accessory = value;
        }
    }


    private Status[] m_PassiveSkills;
    public override IEnumerable<Status> PassiveSkills
    {
        get
        {
            return m_PassiveSkills;
        }
    }


    public PlayerCharacter(XmlElement characterInfo) : base(characterInfo)
    {
        // TODO
    }
}