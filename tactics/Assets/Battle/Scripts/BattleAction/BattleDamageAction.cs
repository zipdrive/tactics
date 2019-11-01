using System;

public class BattleDamageAction : BattleAction
{
    private BattleAgent m_Agent;
    private Element m_Element;
    private int m_Damage;

    public BattleDamageAction(BattleAgent agent, Element element, int damage)
    {
        m_Agent = agent;
        m_Element = element;
        m_Damage = damage;
    }

    public override void Execute(BattleManager manager, BattleQueueTime time)
    {
        BattleDamageEvent eventInfo = new BattleDamageEvent(
            BattleEvent.Type.BeforeTakeDamage, 
            manager,
            time,
            m_Agent, 
            m_Element, 
            m_Damage
            );

        m_Agent.Damage(eventInfo);
    }
}
