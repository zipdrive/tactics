using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAgent
{
    public readonly Character BaseCharacter;

    private BattleUnit m_Unit;

    /// <summary>
    /// The unit that the agent is a part of.
    /// </summary>
    public BattleUnit Unit
    {
        get
        {
            return m_Unit;
        }

        set
        {
            if (m_Unit != null) m_Unit.Remove(this);
            
            m_Unit = value;
            m_Unit.Add(this);
        }
    }

    private BattleBehaviour m_DefaultBehaviour;

    /// <summary>
    /// The behaviour of the agent.
    /// </summary>
    public BattleBehaviour Behaviour
    {
        get
        {
            return m_DefaultBehaviour;
        }
    }

    /// <summary>
    /// The coordinates of the agent.
    /// </summary>
    public Vector2Int Coordinates;
    /// <summary>
    /// The direction that the agent is facing.
    /// </summary>
    public float Direction;

    public int HP;
    public int SP;
    public int CP;

    private Dictionary<string, int> m_TempStats = new Dictionary<string, int>();

    public int this[string stat]
    {
        get
        {
            int s = m_TempStats.ContainsKey(stat) ? m_TempStats[stat] : 0;
            s += BaseCharacter[stat];

            foreach (StatusInstance status in StatusEffects.Values)
                s += status[stat];

            if (!stat.StartsWith("Resist"))
            {
                return s < 0 ? 0 : s;
            }
            else
            {
                s = s < -100 ? -100 : s;

                string element = stat.Substring(7);
                if (element.Equals("Fire") || element.Equals("Ice") || element.Equals("Lightning") || element.Equals("Corrosion"))
                    return s;
                else
                    return s > 100 ? 100 : s;
            }
        }

        set
        {
            m_TempStats[stat] = value;
        }
    }

    public Dictionary<Status, StatusInstance> StatusEffects = new Dictionary<Status, StatusInstance>();


    


    public BattleAgent(Character baseCharacter, BattleBehaviour behaviour)
    {
        BaseCharacter = baseCharacter;
        m_DefaultBehaviour = behaviour;

        HP = this["HP"];
        SP = this["SP"];
        
        foreach (Skill skill in BaseCharacter.ActiveSkills)
        {
            if (this["Skill: " + skill.Type] == 0) this["Skill: " + skill.Type] = 1;
        }
        
        foreach (Status status in BaseCharacter.PassiveSkills)
        {
            if (status != null)
            {
                StatusEffects.Add(status, new StatusInstance(status, int.MaxValue));

                BattleEvent eventInfo = new BattleEvent(BattleEvent.Type.FirstInflictedWithStatus);
                status.OnTrigger(new StatusEvent(eventInfo, status, StatusEffects[status], this));
            }
        }
    }

    public void Damage(BattleDamageEvent eventInfo)
    {
        if (HP - eventInfo.Damage > this["HP"])
            HP = this["HP"];
        else if (HP - eventInfo.Damage < 0)
            HP = 0;
        else
            HP -= eventInfo.Damage;
        
        Debug.Log("[BattleAgent] " + BaseCharacter.Name + " took " + eventInfo.Damage + " " + eventInfo.Element.ToString().ToLower() + " damage!");
    }

    public void OnTrigger(BattleEvent eventInfo)
    {
        List<Status> statusEffects = new List<Status>(StatusEffects.Keys);

        foreach (Status statusEffect in statusEffects)
        {
            statusEffect.OnTrigger(new StatusEvent(eventInfo, statusEffect, StatusEffects[statusEffect], this));
        }
    }
}
