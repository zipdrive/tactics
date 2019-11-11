public class FileSelectOption : GenericAnimateOption
{
    public SaveGameIO.SaveGame SaveGame;

    public override void Select()
    {
        SaveGameIO.Current = SaveGame;
        base.Select();
    }
}