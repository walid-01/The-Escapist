using System;
using UnityEngine;

public class ToggleAnimation : Door
{
    [SerializeField] private GameObject unlocker;
    private IUnlockable unlockable;

    private void Awake()
    {
        unlockable = unlocker.GetComponent<IUnlockable>();
        unlockable.OnToggle += ChangeAnimation;
    }

    public void ChangeAnimation(object sender, EventArgs e)
    {
        PlayAnimation();
    }
}
