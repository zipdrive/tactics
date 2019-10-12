using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAgent
{
    public readonly Character BaseCharacter;

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

    public int this[string stat]
    {
        get
        {
            int s = BaseCharacter[stat];

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
    }

    public Dictionary<Status, StatusInstance> StatusEffects = new Dictionary<Status, StatusInstance>();


    private BattleBehaviour m_DefaultBehaviour;

    public BattleBehaviour Behaviour
    {
        get
        {
            return m_DefaultBehaviour;
        }

        set
        {
            m_DefaultBehaviour = value;
        }
    }


    public BattleAgent(Character baseCharacter)
    {
        BaseCharacter = baseCharacter;

        HP = this["HP"];
        SP = this["SP"];
    }

    public void Damage(int damage, Element element = Element.Null)
    {
        if (HP - damage > this["HP"])
            HP = this["HP"];
        else if (HP - damage < 0)
            HP = 0;
        else
            HP -= damage;
        
        Debug.Log("[BattleAgent] " + BaseCharacter.Name + " took " + damage + " " + element.ToString().ToLower() + " damage!");
    }
}
