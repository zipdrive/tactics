public interface Character
{
    int MaximumHP { get; }
    int CurrentHP { get; set; }
    int MaximumSP { get; }
    int CurrentSP { get; set; }

    int Attack { get; }
    int Defense { get; }
    int Magic { get; }
    int Speed { get; }

    int Move { get; }
}