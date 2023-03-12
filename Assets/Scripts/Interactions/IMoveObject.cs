using UnityEngine;

public class IMoveObject : MonoBehaviour, IInteractable
{
    private Transform mover;
    private Player playerObj;
    private Rigidbody objRig;
    private float distance;
    private DataManager dataManager;

    private void Start()
    {
        objRig = gameObject.GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player").GetComponent<Player>();
        mover = GameObject.Find("Mover").transform;

        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    private void Update()
    {
        if (Player.heldObj == transform.gameObject)
        {
            CalculateDistance();
        }
    }

    public void Interact()
    {
        if (!Player.heldObj)
        {
            if (Player.isGrounded && !Player.isCrouching && !Player.isRunning) PickUp();
        }
        else if (Player.heldObj == transform.gameObject)
        {
            Drop();
        }
    }

    void PickUp()
    {
        dataManager.BoxHold(name, true);

        playerObj.inventory.HoldItem(null);

        objRig.useGravity = false;
        objRig.transform.parent = mover;

        Player.heldObj = objRig.transform.gameObject;
        Player.enableSecondaryMovements = false;
    }

    void Drop()
    {
        dataManager.BoxHold(name, false);
        objRig.useGravity = true;
        objRig.transform.parent = null; //GameObject.Find("Ground").transform

        Player.heldObj = null;
        Player.enableSecondaryMovements = true;
    }

    void CalculateDistance()
    {
        distance = Vector3.Distance(mover.parent.position, objRig.position);
        //Debug.Log(distance);
        if (distance < 1.25f)
        {
            //when the player is too close he cant pick it up
            Drop();
            Debug.Log("too close");
            distance = 0;
        }
        else if (distance > 2.5f)
        {
            Drop();
            Debug.Log("too far");
            distance = 0;
        }
        else
        {
            //resets the object to the move
            Vector3 moveDirection = (mover.position - Player.heldObj.transform.position);
            Player.heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * 150f);
        }
        //stabilize the object when being held
        objRig.velocity = Vector3.zero;
        objRig.angularVelocity = Vector3.zero;
    }
}