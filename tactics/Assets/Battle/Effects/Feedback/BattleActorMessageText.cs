using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro), typeof(Animator))]
public class BattleActorMessageText : MonoBehaviour
{
    public string message;
    public Color color;

    public float duration = -0.25f;

    void Start()
    {
        TextMeshPro text = GetComponent<TextMeshPro>();
        text.text = message;
        text.color = color;
    }

    void Update()
    {
        duration += Time.deltaTime;

        if (duration > Settings.MessageSpeed)
        {
            GetComponent<Animator>().SetTrigger("FadeOut");
        }
    }

    public void SelfDestruct()
    {
        GameObject.Destroy(this.gameObject);
    }
}