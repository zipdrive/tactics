using System.Collections.Generic;

public class Party : IEnumerable<PlayerCharacter>
{
    private List<PlayerCharacter> ActiveMembers = new List<PlayerCharacter>();
    private List<PlayerCharacter> ReserveMembers = new List<PlayerCharacter>();

    public bool Contains(Character character)
    {
        foreach (PlayerCharacter pc in this)
        {
            if (pc.BaseCharacter == character)
                return true;
        }
        return false;
    }

    public bool IsActive(Character character)
    {
        foreach (PlayerCharacter pc in ActiveMembers)
        {
            if (pc.BaseCharacter == character)
                return true;
        }
        return false;
    }

    public void Add(Character character, bool active)
    {
        Add(new PlayerCharacter(character), active);
    }

    public void Add(PlayerCharacter character, bool active)
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
            if (ActiveMembers[k].BaseCharacter == character)
            {
                ActiveMembers.RemoveAt(k);
                return;
            }
        }

        for (int k = ReserveMembers.Count - 1; k >= 0; --k)
        {
            if (ReserveMembers[k].BaseCharacter == character)
            {
                ReserveMembers.RemoveAt(k);
                return;
            }
        }
    }

    public void Clear()
    {
        ActiveMembers.Clear();
        ReserveMembers.Clear();
    }

    public IEnumerator<PlayerCharacter> GetEnumerator()
    {
        foreach (PlayerCharacter character in ActiveMembers)
            yield return character;
        foreach (PlayerCharacter character in ReserveMembers)
            yield return character;
        yield break;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
