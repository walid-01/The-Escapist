using System;
using UnityEngine;

public class DoubleSequentialAnimation : MonoBehaviour
{
    [SerializeField] private GameObject firstUnlocker;
    [SerializeField] private GameObject secondUnlocker;
    [SerializeField] private string animationOn;

    private IUnlockable firstUnlockable;
    private IUnlockable secondUnlockable;

    private bool firstUnlockableUnlocked;
    private bool secondUnlockableUnlocked;

    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        firstUnlockable = firstUnlocker.GetComponent<IUnlockable>();
        firstUnlockable.OnToggle += UnlockFirstUnlockable;

        secondUnlockable = secondUnlocker.GetComponent<IUnlockable>();
        secondUnlockable.OnToggle += UnlockSecondUnlockable;
    }

    private void UnlockFirstUnlockable(object sender, EventArgs e)
    {
        firstUnlockableUnlocked = true;
        TryUnlock();
    }

    private void UnlockSecondUnlockable(object sender, EventArgs e)
    {
        secondUnlockableUnlocked = true;
        TryUnlock();
    }

    public void TryUnlock()
    {
        if (firstUnlockableUnlocked)
        {
            if (secondUnlockableUnlocked)
            {
                animator.Play(animationOn, 0, 0.0f);
            }
        }
    }
}