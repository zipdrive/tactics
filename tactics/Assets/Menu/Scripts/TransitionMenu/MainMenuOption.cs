using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOption : GenericAnimateOption
{
    public override void Select()
    {
        Debug.Log("Selected.");

        Campaign.Current = null;
        base.Select();
    }
}
