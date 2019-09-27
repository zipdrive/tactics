using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BattleObject : MonoBehaviour
{
    public RotatableSprite sprite;

    private SpriteRenderer m_Renderer;

    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
