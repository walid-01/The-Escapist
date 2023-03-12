using UnityEngine;

public class IKeyDoor : Door, IInteractable 
{
    [SerializeField] private Item.ItemType requiredItem;
    private DataManager dataManager;
    private Player playerObj;
    private bool requiresItem = true;
    
    private void Awake()
    {
        playerObj = GameObject.Find("Player").GetComponent<Player>();
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }
    
    public void Interact()
    {
        if (requiresItem)
        {
            if (playerObj.inventory.heldItem != null)
            {
                if (playerObj.inventory.heldItem.itemType == requiredItem)
                {
                    dataManager.SucceededUse(name, requiredItem.ToString());
                    requiresItem = false;
                    playerObj.inventory.UseItem(playerObj.inventory.heldItem);
                    PlayAnimation();
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
        else PlayAnimation();
    }
}
