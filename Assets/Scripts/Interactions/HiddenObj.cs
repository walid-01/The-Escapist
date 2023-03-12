using System;
using UnityEngine;

public class HiddenObj : MonoBehaviour
{
    [SerializeField] private GameObject ObjToUnhide;
    private IUnlockable unlockable;

    private void Awake()
    {
        unlockable = GetComponent<IUnlockable>();
        unlockable.OnToggle += Unhide;
    }

    private void Unhide(object sender, EventArgs e)
    {
        ObjToUnhide.SetActive(true);
        this.tag = "Untagged";
    }
}
