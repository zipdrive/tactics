using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
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
