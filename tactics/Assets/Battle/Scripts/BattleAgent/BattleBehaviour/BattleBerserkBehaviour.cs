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
        Weapon weapon1 = m_Agent.BaseCharacter.PrimaryWeapon;
        Weapon weapon2 = m_Agent.BaseCharacter.SecondaryWeapon;
        string tag1 = weapon1 != null ? weapon1.Type.ToString() : "Fist";
        string tag2 = weapon2 != null ? weapon2.Type.ToString() : "Fist";
        Skill bestSkill = null;
        float bestPower = float.NegativeInfinity;
        foreach (Skill skill in m_Agent.BaseCharacter.Skills)
        {
            if (skill.Tags.Contains("Weapon") && (skill.Tags.Contains(tag1) || skill.Tags.Contains(tag2)))
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
        }

        if (bestSkill == null) return new BattleEndTurnAction(m_Agent);

        // Select a target
        BattleSelectableManhattanRadius range = bestSkill.Range(m_Agent);
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
        
        return new BattleSkillAction(m_Agent, bestSkill, target);
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