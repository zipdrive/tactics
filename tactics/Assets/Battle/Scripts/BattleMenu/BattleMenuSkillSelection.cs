using UnityEngine;

public class BattleMenuSkillSelection : BattleMenuListSelection<Skill>
{
    protected BattleAgent m_User;

    protected bool m_Overdrive;
    protected int m_HP;
    protected int m_SP;

    public BattleMenuSkillSelection(string uiName, BattleAgent user, bool overdrive = false) : base(uiName, user.Coordinates)
    {
        m_User = user;

        m_Overdrive = overdrive;
        m_HP = user.HP;
        m_SP = user.SP;
    }

    public override void Construct(BattleManager manager)
    {
        m_User.HP = m_HP;
        m_User.SP = m_SP;

        base.Construct(manager);
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

            if (m_Overdrive)
            {
                m_User.HP -= opt.value.Cost;
                m_User.SP += opt.value.Cost;
            }
        }
        else
        {
            next = null;
        }
        
        decision = null;
    }
}
