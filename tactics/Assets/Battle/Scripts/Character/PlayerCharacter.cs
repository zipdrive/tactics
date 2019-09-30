using System.Collections.Generic;

public class PlayerCharacter : Character
{
    int m_HP;
    int m_SP;

    int m_Attack;
    int m_Defense;
    int m_Magic;
    int m_Speed;

    public int HP
    {
        get
        {
            return m_HP;
        }
    }

    public int SP
    {
        get
        {
            return m_SP;
        }
    }

    public int Attack
    {
        get
        {
            int a = m_Attack;

            if (PrimaryWeapon != null) a += PrimaryWeapon.AttackBonus;
            if (SecondaryWeapon != null) a += SecondaryWeapon.AttackBonus;

            return a;
        }
    }

    public int Defense
    {
        get
        {
            return m_Defense;
        }
    }

    public int Magic
    {
        get
        {
            return m_Magic;
        }
    }

    public int Speed
    {
        get
        {
            return m_Speed;
        }
    }

    public int Move
    {
        get
        {
            return 3;
        }
    }

    List<WeaponSkill> m_WeaponSkills;

    public List<WeaponSkill> WeaponSkills
    {
        get
        {
            return m_WeaponSkills;
        }
    }

    public List<Skill> MagicSkills
    {
        get
        {
            return null;
        }
    }

    Weapon m_Primary;
    Weapon m_Secondary;

    public Weapon PrimaryWeapon
    {
        get
        {
            return m_Primary;
        }

        set
        {
            if (m_Primary != null) PlayerInventory.Increment(m_Primary);

            if (value.Type == WeaponType.Bow && m_Secondary != null)
            {
                PlayerInventory.Increment(m_Secondary);
                m_Secondary = null;
            }

            m_Primary = value;
        }
    }

    public Weapon SecondaryWeapon
    {
        get
        {
            return m_Secondary;
        }

        set
        {
            // TODO
        }
    }


    public PlayerCharacter()
    {
        m_WeaponSkills = new List<WeaponSkill>();
        m_WeaponSkills.Add(new WeaponSkill(
            "Bash", 
            "", 
            new WeaponSkillArea(), 
            WeaponType.Hammer, WeaponType.Shield
            ));
    }
}