using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static CampaignMenuScene Menu;

    public void LoadNext()
    {
        SaveGameIO.Save();

        if (Campaign.Current == null)
            SceneManager.LoadScene("MainMenu");
        else
            Campaign.LoadNext();
    }
}
