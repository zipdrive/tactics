using System.Collections.Generic;
using UnityEngine;

public class BattleBerserkBehaviour : BattleBehaviour
{
    public BattleBerserkBehaviour(BattleAgent agent) : base(agent) { }

    public override BattleAction Update(bool canMove, bool canAct)
    {
        m_Manager.grid.Selector.SelectedTile = m_Agent.Coordinates;

        if (!canAct) return new BattleEndTurnAction(m_Agent);

        // Find most powerful skill
        WeaponSkillFilter filter = new WeaponSkillFilter(m_Agent.BaseCharacter);

        Skill bestSkill = null;
        float bestPower = float.NegativeInfinity;
        foreach (Skill skill in filter)
        { 
            float power = 0f;
            foreach (SkillEffect effect in skill.Effects)
                power += CalculatePower(effect);

            if (power > bestPower)
            {
                bestSkill = skill;
                bestPower = power;
            }
        }

        if (bestSkill == null) return new BattleEndTurnAction(m_Agent);

        // Select a target
        BattleManhattanDistanceZone range = Skill.GetRange(bestSkill.Range, m_Agent);
        List<Vector2Int> options = new List<Vector2Int>();
        Vector2Int target;

        foreach (Vector2Int point in range)
        {
            BattleTile tile = m_Manager.grid[point];
            if (tile != null && tile.Actor != null)
                options.Add(point);
        }

        if (options.Count == 0)
        {
            if (!canMove) return new BattleEndTurnAction(m_Agent);

            // Try to move closer
            target = TargetNearest();
            BattleAction moveAction = MoveWithinRangeOfTarget(target, range);
            
            return moveAction != null ? moveAction : new BattleEndTurnAction(m_Agent);
        }
        else if (options.Count == 1)
        {
            target = options[0];
        }
        else
        {
            System.Random rand = new System.Random();
            target = options[rand.Next() % options.Count];
        }

        BattleManhattanDistanceZone targets = Skill.GetTarget(bestSkill.Target, m_Agent, range);
        targets.Center = target;

        return new BattleSkillAction(m_Agent, bestSkill, targets, 0.01f * m_Agent["Power: " + bestSkill.Element], 1f, 0f);
    }

    private float CalculatePower(SkillEffect effect)
    {
        if (effect is DamageSkillEffect)
        {
            return (effect as DamageSkillEffect).Power;
        }
        else if (effect is HitSkillEffect)
        {
            float p = 0f;
            foreach (SkillEffect subeffect in (effect as HitSkillEffect).Effects)
            {
                p += CalculatePower(subeffect);
            }
            return p;
        }

        return 0f;
    }
}