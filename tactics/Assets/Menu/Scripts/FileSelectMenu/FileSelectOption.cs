public class FileSelectOption : MenuSetPageOption
{
    public SaveGameIO.SaveGame SaveGame;

    public override void Select()
    {
        SaveGameIO.Current = SaveGame;
        base.Select();
    }
}