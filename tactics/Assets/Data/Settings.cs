using UnityEngine;

public class Settings
{
    /// <summary>
    /// How quickly the camera moves around during battles.
    /// </summary>
    public static float CameraSpeed = 0.05f;

    /// <summary>
    /// How long messages (like miss notifications and damage taken) display for during battle.
    /// </summary>
    public static float MessageSpeed = 1f;

    /// <summary>
    /// How long enemy AI targeting is held for.
    /// </summary>
    public static float AITargetSpeed = 0.5f;

    /// <summary>
    /// How quickly dialogue is displayed.
    /// </summary>
    public static float TextSpeed = 30f;

    /// <summary>
    /// What color the text boxes are.
    /// </summary>
    public static Color TextBoxColor = new Color32(0xb0, 0xd0, 0xd0, 0xff);
}
