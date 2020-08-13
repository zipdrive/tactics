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
        
        foreach (Equipment equipment in BaseCharacter.Equipment)
        {
            if (equipment != null)
            {
                foreach (Status passive in equipment.Passive)
                {
                    Inflict(passive, int.MaxValue, new BattleQueueTime(-100f, -10f));
                }
            }
        }
    }

    /// <summary>
    /// Cause the agent to take damage.
    /// </summary>
    /// <param name="eventInfo">Information about the damage to be taken</param>
    public void Damage(BattleDamageEvent eventInfo)
    {
        BattleQueueTime.Generator time = new BattleQueueTime.FiniteGenerator(eventInfo.Time, 3);

        string message = eventInfo.Damage < 0 ? (-eventInfo.Damage).ToString() : eventInfo.Damage.ToString();
        Color color = Color.white;
        
        // Pre-events
        eventInfo.Event = BattleEvent.Type.BeforeTakeDamage;
        eventInfo.Time = time.Generate();
        OnTrigger(eventInfo);

        // CP damage
        if (eventInfo.Affects == BattleDamageEvent.DamageTo.CP)
        {
            CP -= eventInfo.Damage;
            if (CP < 0) CP = 0;
            if (CP > 100) CP = 100;

            message += " CP";
        }

        // SP damage
        if (eventInfo.Affects == BattleDamageEvent.DamageTo.SP)
        {
            SP -= eventInfo.Damage;
            if (SP < 0) SP = 0;
            if (SP > this["SP"]) SP = this["SP"];

            message += " SP";
        }

        // HP damage
        if (eventInfo.Affects == BattleDamageEvent.DamageTo.HP)
        {
            int hp = HP - eventInfo.Damage;

            if (hp <= 0)
            {
                if (HP > 0) // Inflict critical status
                    Inflict(AssetHolder.StatusEffects["Critical"], 1, time.Generate(), "Critical");
                else if (eventInfo.Damage > 1) // Inflict KO status
                    Inflict(AssetHolder.StatusEffects["KO"], int.MaxValue, time.Generate(), "KO");

                HP = 0;
                CP = 0;

                message = "";
            }
            else
            {
                if (hp > this["HP"])
                {
                    HP = this["HP"];
                }
                else
                {
                    HP = hp;
                }
            }
        }

        // Show damage taken
        if (!message.Equals(""))
        {
            eventInfo.Manager.Add(new BattleShowAgentMessage(
              time.Generate(),
              eventInfo.Manager,
              this,
              message,
              color
              ));
        }

        // Post-effects
        eventInfo.Event = BattleEvent.Type.AfterTakeDamage;
        eventInfo.Time = time.Generate();
        OnTrigger(eventInfo);
    }

    /// <summary>
    /// Inflict the agent with a status effect.
    /// </summary>
    /// <param name="status">The status effect to inflict</param>
    /// <param name="duration">The duration of the status effect</param>
    public void Inflict(Status status, int duration, BattleQueueTime time, string message = "")
    {
        BattleManager manager = GameObject.FindObjectOfType<BattleManager>();

        if (StatusEffects.ContainsKey(status))
        {
            StatusEffects[status]["Duration"] += duration;
        }
        else
        {
            StatusEffects[status] = new StatusInstance(status, duration);

            BattleEvent eventInfo = new BattleEvent(BattleEvent.Type.FirstInflictedWithStatus, manager, time);
            status.OnTrigger(new StatusEvent(eventInfo, status, StatusEffects[status], this));
        }

        if (!message.Equals(""))
        {
            manager.Add(new BattleShowAgentMessage(time, manager, this, message));
        }
    }

    /// <summary>
    /// Update any status effects that may respond to an event
    /// </summary>
    /// <param name="eventInfo">The event in question</param>
    public void OnTrigger(BattleEvent eventInfo)
    {
        List<Status> statusEffects = new List<Status>(StatusEffects.Keys);

        foreach (Status statusEffect in statusEffects)
        {
            statusEffect.OnTrigger(new StatusEvent(eventInfo, statusEffect, StatusEffects[statusEffect], this));
        }
    }
}
