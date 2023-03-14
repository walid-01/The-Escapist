using UnityEngine;

public class IPush : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isHorizontal;
    [SerializeField] private bool direction;
    private PushableCrate pushableCrate;
    private Player playerObj;

    private void Start()
    {
        pushableCrate = transform.parent.GetComponentInParent<PushableCrate>();
        playerObj = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Interact()
    {
        if (!Player.heldPushable)
        {
            pushableCrate.SetDirection(isHorizontal, direction);
            pushableCrate.Push();
            playerObj.inventory.HoldItem(null);
        }
        else
        {
            pushableCrate.Release();
        }
    }
}
