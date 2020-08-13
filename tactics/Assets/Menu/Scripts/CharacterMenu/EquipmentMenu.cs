using System.Collections.Generic;

public class EquipmentMenu : GenericOptionFixedSizeList<EquipmentOption>
{
    public CharacterStatisticDisplay characterStats;

    private string m_EquipmentSlot;
    public string EquipmentSlot
    {
        get
        {
            return m_EquipmentSlot;
        }

        set
        {
            m_EquipmentSlot = value;

            Clear();
            Add(true, "None").Equipment = null;

            if (m_EquipmentSlot.StartsWith("Passive"))
            {
                HashSet<Equipment> equipped = new HashSet<Equipment>();
                for (int k = 0; k < 4; ++k)
                {
                    Equipment equippedPassiveSkill;
                    if (CharacterMenu.Character.BaseCharacter.GetEquipment("Passive" + k, out equippedPassiveSkill))
                    {
                        equipped.Add(equippedPassiveSkill);
                    }
                }

                foreach (Equipment passive in CharacterMenu.Character.PassiveSkills)
                {
                    Add(!equipped.Contains(passive), passive.Name.Substring(8)).Equipment = passive;
                }
            }
            else
            {
                Equipment.Location slot;
                if (m_EquipmentSlot.StartsWith("Hand"))
                {
                    slot = Equipment.Location.Hand;
                }
                else
                {
                    System.Enum.TryParse(m_EquipmentSlot, out slot);
                }

                Dictionary<Equipment, int> inUseCounts = new Dictionary<Equipment, int>();
                foreach (PlayerCharacter pc in Campaign.Current.Party)
                {
                    foreach (Equipment equipped in pc.BaseCharacter.Equipment)
                    {
                        if (equipped.Slot == slot)
                        {
                            if (!inUseCounts.ContainsKey(equipped))
                                inUseCounts[equipped] = 1;
                            else
                                ++inUseCounts[equipped];
                        }
                    }
                }

                Campaign.Current.Inventory.Foreach((Equipment item, int count) =>
                    {
                        if (item.Slot == slot)
                        {
                            int inUseCount = inUseCounts.ContainsKey(item) ? inUseCounts[item] : 0;
                            Add(inUseCount < count, item.Name, inUseCount + "/" + count).Equipment = item;
                        }
                    }
                );
            }

            Index = 0;
            LowestVisibleIndex = 0;
        }
    }

    public override int Index
    {
        get
        {
            return base.Index;
        }

        set
        {
            base.Index = value;
            characterStats.Simulate(m_EquipmentSlot, Current.Equipment);
        }
    }
}