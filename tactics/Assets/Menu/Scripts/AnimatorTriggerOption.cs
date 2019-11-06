using UnityEngine;

public class AnimatorTriggerOption : GenericOption
{
    private Animator m_TopmostAnimator;
    public string trigger;

    protected override void Start()
    {
        base.Start();
        // Find the top-most Animator
        Animator[] animators = GetComponentsInParent<Animator>();
        m_TopmostAnimator = animators[animators.Length - 1];
    }

    public override void Select()
    {
        base.Select();
        m_TopmostAnimator.SetTrigger(trigger);
    }
}
