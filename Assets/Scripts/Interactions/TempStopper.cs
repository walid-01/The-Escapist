using System;
using UnityEngine;

public class TempStopper : MonoBehaviour
{
    [SerializeField] private GameObject unlocker;

    private IUnlockable unlockable;

    private void Start()
    {
        unlockable = unlocker.GetComponent<IUnlockable>();
        unlockable.OnToggle += DeleteSelf;
    }

    private void DeleteSelf(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}
