using UnityEngine.UI;
using TMPro;

public class EquipmentSlotOption : GenericOption
{
    public Image Icon;
    public TextMeshProUGUI Label;

    public string EquipmentSlot;

    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }

        set
        {
            base.Enabled = value;

            if (Icon != null)
                Icon.gameObject.SetActive(value);
            Label.text = "";
        }
    }

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

            if (m_Equipment == null)
            {
                if (Icon != null) Icon.gameObject.SetActive(false);
                Label.text = "None";
            }
            else
            {
                if (Icon != null)
                {
                    if (m_Equipment.Icon == null)
                    {
                        Icon.gameObject.SetActive(false);
                    }
                    else
                    {
                        Icon.gameObject.SetActive(true);
                        Icon.sprite = m_Equipment.Icon;
                    }
                }
                Label.text = m_Equipment.Name;
            }
        }
    }

    public override void Select()
    {
        base.Select();

        GetComponentInParent<CharacterMenu>().Swap();
    }
}