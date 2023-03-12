using UnityEngine;

public class IPickUp : MonoBehaviour, IInteractable
{
    public Item item;

    private DataManager dataManager;
    private GameObject player;
    private Player playerObj;

    void Start()
    {
        player = GameObject.Find("Player");
        playerObj = player.GetComponent<Player>();
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void Interact()
    {
        dataManager.PickedItem(item.itemType.ToString());
        playerObj.inventory.AddItem(item);
        Destroy(gameObject);
    }
}