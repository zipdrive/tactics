using UnityEngine;
using TMPro;

public class BattleOverScreen : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subtitleText;

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
