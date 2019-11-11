using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimeCounter : MonoBehaviour
{
    public static float PlayTime = 0f;

    // Update is called once per frame
    void Update()
    {
        PlayTime += Time.unscaledDeltaTime;
    }
}
