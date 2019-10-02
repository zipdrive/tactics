using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BattleObject2D : BattleObject
{
    protected static Quaternion BillboardRotation = Quaternion.AngleAxis(30f, new Vector3(1f, 0f));

    protected virtual void Update()
    {
        transform.rotation = BillboardRotation;
        transform.localScale = new Vector3(1f, 1f, 2.5f);
    }
}