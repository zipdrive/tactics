using UnityEngine;

public class GenericAnimateOption : GenericOption
{
    protected Animator m_MenuAnimator;
    public string Trigger = "";

    protected override void Start()
    {
        base.Start();
        // Find the top-most Animator
        Animator[] animators = GetComponentsInParent<Animator>();
        m_MenuAnimator = animators[animators.Length - 1];
    }

    public override void Select()
    {
        base.Select();
        m_MenuAnimator.SetTrigger(Trigger);
    }
}