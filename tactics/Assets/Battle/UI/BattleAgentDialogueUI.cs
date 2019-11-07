using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class BattleAgentDialogueUI : MonoBehaviour
{
    public Image speakerBox;
    public TextMeshProUGUI speaker;

    public Image dialogueBox;
    public TextMeshProUGUI dialogue;

    private string m_Text = "";
    private int m_Index = 0;

    private List<string> m_TextAppearing = new List<string>();
    private float m_Time = 0f;
    private float m_Speed = 0.03f;
    private float m_Pause = 0f;

    public string Text
    {
        get
        {
            return m_Text;
        }

        set
        {
            dialogue.text = "";

            m_Text = value;
            m_Index = -1;

            m_Time = 0f;
            m_Speed = 1f / Settings.TextSpeed;

            m_TextAppearing.Clear();
            m_TextAppearing.Add("");
            m_TextAppearing.Add("");
            m_TextAppearing.Add("");
        }
    }

    public bool Finished
    {
        get
        {
            return m_TextAppearing.Count <= 1;
        }
    }

    public bool Shown
    {
        set
        {
            GetComponent<Animator>().SetBool("Show", value);
        }
    }

    void Start()
    {
        speakerBox.color = Settings.TextBoxColor;
        dialogueBox.color = Settings.TextBoxColor;
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            m_Index = -1;
            string text = "";

            while (m_Index < m_Text.Length - 1)
                text += Increment();

            dialogue.text = text;
            m_TextAppearing.Clear();
        }
        else if (m_TextAppearing.Count > 1)
        {
            if (m_Pause > 0f)
            {
                m_Pause -= Time.deltaTime;
            }

            m_Time += Time.deltaTime;
            if (m_Time > m_Speed)
            {
                while (m_Time > m_Speed)
                {
                    if (m_Index >= m_Text.Length - 1)
                    {
                        if (m_TextAppearing.Count > 1)
                        {
                            m_TextAppearing[0] += m_TextAppearing[1];
                            for (int k = 1; k < m_TextAppearing.Count - 1; ++k)
                                m_TextAppearing[k] = m_TextAppearing[k + 1];
                            m_TextAppearing.RemoveAt(m_TextAppearing.Count - 1);
                        }
                    }
                    else
                    {
                        string addition;

                        if (m_Pause > 0f)
                        {
                            addition = "";
                        }
                        else
                        {
                            addition = Increment();
                        }

                        if (m_TextAppearing.Count < 4)
                            m_TextAppearing.Add(addition);
                        else
                        {
                            m_TextAppearing[0] += m_TextAppearing[1];
                            m_TextAppearing[1] = m_TextAppearing[2];
                            m_TextAppearing[2] = m_TextAppearing[3];
                            m_TextAppearing[3] = addition;
                        }
                    }

                    m_Time -= m_Speed;
                }

                dialogue.text = m_TextAppearing[0];
                if (m_TextAppearing.Count > 1)
                {
                    dialogue.text += "<alpha=#b0>" + m_TextAppearing[1];
                    if (m_TextAppearing.Count > 2)
                    {
                        dialogue.text += "<alpha=#80>" + m_TextAppearing[2];
                        if (m_TextAppearing.Count > 3)
                            dialogue.text += "<alpha=#40>" + m_TextAppearing[3];
                    }
                }
            }
        }
    }

    private string Increment()
    {
        ++m_Index;

        if (m_Text[m_Index] == '<')
        {
            Match match = Regex.Match(m_Text.Substring(m_Index), @"\A(<([^<]*)>.?)");
            if (match.Success)
            {
                m_Index += match.Groups[1].Length - 1;

                Match speedMatch = Regex.Match(match.Groups[2].Value, @"speed=(\d*\.?\d*)");
                if (speedMatch.Success)
                {
                    float mod;
                    if (!float.TryParse(speedMatch.Groups[1].Value, out mod))
                        mod = 1f;
                    m_Speed = 1f / (Settings.TextSpeed * mod);

                    if (m_Index < m_Text.Length)
                        return m_Text[m_Index].ToString();
                    else
                        return "";
                }
                else
                {
                    Match pauseMatch = Regex.Match(match.Groups[2].Value, @"pause=(\d*.?\d*)");
                    if (pauseMatch.Success)
                    {
                        if (!float.TryParse(pauseMatch.Groups[1].Value, out m_Pause))
                            m_Pause = 30f;
                        m_Pause = m_Pause / Settings.TextSpeed;

                        if (m_Index < m_Text.Length)
                            return m_Text[m_Index].ToString();
                        else
                            return "";
                    }
                    else
                    {
                        return match.Groups[1].Value;
                    }
                }
            }
            else
            {
                return "<";
            }
        }

        return m_Text[m_Index].ToString();
    }
}
