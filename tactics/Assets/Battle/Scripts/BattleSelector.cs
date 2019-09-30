using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelector : MonoBehaviour
{
    public static float SelectionSpeed = 15f;
    public static bool SelectionSnap = false;

    public static bool Frozen = false;

    public Vector2Int SelectedTile;
    public Vector3 Velocity;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Velocity * SelectionSpeed * Time.deltaTime;

        if ((transform.localPosition.x > SelectedTile.x && Velocity.x > 0f)
            || (transform.localPosition.x < SelectedTile.x && Velocity.x < 0f)
            || (transform.localPosition.y > SelectedTile.y && Velocity.y > 0f)
            || (transform.localPosition.y < SelectedTile.y && Velocity.y < 0f))
            Snap();
    }

    public void Snap()
    {
        transform.localPosition = new Vector3(SelectedTile.x, SelectedTile.y, -0.5f * GetComponentInParent<BattleGrid>()[SelectedTile.x, SelectedTile.y].Height);
    }
}
