using UnityEngine;

public class BattleSkillListMenu : BattleListMenu
{
    public override BattleMenu Next
    {
        get
        {
            return null;
        }

        set
        {
            // Do nothing
        }
    }


    private string m_ID;

    protected class SkillOption : Option
    {
        public Skill skill;

        public SkillOption(Skill skill, float spcost, float hpcost)
        {
            this.skill = skill;

            int cost = skill.Cost(m_Agent);
            int hp = Mathf.CeilToInt(hpcost * cost);
            int sp = Mathf.CeilToInt(spcost * cost);
            labels = new string[] { skill.Name, (hp > 0 ? hp + " HP" : "") + (hp > 0 && sp > 0 ? ", " : "") + (sp > 0 ? sp + " SP" : "") };
            enabled = hp < m_Agent.HP && sp <= m_Agent.SP;

            description = skill.Description.Split('\n');
        }

        public override UpdateResult OnSelect(BattleMenu menu)
        {
            BattleSkillListMenu skillMenu = menu as BattleSkillListMenu;

            if (skillMenu != null)
            {
                m_Selections[skillMenu.m_ID] = skill;
                return UpdateResult.Completed;
            }

            return UpdateResult.InProgress;
        }
    }


    public BattleSkillListMenu(SkillFilter filter, string id, float spcost, float hpcost)
    {
        m_ID = id;
        
        foreach (Skill skill in filter)
        {
            Add(new SkillOption(skill, spcost, hpcost));
        }
    }
}