using System;
using UnityEngine;

public class AddItemOnInteract : MonoBehaviour
{
    [SerializeField] private GameObject unlocker;
    [SerializeField] private Item.ItemType item;
    private IUnlockable unlockable;

    private Player playerObj;

    private void Awake()
    {
        playerObj = GameObject.Find("Player").GetComponent<Player>();
        
        unlockable = GetComponent<IUnlockable>();
        unlockable.OnToggle += Remove;
    }

    private void Remove(object sender, EventArgs e)
    {
        playerObj.inventory.AddItem(new Item { itemType = item });
    }
}
