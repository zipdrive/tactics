using System.Collections.Generic;

public class Party
{
    private List<Character> ActiveMembers = new List<Character>();
    private List<Character> ReserveMembers = new List<Character>();

    public void Add(Character character, bool active)
    {
        if (active)
            ActiveMembers.Add(character);
        else
            ReserveMembers.Add(character);
    }

    public void Remove(Character character)
    {
        for (int k = ActiveMembers.Count - 1; k >= 0; --k)
        {
            if (ActiveMembers[k] == character)
            {
                ActiveMembers.RemoveAt(k);
                return;
            }
        }

        for (int k = ReserveMembers.Count - 1; k >= 0; --k)
        {
            if (ReserveMembers[k] == character)
            {
                ReserveMembers.RemoveAt(k);
                return;
            }
        }
    }
}
