using UnityEngine;

public class MainMenu : GenericOptionList<GenericAnimateOption>
{
    public GameObject pressAnyKeyToContinue;

    protected override void OnEnable()
    {
        Clear();

        // Add "New Game" option
        // Transition to campaign select menu
        Add(true, "New Game").Trigger = "NewGame";

        // Add "Continue" option
        // Transition to load game menu
        Add(SaveGameIO.SavedGames.Count > 0, "Continue").Trigger = "Continue";

        // Add "Settings" option
        // Transition to settings menu
        Add(true, "Settings").Trigger = "Settings";

        // Add "Credits" option
        // Transition to credits page
        Add(true, "Credits").Trigger = "Credits";

        // Add "Quit" option
        // Quit game
        Add(true, "Quit");

        m_Index = SaveGameIO.SavedGames.Count > 0 ? 1 : 0;

        base.OnEnable();

        SaveGameIO.Current = null;
        Campaign.Current = null;
    }

    protected override void Update()
    {
        if (!Interactable)
        {
            if (Input.anyKeyDown && pressAnyKeyToContinue.activeInHierarchy)
            {
                Destroy(pressAnyKeyToContinue);
                List.gameObject.SetActive(true);
                Interactable = true;
                Reset();
            }
        }
        else
        {
            base.Update();
        }
    }
}
