public class RadialSkillRange : SkillRange
{
    private int m_Radius;

    public BattleSelectableManhattanRadius this[BattleAgent user]
    {
        get
        {
            return new BattleSelectableManhattanRadius(user.Coordinates, 0, m_Radius);
        }
    }

    public RadialSkillRange(int radius)
    {
        m_Radius = radius;
    }
}
