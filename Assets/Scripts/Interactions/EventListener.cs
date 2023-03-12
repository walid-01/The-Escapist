using System;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    [SerializeField] GameObject eventHolder;

    private IUnlockable unlockable;

    void Start()
    {
        unlockable = eventHolder.GetComponent<IUnlockable>();
        unlockable.OnToggle += AssignTag;
    }

    private void AssignTag(object sender, EventArgs e)
    {
        gameObject.tag = "Interactable";
        unlockable.OnToggle -= AssignTag;
        Destroy(this);
    }
}
