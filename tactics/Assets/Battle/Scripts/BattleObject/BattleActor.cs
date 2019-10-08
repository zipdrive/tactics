using UnityEngine;

public class BattleActor : BattleObject2D
{
    public BattleAgent Agent;

    private Transform m_GridTransform;
    private MeshRenderer m_Renderer;

    public BattleSprite Sprite;

    void Start()
    {
        m_GridTransform = GameObject.FindObjectOfType<BattleGrid>().transform;
        m_Renderer = GetComponent<MeshRenderer>();
        Sprite = Agent.BaseCharacter.Sprite;
    }

    protected override void Update()
    {
        base.Update();
        
        Sprite.Direction = (Direction)(90 * Mathf.RoundToInt((Agent.Direction - m_GridTransform.localEulerAngles.z) / 90f));

        Sprite.Update(Time.deltaTime);
        m_Renderer.sharedMaterial = Sprite.Image;

        transform.rotation = BillboardRotation;
        transform.localScale = new Vector3((int)Sprite.Direction % 180 == 0 ? -1f : 1f, 1f, 2.5f);
    }
}