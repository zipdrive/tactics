using UnityEngine;

public class BattleMenuSkillSelection : BattleMenuListSelection<Skill>
{
    protected BattleAgent m_User;
    protected bool m_Overdrive;

    public BattleMenuSkillSelection(string uiName, BattleAgent user, bool overdrive = false) : base(uiName, user.Coordinates)
    {
        m_User = user;
        m_Overdrive = overdrive; // TODO something with this
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
