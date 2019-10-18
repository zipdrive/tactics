using System;
using UnityEngine;

public class ReportSkillEffect : SkillEffect
{
    public override void Execute(BattleSkillEvent eventInfo)
    {
        BattleManager manager = GameObject.FindObjectOfType<BattleManager>();
        manager.Add(new BattleShowAgentReport(int.MinValue, eventInfo.Target));
    }
}