using System;
using UnityEngine;

public class ToggleAnimation : MonoBehaviour
{
    [SerializeField] private GameObject unlocker;
    [SerializeField] private string animationOn;
    [SerializeField] private string animationOff;

    private IUnlockable unlockable;
    private Animator animator;    
    private bool isOn = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        unlockable = unlocker.GetComponent<IUnlockable>();
        unlockable.OnToggle += ChangeAnimation;
    }

    public void ChangeAnimation(object sender, EventArgs e)
    {
        if (isOn)
        {
            animator.Play(animationOff, 0, 0.0f);
        }
        else
        {
            animator.Play(animationOn, 0, 0.0f);
        }
        isOn = !isOn;
    }
}
