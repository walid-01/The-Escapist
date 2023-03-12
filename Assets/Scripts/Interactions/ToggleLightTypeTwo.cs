using System;
using UnityEngine;

public class ToggleLightTypeTwo : MonoBehaviour
{
    [SerializeField] private GameObject firstUnlocker;
    [SerializeField] private GameObject secondUnlocker;
    [SerializeField] private Material colorOn;
    [SerializeField] private Material colorOff;

    private IUnlockable firstUnlockable;
    private IUnlockable secondUnlockable;
    private new Renderer renderer;

    private bool firstUnlockableUnlocked = false;
    private bool secondUnlockableUnlocked = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;

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
                renderer.sharedMaterial = colorOn;
            }
        }
    }
}
