using UnityEngine;
using System.Collections.Generic;

public class CharacterMenu : GenericOptionList<EquipmentSlotOption>
{
    public static PlayerCharacter Character;

    public GenericOption backButton;
    public EquipmentMenu equipmentMenu;

    public override int Index
    {
        get
        {
            return base.Index;
        }

        set
        {
            if (m_Options[value].Enabled)
            {
                base.Index = value;
            }
            else
            {
                int d = value - m_Index;
                for (int k = value + d; k < m_Options.Count && k >= 0; k += d)
                {
                    if (m_Options[k].Enabled)
                    {
                        base.Index = k;
                        break;
                    }
                }
            }

            equipmentMenu.EquipmentSlot = Current.EquipmentSlot;
        }
    }

    protected override void OnEnable()
    {
        m_Options = new List<EquipmentSlotOption>(GetComponentsInChildren<EquipmentSlotOption>());
        Reset();
        Index = 0;

        base.OnEnable();
    }


    public override void Reset()
    {
        base.Reset();

        foreach (EquipmentSlotOption slot in m_Options)
        {
            Equipment equipment;
            if (Character.BaseCharacter.GetEquipment(slot.EquipmentSlot, out equipment))
            {
                slot.Enabled = true;
                slot.Equipment = equipment;
            }
            else
            {
                slot.Enabled = false;
                slot.Equipment = null;
            }
        }

        equipmentMenu.EquipmentSlot = Current.EquipmentSlot;
        equipmentMenu.Current.Highlighted = false;
    }

    public void Swap()
    {
        equipmentMenu.Interactable = Interactable;
        equipmentMenu.Current.Highlighted = equipmentMenu.Interactable;
        Interactable = !Interactable;

        if (Interactable)
        {
            Reset();
            equipmentMenu.characterStats.Reset();
        }
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Cancel"))
        {
            if (Interactable)
            {
                backButton.Highlighted = true;
                backButton.Select();
                Interactable = false;
                Current.Highlighted = false;
            }
            else
            {
                Swap();
            }
        }
    }
}
