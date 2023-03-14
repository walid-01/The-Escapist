using System;
using UnityEngine;

public class DoubleSequentialAnimation : MonoBehaviour
{
    [SerializeField] private IUse cable;
    [SerializeField] private MultiLevers levers;
    [SerializeField] private string animationOn;

    private bool isCablePut;
    private bool isLeversUnlocked;

    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        cable.OnToggle += UnlockFirstUnlockable;

        levers.OnToggle += UnlockSecondUnlockable;
    }

    private void UnlockFirstUnlockable(object sender, EventArgs e)
    {
        isCablePut = true;
        TryUnlock();
    }

    private void UnlockSecondUnlockable(object sender, EventArgs e)
    {
        isLeversUnlocked = true;
        TryUnlock();
    }

    public void TryUnlock()
    {
        if (isCablePut && isLeversUnlocked)
        {
            animator.Play(animationOn, 0, 0.0f);
        }
    }
}