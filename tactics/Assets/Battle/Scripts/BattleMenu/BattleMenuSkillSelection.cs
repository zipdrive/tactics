using UnityEngine;

public class BattleMenuSkillSelection : BattleMenuListSelection<Skill>
{
    protected BattleAgent m_User;

    public BattleMenuSkillSelection(string uiName, BattleAgent user) : base(uiName, user.Coordinates)
    {
        m_User = user;
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        Option opt = m_Options[m_Index];

        if (!opt.disabled)
        {
            Skill skill = opt.value;
            next = skill.EffectArea is RangeSkillEffectArea ?
                (BattleMenu)new BattleMenuSkillAreaConfirmation(m_User, skill, m_User.Coordinates) :
                new BattleMenuSkillAreaSelection(m_User, skill);
        }
        else
        {
            next = null;
        }
        
        decision = null;
    }
}
