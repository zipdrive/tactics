using System.Xml;

public class StatusSkillTriggerExecutor : StatusTriggerExecutor
{
    public string m_Type;
    public string m_Element;

    public StatusSkillTriggerExecutor(XmlElement effectsInfo) : base(effectsInfo)
    {
        m_Type = effectsInfo.HasAttribute("type") ? effectsInfo.GetAttribute("type") : "";
        m_Element = effectsInfo.HasAttribute("element") ? effectsInfo.GetAttribute("element") : "";
    }

    public override void Execute(StatusEvent eventInfo)
    {
        BattleSkillEvent skillEventInfo = eventInfo.Event as BattleSkillEvent;

        if (skillEventInfo != null)
        {
            Skill skill = skillEventInfo.Skill;
            if (skill.Type.StartsWith(m_Type) && skill.Element.StartsWith(m_Element))
                base.Execute(eventInfo);
        }
    }
}
