using UnityEngine;

public class PushableCrate : MonoBehaviour
{
    private CharacterController controller;
    private Transform playerObj;

    public bool isHorizontal;
    public bool direction;

    private Vector3 move;
    private float z;

    private DataManager dataManager;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerObj = GameObject.Find("Player").transform;

        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void SetDirection(bool _isHorizontal, bool _direction)
    {
        isHorizontal = _isHorizontal;
        direction = _direction;
    }

    private void Update()
    {
        if (Player.heldPushable == this)
        {
            z = Input.GetAxis("Vertical");

            if (isHorizontal)
            {
                move = z * transform.right;
            }
            else
            {
                move = z * transform.forward;
            }

            if (!direction)
            {
                move *= -1;
            }

            controller.Move(-2f * Time.deltaTime * move);
        }
    }

    public void Push()
    {
        if (Player.isGrounded && !Player.isCrouching && !Player.isRunning)
        {
            dataManager.BoxHold(name, true);

            playerObj.transform.parent = transform;

            Player.heldPushable = this;
            Player.enablePrimaryMovements = false;
        }
    }

    public void Release()
    {
        dataManager.BoxHold(name, false);
        playerObj.transform.parent = null;

        Player.heldPushable = null;
        Player.enablePrimaryMovements = true;
    }
}
