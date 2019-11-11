using UnityEngine;

public class FileSelectMenu : GenericOptionScrollingList<FileSelectOption>
{
    public GenericOption backButton;

    protected override void OnEnable()
    {
        Clear();

        foreach (SaveGameIO.SaveGame savedGame in SaveGameIO.SavedGames)
        {
            string playTime = string.Empty;
            int hours = Mathf.FloorToInt(savedGame.Time / 3600f);
            playTime += (hours < 10 ? "0" + hours.ToString() : hours.ToString()) + ":";
            int minutes = Mathf.FloorToInt(savedGame.Time / 60f) % 60;
            playTime += (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString()) + ":";
            int seconds = Mathf.FloorToInt(savedGame.Time) % 60;
            playTime += (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString()) + "<size=16>.";
            int milliseconds = Mathf.FloorToInt(0.001f * savedGame.Time) % 1000;
            playTime += milliseconds < 100 ? "0" + (milliseconds < 10 ? "0" + milliseconds.ToString() : milliseconds.ToString()) : milliseconds.ToString();

            Add(true, savedGame.Name, "Completion:\n<font=\"Bahnschrift SDF\">" + savedGame.Completion + "%</font>", "Play Time:\n<font=\"Bahnschrift SDF\">" + playTime).SaveGame = savedGame;
        }

        Interactable = true;
        backButton.Highlighted = false;

        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Vertical"))
        {
            if (backButton.Highlighted && Input.GetAxis("Vertical") < 0f)
            {
                backButton.Highlighted = false;
                Current.Highlighted = true;
                Interactable = true;
            }
            else if (!backButton.Highlighted && Input.GetAxis("Vertical") > 0f)
            {
                backButton.Highlighted = true;
                Current.Highlighted = false;
                Interactable = false;
            }
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            Current.Highlighted = false;
            backButton.Highlighted = true;
            backButton.Select();
        }
        else if (Input.GetButtonDown("Submit") && backButton.Highlighted)
        {
            backButton.Select();
        }
    }
}
