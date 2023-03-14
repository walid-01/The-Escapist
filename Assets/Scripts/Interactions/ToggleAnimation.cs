using System;
using UnityEngine;

public class ToggleAnimation : Door
{
    [SerializeField] private ILever unlocker;

    private void Awake()
    {
        unlocker.OnToggle += ChangeAnimation;
    }

    public void ChangeAnimation(object sender, EventArgs e)
    {
        PlayAnimation();
    }
}
