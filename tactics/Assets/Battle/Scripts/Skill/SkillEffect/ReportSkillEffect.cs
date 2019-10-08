using UnityEngine;

public class ReportSkillEffect : SkillEffect
{
    public override void Execute(BattleAgent user, BattleAgent target)
    {
        BattleManager manager = GameObject.FindObjectOfType<BattleManager>();
        manager.Add(new BattleShowAgentReport(int.MinValue, target));
    }
}