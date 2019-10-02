using System;

public class MoveSkill : Skill
{
    private static MoveSkill m_Skill;

    public static MoveSkill Skill
    {
        get
        {
            if (m_Skill == null)
                m_Skill = new MoveSkill();
            return m_Skill;
        }
    }

    private MoveSkill()
    {
        Name = "Move";
        Description = "Move to another tile.";
        Area = new MoveArea();
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        throw new NotImplementedException();
    }
}