using System;
using UnityEngine;

public class RemoveOnInteract : MonoBehaviour
{
    [SerializeField] private GameObject unlocker;
    private IUnlockable unlockable;

    private void Awake()
    {
        unlockable = GetComponent<IUnlockable>();
        unlockable.OnToggle += Remove;
    }

    private void Remove(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}
