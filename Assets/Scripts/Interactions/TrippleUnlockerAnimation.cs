using System;
using UnityEngine;

public class TrippleUnlockerAnimation : MonoBehaviour
{
    [SerializeField] private GameObject firstUnlocker;
    [SerializeField] private GameObject secondUnlocker;
    [SerializeField] private GameObject thirdUnlocker;
    [SerializeField] private string animationOn;

    private IUnlockable firstUnlockable;
    private IUnlockable secondUnlockable;
    private IUnlockable thirdUnlockable;

    private bool unlockedFirst = false;
    private bool unlockedSecond = false;
    private bool unlockedThird = false;

    private Animator animator;
    private bool isOn = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        firstUnlockable = firstUnlocker.GetComponent<IUnlockable>();
        firstUnlockable.OnToggle += UnlockedFirst;

        secondUnlockable = secondUnlocker.GetComponent<IUnlockable>();
        secondUnlockable.OnToggle += UnlockedSecond;

        thirdUnlockable = thirdUnlocker.GetComponent<IUnlockable>();
        thirdUnlockable.OnToggle += UnlockedThird;
    }

    private void UnlockedFirst(object sender, EventArgs e)
    {
        unlockedFirst = true;
        TryOpen();
    }

    private void UnlockedSecond(object sender, EventArgs e)
    {
        unlockedSecond = true;
        TryOpen();
    }

    private void UnlockedThird(object sender, EventArgs e)
    {
        unlockedThird = true;
        TryOpen();
    }

    public void TryOpen()
    {
        if (unlockedFirst && unlockedSecond && unlockedThird)
        {
            animator.Play(animationOn, 0, 0.0f);
        }
        isOn = !isOn;
    }
}