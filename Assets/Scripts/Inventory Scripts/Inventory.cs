using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    private List<Item> itemList;
    public event EventHandler OnItemListChanged;
    public Item heldItem = null;

    public Inventory()
    {
        itemList = new List<Item>();

        //AddItem(new Item { itemType = Item.ItemType.Shovel });
        //AddItem(new Item { itemType = Item.ItemType.Axe });
        //AddItem(new Item { itemType = Item.ItemType.TorchOn });
        //AddItem(new Item { itemType = Item.ItemType.GreenKey });
        //AddItem(new Item { itemType = Item.ItemType.BlueKey });
        //AddItem(new Item { itemType = Item.ItemType.RedKey });
        //AddItem(new Item { itemType = Item.ItemType.Stick });
        //AddItem(new Item { itemType = Item.ItemType.Coal });
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void UseItem(Item item)
    {
        itemList.Remove(item);
        heldItem.itemType = Item.ItemType.Default;
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    //optimizable switch it ?
    public void SelectItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && itemList.Count > 0)
        {
            HoldItem(itemList[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && itemList.Count > 1)
        {
            HoldItem(itemList[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && itemList.Count > 2)
        {
            HoldItem(itemList[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && itemList.Count > 3)
        {
            HoldItem(itemList[3]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && itemList.Count > 4)
        {
            HoldItem(itemList[4]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && itemList.Count > 5)
        {
            HoldItem(itemList[5]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && itemList.Count > 6)
        {
            HoldItem(itemList[6]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && itemList.Count > 7)
        {
            HoldItem(itemList[7]);
        }

        if (Input.GetKeyDown(KeyCode.B) && heldItem != null)
        {
            Debug.Log(heldItem.itemType);
        }
    }

    public void HoldItem(Item holdItem)
    {
        foreach (Item item in itemList)
        {
            item.isActive = false;
        }

        if (heldItem == holdItem)
        {
            heldItem = null;
        }
        else 
        {
            heldItem = holdItem;
            if (holdItem != null) heldItem.isActive = true;
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
}
