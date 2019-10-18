using System.Xml;

public class StatusTriggerExecutor : StatusEffectExecutor
{
    private BattleEvent.Type m_Event;

    public StatusTriggerExecutor(XmlElement effectsInfo) : base(effectsInfo)
    {
        switch (effectsInfo.Name)
        {
            case "begin":
                m_Event = BattleEvent.Type.FirstInflictedWithStatus;
                break;
            case "tick":
                m_Event = BattleEvent.Type.Tick;
                break;
            case "turn":
                m_Event = effectsInfo.HasAttribute("trigger") && effectsInfo.GetAttribute("trigger").Equals("before") ? 
                    BattleEvent.Type.BeforeTurn : 
                    BattleEvent.Type.AfterTurn;
                break;
            case "use":
                m_Event = BattleEvent.Type.BeforeUseSkill;
                break;
            case "target":
                m_Event = effectsInfo.HasAttribute("trigger") && effectsInfo.GetAttribute("trigger").Equals("after") ? 
                    BattleEvent.Type.AfterTargetedBySkill : 
                    BattleEvent.Type.BeforeTargetedBySkill;
                break;
            case "damaged":
                m_Event = effectsInfo.HasAttribute("trigger") && effectsInfo.GetAttribute("trigger").Equals("after") ?
                    BattleEvent.Type.AfterTakeDamage :
                    BattleEvent.Type.BeforeTakeDamage;
                break;
        }
    }

    public override void Execute(StatusEvent eventInfo)
    {
        if (eventInfo.Event.Event == m_Event)
            base.Execute(eventInfo);
    }
}
