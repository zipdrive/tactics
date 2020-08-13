using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewGameMenu : GenericList
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.Select();
    }

    void Update()
    {

    }
}
