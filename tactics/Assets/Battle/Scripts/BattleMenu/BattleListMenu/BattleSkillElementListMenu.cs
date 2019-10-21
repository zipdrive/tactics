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

            // TODO description
            description = new string[] { "This is an element." };
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
