using System.Collections.Generic;

public class BattleSkillElementListMenu : BattleListMenu
{
    private BattleSkillListMenu m_Next;

    public override BattleMenu Next
    {
        get
        {
            return m_Next;
        }

        set
        {
            if (value == null)
                m_Next = null;
            else
                m_Next = value as BattleSkillListMenu;
        }
    }


    private string m_ID;
    private float m_SPCost;
    private float m_HPCost;

    protected class SkillElementOption : Option
    {
        public SkillElementOption(string element)
        {
            labels = new string[] { element };
            enabled = true;

            string desc;
            switch (element)
            {
                case "Martial":
                    desc = "Martial arts techniques.";
                    break;
                case "Blunt":
                    desc = "Combat techniques that use a bludgeoning weapon.";
                    break;
                case "Slashing":
                    desc = "Combat techniques that use a slashing weapon.";
                    break;
                case "Piercing":
                    desc = "Combat techniques that use a piercing weapon.";
                    break;
                case "Ranged":
                    desc = "Combat techniques that use a ranged weapon.";
                    break;
                case "Shield":
                    desc = "Defensive techniques that use a shield.";
                    break;
                case "Fire":
                    desc = "Magic of heat and flame.";
                    break;
                case "Ice":
                    desc = "Magic of cold and ice.";
                    break;
                case "Lightning":
                    desc = "Magic of electricity.";
                    break;
                case "Corrosion":
                    desc = "Magic of acid, corrosion, and decay.";
                    break;
                case "Air":
                    desc = "Magic of clouds and wind.";
                    break;
                case "Bio":
                    desc = "Magic that manipulates the body.";
                    break;
                case "Anima":
                    desc = "Magic that manipulates the soul, mind, and emotion.";
                    break;
                case "Time":
                    desc = "Magic that warps space and time.";
                    break;
                default:
                    desc = "This is an element.";
                    break;
            }
            
            description = new string[] { desc };
        }

        public override UpdateResult OnSelect(BattleMenu menu)
        {
            BattleSkillElementListMenu elementsMenu = menu as BattleSkillElementListMenu;

            if (elementsMenu != null)
            {
                elementsMenu.m_Next = new BattleSkillListMenu(
                    new ElementSkillFilter(m_Agent.BaseCharacter, labels[0]), 
                    elementsMenu.m_ID, 
                    elementsMenu.m_SPCost, 
                    elementsMenu.m_HPCost
                    );
                elementsMenu.m_Next.Construct();
                return UpdateResult.InProgress;
            }

            return UpdateResult.InProgress;
        }
    }


    public BattleSkillElementListMenu(SkillFilter filter, string id, float spcost, float hpcost)
    {
        m_ID = id;
        m_SPCost = spcost;
        m_HPCost = hpcost;

        HashSet<string> elements = new HashSet<string>();
        
        foreach (Skill skill in filter)
        {
            if (!elements.Contains(skill.Element))
            {
                elements.Add(skill.Element);
                Add(new SkillElementOption(skill.Element));
            }
        }
    }
}
