using System.Xml;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static string NextMapFile = "map_test";
    public static string NextBattle = "debug";

    public Transform battleMenus;
    public BattleOptionList battleOptionListPrefab;
    public BattleOverScreen battleOverScreen;

    public BattleGrid grid;
    public List<BattleAgent> agents = new List<BattleAgent>();

    private List<BattleQueueMember> m_Queue;
    private BattleQueueMember m_Current;
    private Dictionary<string, BattleAction> m_Cutscenes;

    void Start()
    {
        // Load map
        XmlDocument mapDoc = new XmlDocument();
        mapDoc.PreserveWhitespace = false;

        try
        {
            mapDoc.Load("Assets/Battle/Data/Maps/" + NextMapFile + ".xml");
            XmlNode root = mapDoc.SelectSingleNode("map");

            grid.info = root.SelectSingleNode("grid") as XmlElement;

            foreach (XmlElement battleInfo in root.SelectNodes("battle"))
            {
                if (battleInfo.GetAttribute("id").Equals(NextBattle))
                {
                    // Load agents

                    BattleUnit.Reset();

                    foreach (XmlElement characterInfo in battleInfo.SelectNodes("character"))
                    {
                        try
                        {
                            string unit = characterInfo.HasAttribute("unit") ? characterInfo.GetAttribute("unit") : "civilian";

                            BattleBehaviour behaviour;
                            if (characterInfo.HasAttribute("behaviour"))
                                behaviour = BattleBehaviour.Parse(characterInfo.GetAttribute("behaviour"));
                            else if (unit.Equals("player"))
                                behaviour = null;
                            else if (unit.Equals("civilian"))
                                behaviour = null; // TODO run away
                            else
                                behaviour = new BattleOffensiveBehaviour();

                            Character character = AssetHolder.Characters[characterInfo.GetAttribute("name")];
                            BattleAgent agent = new BattleAgent(character, behaviour);
                            agent.Coordinates = new Vector2Int(
                                int.Parse(characterInfo.GetAttribute("x")),
                                int.Parse(characterInfo.GetAttribute("y"))
                                );
                            agent.Unit = BattleUnit.Get(unit);

                            agents.Add(agent);

                            BattleTile tile = grid[agent.Coordinates];
                            BattleActor actor = Instantiate<BattleActor>((BattleActor)AssetHolder.Objects["actor"], tile.ground.transform);
                            actor.Agent = agent;
                            tile.Actor = actor;
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log(e); // TODO
                        }
                    }


                    // Load cutscenes

                    m_Cutscenes = new Dictionary<string, BattleAction>();

                    foreach (XmlElement cutsceneInfo in battleInfo.SelectNodes("cutscene"))
                    {
                        try
                        {
                            List<BattleAction> cutsceneSequence = new List<BattleAction>();

                            foreach (XmlNode sceneInfoNode in cutsceneInfo.ChildNodes)
                            {
                                XmlElement sceneInfo = sceneInfoNode as XmlElement;
                                if (sceneInfo != null)
                                {
                                    if (sceneInfo.Name.Equals("move"))
                                    {
                                        string character = sceneInfo.GetAttribute("character");
                                        BattleAgent target = null;
                                        foreach (BattleAgent agent in agents)
                                        {
                                            if (agent.BaseCharacter.Name.Equals(character))
                                            {
                                                target = agent;
                                                break;
                                            }
                                        }

                                        if (target != null)
                                        {
                                            cutsceneSequence.Add(
                                                new BattleMoveAction(
                                                    target,
                                                    new Vector2Int(
                                                        int.Parse(sceneInfo.GetAttribute("x")),
                                                        int.Parse(sceneInfo.GetAttribute("y"))
                                                    )
                                                )
                                            );
                                        }
                                    }
                                    else if (sceneInfo.Name.Equals("text"))
                                    {
                                        string character = sceneInfo.GetAttribute("character");
                                        BattleAgent target = null;
                                        foreach (BattleAgent agent in agents)
                                        {
                                            if (agent.BaseCharacter.Name.Equals(character))
                                            {
                                                target = agent;
                                                break;
                                            }
                                        }

                                        if (target != null)
                                        {
                                            cutsceneSequence.Add(
                                                new BattleDialogueAction(target, sceneInfo.InnerText.Trim())
                                            );
                                        }
                                    }
                                }
                            }

                            m_Cutscenes.Add(cutsceneInfo.GetAttribute("trigger"), new BattleSequenceAction(cutsceneSequence));
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log(e);
                        }
                    }
                }
            }

            BattleAgentUI.Shown = false;
            BattleSelector.Shown = false;
        }
        catch (System.Exception e)
        {
            Debug.Log(e); // TODO
        }
    }

    void Update()
    {
        if (m_Current != null && m_Current.QUpdate(this))
        {
            m_Current = m_Queue[0];
            m_Queue.RemoveAt(0);
            m_Current.QStart(this);
        }
    }

    public void Add(BattleQueueMember member)
    {
        m_Queue.Add(member);
        m_Queue.Sort();
    }

    public void BeginBattle()
    {
        BattleAgentUI.Shown = true;

        grid.CameraSpeed = Settings.CameraSpeed;

        // Construct queue
        m_Queue = new List<BattleQueueMember>();
        m_Queue.Add(new BattleClock());

        if (m_Cutscenes.ContainsKey("begin"))
        {
            m_Cutscenes["begin"].Execute(this, new BattleQueueTime(-1000f, -100f));
        }

        m_Current = m_Queue[0];
        m_Queue.RemoveAt(0);
        m_Current.QStart(this);
    }

    /// <summary>
    /// Check for if the battle has been won or lost.
    /// </summary>
    public void Check()
    {
        Dictionary<string, BattleUnit> units = BattleUnit.GetAll();

        bool battleOver;

        if (units.ContainsKey("player"))
        {
            // End battle if all players have been incapacitated.

            battleOver = true;
            foreach (BattleAgent playerAgent in units["player"])
            {
                if (playerAgent["Speed"] > 0)
                {
                    battleOver = false;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(false, "");
            }
        }

        if (units.ContainsKey("rescue"))
        {
            // End battle if any unit to rescue is incapacitated.

            battleOver = false;
            string agentName = "";
            foreach (BattleAgent rescueAgent in units["rescue"])
            {
                if (rescueAgent["Speed"] == 0)
                {
                    battleOver = true;
                    agentName = rescueAgent.BaseCharacter.Name;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(false, "You failed to save " + agentName + ".");
            }
        }

        if (units.ContainsKey("civilian"))
        {
            // End battle if any civilian is harmed.

            battleOver = false;
            foreach (BattleAgent civilianAgent in units["civilian"])
            {
                if (civilianAgent.HP < civilianAgent["HP"])
                {
                    battleOver = true;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(false, "A civilian was harmed.");
            }
        }

        if (units.ContainsKey("enemy"))
        {
            // End battle if all enemies have been incapacitated.

            battleOver = true;
            foreach (BattleAgent enemyAgent in units["enemy"])
            {
                if (enemyAgent["Speed"] > 0)
                {
                    battleOver = false;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(true, "This battle is over!");
            }
        }
    }

    public void EndBattle(bool success, string message)
    {
        // Show battle completed screen
        battleOverScreen.gameObject.SetActive(true);
        battleOverScreen.success = success;
        battleOverScreen.message = message;

        BattleAgentUI.Shown = false;

        // Destroy the BattleManager
        GameObject.Destroy(this);
    }
}