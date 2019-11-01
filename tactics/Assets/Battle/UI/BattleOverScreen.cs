using UnityEngine;
using UnityEngine.UI;

public class BattleOverScreen : MonoBehaviour
{
    public Text mainText;
    public Text subtitleText;

    public bool success
    {
        set
        {
            if (value)
            {
                mainText.text = "CONGRATULATIONS!";
            }
            else
            {
                mainText.text = "BATTLE FAILED";
            }
        }
    }

    public string message
    {
        set
        {
            subtitleText.text = value;
        }
    }
}
