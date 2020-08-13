using System;
using System.Collections.Generic;
using System.Xml;

public class HumanoidCharacter : Character
{
    private Dictionary<string, Equipment> m_Equipment = new Dictionary<string, Equipment>();


    public override int this[string stat]
    {
        get
        {
            int s = base[stat];

            foreach (KeyValuePair<string, Equipment> pair in m_Equipment)
            {
                if (pair.Value != null)
                {
                    if (pair.Key.StartsWith("Passive"))
                    {
                        if (!stat.Equals("Passive Slots"))
                        {
                            int index;
                            if (int.TryParse(pair.Key.Substring(7), out index) && index < this["Passive Slots"])
                            {
                                s += pair.Value[stat];
                            }
                        }
                    }
                    else
                    {
                        s += pair.Value[stat];
                    }
                }
            }

            return s;
        }

        set
        {
            base[stat] = value;
        }
    }


    public HumanoidCharacter(HumanoidCharacter character) : base(character)
    {
        foreach (KeyValuePair<string, Equipment> pair in character.m_Equipment)
        {
            m_Equipment.Add(pair.Key, pair.Value);
        }
    }

    public HumanoidCharacter(XmlElement characterInfo) : base(characterInfo) { }


    public override IEnumerable<Equipment> Equipment
    {
        get
        {
            return m_Equipment.Values;
        }
    }

    public override bool GetEquipment(string key, out Equipment equipment)
    {
        m_Equipment.TryGetValue(key, out equipment);

        if (key.StartsWith("Passive"))
        {
            int index;
            if (int.TryParse(key.Substring(7), out index))
            {
                return index < this["Passive Slots"];
            }
        }
        else if (key.Equals("Hand1"))
        {
            Equipment rightHand;
            if (m_Equipment.TryGetValue("Hand0", out rightHand))
            {
                return (rightHand as Weapon).TwoHanded;
            }
            return true;
        }
        else
        {
            return key.Equals("Hand0") || key.Equals("Body") || key.Equals("Accessory");
        }

        return false;
    }

    public override void SetEquipment(string key, Equipment equipment)
    {
        if (equipment == null)
        {
            m_Equipment.Remove(key);
        }
        else if (key.StartsWith("Hand"))
        {
            Weapon weapon = equipment as Weapon;

            if (equipment.Slot == global::Equipment.Location.Hand && weapon != null)
            {
                int index;
                if (int.TryParse(key.Substring(4), out index))
                {
                    if (index == 0)
                    {
                        // Primary slot
                        if (weapon.TwoHanded)
                        {
                            m_Equipment["Hand1"] = null;
                        }

                        m_Equipment["Hand0"] = weapon;
                    }
                    else
                    {
                        // Secondary slot
                        if (weapon.TwoHanded)
                        {
                            m_Equipment["Hand0"] = weapon;
                            m_Equipment["Hand1"] = null;
                        }
                        else if (weapon.Type == WeaponType.Shield || this["Dual Wield"] > 0)
                        {
                            m_Equipment["Hand1"] = weapon;
                        }
                    }
                }
            }
        }
        else if (key.StartsWith("Passive"))
        {
            if (equipment.Slot == global::Equipment.Location.Passive)
            {
                m_Equipment[key] = equipment;
            }
        }
        else
        {
            Equipment.Location slot;
            if (Enum.TryParse(key, out slot))
            {
                if (equipment.Slot == slot)
                {
                    m_Equipment[key] = equipment;
                }
            }
        }
    }


    public override void Load(XmlElement characterInfo)
    {
        base.Load(characterInfo);

        // Load equipment
        int numPassiveSkillsEquipped = 0;
        foreach (XmlElement equipmentInfo in characterInfo.SelectNodes("equipment/*"))
        {
            Item tempItem;
            Equipment equipment = null;
            string id = (equipmentInfo.Name.Equals("passive") ? "Passive:" : "") + equipmentInfo.InnerText.Trim();
            if (AssetHolder.Items.TryGetValue(id, out tempItem))
            {
                equipment = tempItem as Equipment;
            }
            else
            {
                UnityEngine.Debug.Log("[HumanoidCharacter] Unable to retrieve equipment \"" + equipmentInfo.InnerText.Trim() + "\"");
            }

            if (equipment != null)
            {
                switch (equipmentInfo.Name)
                {
                    case "hand":
                        {
                            int index = 0;

                            // TODO set index

                            SetEquipment("Hand" + index, equipment);
                        }
                        break;
                    case "body":
                        SetEquipment("Body", equipment);
                        break;
                    case "accessory":
                        SetEquipment("Accessory", equipment);
                        break;
                    case "passive":
                        SetEquipment("Passive" + (numPassiveSkillsEquipped++), equipment);
                        break;
                }
            }
        }
    }

    public override Character Copy()
    {
        return new HumanoidCharacter(this);
    }
}