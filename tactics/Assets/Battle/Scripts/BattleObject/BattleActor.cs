using UnityEngine;

public class BattleActor : BattleObject2D
{
    public BattleAgent Agent;

    private MeshRenderer m_Renderer;
    private BattleSprite m_Sprite;

    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_Sprite = Agent.BaseCharacter.Sprite;
    }

    protected override void Update()
    {
        base.Update();

        m_Sprite.Update(Time.deltaTime);
        m_Renderer.sharedMaterial = m_Sprite.Image;

        transform.rotation = BillboardRotation;
        transform.localScale = new Vector3(m_Sprite.Direction == Direction.Left ? -1f : 1f, 1f, 2.5f);
    }
}