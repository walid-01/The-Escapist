using System;
using UnityEngine;

public class IWorkbench : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject hiddenTorch;
    [SerializeField] GameObject hiddenStick;
    [SerializeField] GameObject hiddenCoal;

    private Player playerObj;
    private DataManager dataManager;

    private bool isCoalPut = false;
    private bool isStickPut = false;

    private void Awake()
    {
        playerObj = GameObject.Find("Player").GetComponent<Player>();
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void Interact()
    {
        if (playerObj.inventory.heldItem.itemType != default)
        {
            if (playerObj.inventory.heldItem.itemType == Item.ItemType.Coal)
            {
                dataManager.SucceededUse(name, Item.ItemType.Coal.ToString());

                playerObj.inventory.UseItem(playerObj.inventory.heldItem);
                hiddenCoal.SetActive(true);
                isCoalPut = true;
                Check();
            }
            else if (playerObj.inventory.heldItem.itemType == Item.ItemType.Stick)
            {
                dataManager.SucceededUse(name, Item.ItemType.Stick.ToString());

                playerObj.inventory.UseItem(playerObj.inventory.heldItem);
                hiddenStick.SetActive(true);
                isStickPut = true;
                Check();
            }
            else
            {
                CallFailedUse(playerObj.inventory.heldItem.itemType.ToString());
            }
        }
        else
        {
            CallFailedUse("Empty Hand");
        }
    }

    private void CallFailedUse(string heldItem)
    {
        if (!isStickPut && !isCoalPut)
        {
            dataManager.FailedUse(name, heldItem, Item.ItemType.Stick.ToString() + " " + Item.ItemType.Coal.ToString());
        }
        else if (!isCoalPut)
        {
            dataManager.FailedUse(name, heldItem, Item.ItemType.Coal.ToString());
        }
        else
        {
            dataManager.FailedUse(name, heldItem, Item.ItemType.Stick.ToString());
        }
    }

    private void Check()
    {
        if (isCoalPut && isStickPut)
        {
            tag = "Untagged";

            hiddenCoal.SetActive(false);
            hiddenStick.SetActive(false);
            hiddenTorch.SetActive(true);
        }
    }
}
