using UnityEngine.UI;

public class EquipmentOption : GenericOption
{
    public Image Icon;

    private Equipment m_Equipment;
    public Equipment Equipment
    {
        get
        {
            return m_Equipment;
        }

        set
        {
            m_Equipment = value;

            if (m_Equipment == null || m_Equipment.Icon == null)
            {
                Icon.gameObject.SetActive(false);
            }
            else
            {
                Icon.sprite = m_Equipment.Icon;
            }
        }
    }

    public override void Select()
    {
        base.Select();

        EquipmentMenu menu = GetComponentInParent<EquipmentMenu>();
        CharacterMenu.Character.BaseCharacter.SetEquipment(menu.EquipmentSlot, m_Equipment);

        CharacterMenu characterMenu = GetComponentInParent<CharacterMenu>();
        characterMenu.Swap();
    }
}