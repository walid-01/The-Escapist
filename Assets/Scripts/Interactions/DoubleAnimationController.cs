using System;
using UnityEngine;

public class DoubleAnimationController : Door
{
    [SerializeField] private ILever firstUnlocker;
    [SerializeField] private ILever secondUnlocker;

    private void Awake()
    {
        firstUnlocker.OnToggle += ChangeAnimation;
        secondUnlocker.OnToggle += ChangeAnimation;
    }

    public void ChangeAnimation(object sender, EventArgs e)
    {
        PlayAnimation();
    }
}
