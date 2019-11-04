using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class BattleAgentDialogueUI : MonoBehaviour
{
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;

    public string text;

    public bool Shown
    {
        set
        {
            GetComponent<Animator>().SetBool("Show", value);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            dialogue.text = text;
        }
    }
}
