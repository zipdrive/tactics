using System.Collections.Generic;
using System.Xml;

public class Weapon : Equipment
{
    public WeaponType Type;

    public bool TwoHanded
    {
        get
        {
            return Type.Equals("Bow");
        }
    }


    public Weapon(XmlElement weaponInfo) : base(weaponInfo, Location.Hand)
    {
        if (!System.Enum.TryParse(weaponInfo.GetAttribute("type"), out Type))
            Type = WeaponType.Hammer;
    }

    public bool IsClass(string weaponClass)
    {
        switch (weaponClass)
        {
            case "Natural":
                return false;
            case "Martial":
                return Type == WeaponType.Fist;
            case "Blunt":
                return Type == WeaponType.Hammer || Type == WeaponType.Shield;
            case "Slashing":
                return Type == WeaponType.Axe || Type == WeaponType.Sword;
            case "Piercing":
                return Type == WeaponType.Knife || Type == WeaponType.Sword || Type == WeaponType.Spear;
            case "Ranged":
                return Type == WeaponType.Bow || Type == WeaponType.Gun;
            case "Shield":
                return Type == WeaponType.Shield;
            default:
                return true;
        }
    }
}