using System;
using System.Xml;

public class MagicSkill : Skill
{
    public int Cost;

    public MagicSkill(XmlElement magicSkillInfo) : base(magicSkillInfo)
    {
        Cost = int.Parse(magicSkillInfo.GetAttribute("cost"));
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        throw new NotImplementedException();
    }
}