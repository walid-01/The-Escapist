using System;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    [SerializeField] private GameObject unlocker;
    [SerializeField] private Material colorOn;
    [SerializeField] private Material colorOff;

    private IUnlockable unlockable;
    private new Renderer renderer;
    public bool isOn = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        unlockable = unlocker.GetComponent<IUnlockable>();
        unlockable.OnToggle += ChangeMaterial;
    }

    private void ChangeMaterial(object sender, EventArgs e)
    {
        if (isOn)
        {
            renderer.sharedMaterial = colorOff;
        }
        else
        {
            renderer.sharedMaterial = colorOn;
        }
        isOn = !isOn;
    }
}
