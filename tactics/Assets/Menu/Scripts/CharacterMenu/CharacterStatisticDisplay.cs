using UnityEngine;
using TMPro;

public class CharacterStatisticDisplay : MonoBehaviour
{
    public TextMeshProUGUI HPValue;
    public TextMeshProUGUI SPValue;

    public TextMeshProUGUI StatValues;

    public TextMeshProUGUI[] Resistances;

    public TextMeshProUGUI Immunities;
    public TextMeshProUGUI Auto;
    public TextMeshProUGUI PassiveSkills;

    public void Reset()
    {
        Character character = CharacterMenu.Character.BaseCharacter;

        HPValue.text = character["HP"].ToString();
        SPValue.text = character["SP"].ToString();

        StatValues.text = character["Attack"] + "\n" + character["Magic"] + "\n" + character["Speed"];

        foreach (TextMeshProUGUI resistance in Resistances)
        {
            resistance.text = character[resistance.name] + "%";
        }

        // TODO immunities
        // TODO auto status
        // TODO passive skills
    }

    public void Simulate(string slot, Equipment equip)
    {
        Character character = CharacterMenu.Character.BaseCharacter;
        Equipment prior;
        if (!character.GetEquipment(slot, out prior))
            prior = null;

        StatValues.text = SimulatedValue(character, equip, prior, "Attack") + "\n"
            + SimulatedValue(character, equip, prior, "Magic") + "\n"
            + SimulatedValue(character, equip, prior, "Speed");

        foreach (TextMeshProUGUI resistance in Resistances)
        {
            resistance.text = SimulatedValue(character, equip, prior, resistance.name) + "%";
        }

        // TODO immunities
        // TODO auto status
        // TODO passive skills
    }

    private string SimulatedValue(Character character, Equipment equip, Equipment prior, string stat)
    {
        const string neutral = "<color=#000000>";
        const string positive = "<color=#F10D0D>";
        const string negative = "<color=#00BE25>";

        int diff = Bonus(equip, stat) - Bonus(prior, stat);
        return (diff == 0 ? neutral : (diff < 0 ? negative : positive)) + (character[stat] + diff);
    }

    private int Bonus(Equipment equip, string stat)
    {
        if (equip == null)
            return 0;
        else
            return equip[stat];
    }
}
