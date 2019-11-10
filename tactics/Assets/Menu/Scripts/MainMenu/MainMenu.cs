using UnityEngine;

public class MainMenu : GenericOptionList<GenericAnimateOption>
{
    public GameObject pressAnyKeyToContinue;

    protected override void Start()
    {
        // Add "Play" option
        // Transition to campaign select menu
        Add(true, "Play").Trigger = "Campaign";

        // Add "Settings" option
        // Transition to settings menu
        Add(true, "Settings").Trigger = "Settings";

        // Add "Credits" option
        // Transition to credits page
        Add(true, "Credits").Trigger = "Credits";

        // Add "Quit" option
        // Quit game
        Add(true, "Quit");

        base.Start();
    }

    protected override void Update()
    {
        if (!Interactable)
        {
            if (Input.anyKeyDown)
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
