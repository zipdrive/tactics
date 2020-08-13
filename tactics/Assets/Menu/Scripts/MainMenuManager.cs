using UnityEngine;

public class MainMenuManager : MenuPageManager
{
    private static bool m_FancyFadeIn = true;

    protected override void Start()
    {
        base.Start();

        Animator animator = GetComponent<Animator>();
        if (m_FancyFadeIn)
        {
            animator.SetTrigger("Fade");
            m_FancyFadeIn = false;
        }
        else
        {
            MainMenu main = GetComponentInChildren<MainMenu>();
            Destroy(main.pressAnyKeyToContinue);
            main.List.gameObject.SetActive(true);
            main.Interactable = true;
            main.Reset();
        }
    }


    public void CreateNewSaveGame(string name)
    {
        SaveGameIO.SaveGame saveGame = new SaveGameIO.SaveGame(
            name,
            Application.dataPath + "/Save/save" + (SaveGameIO.SavedGames.Count + 1) + ".xml",
            0,
            0f
        );

        SaveGameIO.Save(saveGame);
        SaveGameIO.SavedGames.Add(saveGame);
        SaveGameIO.Current = saveGame;
    }

    public void LoadCampaign()
    {
        if (Campaign.Current != null)
        {
            Campaign.LoadCurrent();
        }
    }
}
