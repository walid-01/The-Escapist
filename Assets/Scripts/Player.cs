using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float runningSpeed = 3f;
    [SerializeField] private float crouchinSpeed = 3f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask environementLayer;

    private float speed;

    public static bool isGrounded = true;
    public static bool isCrouching = false;
    public static bool isRunning = false;
    public static bool enablePrimaryMovements = true;
    public static bool enableSecondaryMovements = true;
    public static bool enableInteractions = true;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 posToShift = new Vector3(0, 0.5f, 0);

    private bool ceilingClear = true;
    private bool wantsToStand = false;
    private bool canJumpUpper = false;
    private bool canJumpLower = false;

    private readonly float gravity = -25f;
    private readonly float jumpHeight = 2f;
    private readonly float crouchHeight = 1f;
    private readonly float standingHeight = 1.8f;
    [SerializeField] private float distToGround = 1f;

    private RaycastHit groundHit;
    private RaycastHit lowerStepHit;
    private RaycastHit upperStepHit;
    private RaycastHit environementHit;

    public Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    public IInteractable interactableRaycastedObj;

    public static GameObject heldObj;
    public static PushableCrate heldPushable;

    public Image crosshair;
    public Sprite handSprite;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        speed = walkingSpeed;
    }

    void Update()
    {
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        if (enablePrimaryMovements)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //moving around
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(speed * Time.deltaTime * move);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (enableSecondaryMovements)
            {
                //jumping crouching, and sprinting
                if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isCrouching) velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);

                if (isGrounded && isCrouching) speed = crouchinSpeed;
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = runningSpeed;
                    isRunning = true;
                }
                else
                {
                    speed = walkingSpeed;
                    isRunning = false;
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    Crouch();
                    wantsToStand = false;
                }
                else if (Input.GetKeyUp(KeyCode.C)) wantsToStand = true;

                if (wantsToStand && isCrouching && ceilingClear)
                {
                    StandUp();
                    wantsToStand = false;
                }

                inventory.SelectItem();
            }
        }

        controller.stepOffset = (!canJumpUpper && canJumpLower && isGrounded) ? 0.7f : 0f;

        GroundedCheck();
        CeilingCheck();
        UpperStepCheck();
        LowerStepCheck();
        InteractableCheck();
    }

    private void Crouch()
    {
        //crouching
        isCrouching = true;
        controller.height = crouchHeight;
        distToGround = 0.6f;
        groundChecker.position += posToShift;
    }

    private void StandUp()
    {
        //standing up
        isCrouching = false;
        controller.height = standingHeight;
        distToGround = 1f;
        groundChecker.position -= posToShift;
    }

    //Player standing on Ground ?
    private void GroundedCheck()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out groundHit, distToGround))
        {
            if (!groundHit.transform.CompareTag("Trigger"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * distToGround, Color.red);
                isGrounded = true;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * distToGround, Color.green);
            isGrounded = false;
        }
    }

    //Ceiling above the player is clear ?
    private void CeilingCheck()
    {
        if (Physics.Raycast(transform.position - new Vector3(0f, -0.6f, 0f), transform.TransformDirection(Vector3.up), out groundHit, 0.75f, environementLayer))
        {
            ceilingClear = false;
            Debug.DrawRay(transform.position - new Vector3(0f, -0.6f, 0f), transform.TransformDirection(Vector3.up) * groundHit.distance, Color.red);
        }
        else
        {
            ceilingClear = true;
            Debug.DrawRay(transform.position - new Vector3(0f, -0.6f, 0f), transform.TransformDirection(Vector3.up) * 0.75f, Color.green);
        }
    }

    //Upper Step Height Check
    private void UpperStepCheck()
    {
        if (Physics.Raycast(transform.position - new Vector3(0f, 0.3f, 0f), transform.TransformDirection(Vector3.forward), out upperStepHit, 0.75f, environementLayer))
        {
            canJumpUpper = true;
            Debug.DrawRay(transform.position - new Vector3(0f, 0.3f, 0f), transform.TransformDirection(Vector3.forward) * upperStepHit.distance, Color.red);
        }
        else
        {
            canJumpUpper = false;
            Debug.DrawRay(transform.position - new Vector3(0f, 0.3f, 0f), transform.TransformDirection(Vector3.forward) * 0.75f, Color.green);
        }
    }

    //Lower Step Height Check
    private void LowerStepCheck()
    {
        if (Physics.Raycast(transform.position - new Vector3(0f, 0.6f, 0f), transform.TransformDirection(Vector3.forward), out lowerStepHit, 2f, environementLayer))
        {
            canJumpLower = true;
            Debug.DrawRay(transform.position - new Vector3(0f, 0.6f, 0f), transform.TransformDirection(Vector3.forward) * lowerStepHit.distance, Color.red);
        }
        else
        {
            canJumpLower = false;
            Debug.DrawRay(transform.position - new Vector3(0f, 0.6f, 0f), transform.TransformDirection(Vector3.forward) * 2f, Color.green);
        }
    }

    private void InteractableCheck()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out environementHit, 2f, environementLayer) && environementHit.transform.CompareTag("Interactable"))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * environementHit.distance, Color.red);

            interactableRaycastedObj = environementHit.collider.GetComponent<IInteractable>();
            ChangeCosshair(handSprite, new Vector2(32, 32));

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                interactableRaycastedObj.Interact();
            }
        }
        else
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 2f, Color.green);
            ChangeCosshair(null, new Vector2(6, 6));
        }
    }

    private void ChangeCosshair(Sprite sprite, Vector2 size)
    {
        crosshair.GetComponent<Image>().sprite = sprite;
        crosshair.GetComponent<RectTransform>().sizeDelta = size;
    }
}

