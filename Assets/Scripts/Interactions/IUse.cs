using System;
using UnityEngine;

public class IUse : MonoBehaviour, IInteractable, IUnlockable
{
    [SerializeField] Item.ItemType requiredItem;

    private DataManager dataManager;
    private Player playerObj;

    public event EventHandler OnToggle;

    private void Awake()
    {
        playerObj = GameObject.Find("Player").GetComponent<Player>();
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void Interact()
    {
        if (playerObj.inventory.heldItem != null)
        {
            if (playerObj.inventory.heldItem.itemType == requiredItem)
            {
                dataManager.SucceededUse(name.ToString(), requiredItem.ToString());
                Unlock();
            }
            else
            {
                dataManager.FailedUse(name, playerObj.inventory.heldItem.itemType.ToString(), requiredItem.ToString());
            }
        }
        else
        {
            dataManager.FailedUse(name, "Empty Hand", requiredItem.ToString());
        }
    }

    public void Unlock()
    {
        OnToggle?.Invoke(this, EventArgs.Empty);
        playerObj.inventory.UseItem(playerObj.inventory.heldItem);
    }
}
