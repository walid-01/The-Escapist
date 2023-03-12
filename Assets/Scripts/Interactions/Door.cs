using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator doorAnim;
    private readonly float waitTimer = 1.2f;
    public bool isOpen = false;
    private bool pauseInteraction = false;

    [SerializeField] bool isOutward;
    private string openDoorAnimation;
    private string closeDoorAnimation;

    private DataManager dataManager;

    private void Start()
    {
        doorAnim = gameObject.GetComponentInParent<Animator>();
        if (!isOutward)
        {
            openDoorAnimation = "OpenDoor";
            closeDoorAnimation = "CloseDoor";
        }
        else
        {
            openDoorAnimation = "OpenDoorOutward";
            closeDoorAnimation = "CloseDoorOutward";
        }

        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void PlayAnimation()
    {
        if (!pauseInteraction)
        {
            if (!isOpen)
            {
                OpenDoor();
                dataManager.SucceededInteract(name, true);

            }
            else
            {
                CloseDoor();
                dataManager.SucceededInteract(name, false);

            }

            StartCoroutine(PauseInteraction());
        }
        else
        {
            dataManager.PausedInteractionInteract(name);
        }
    }

    public void OpenDoor()
    {
        doorAnim.Play(openDoorAnimation, 0, 0.0f);
        isOpen = true;
    }

    public void CloseDoor()
    {
        doorAnim.Play(closeDoorAnimation, 0, 0.0f);
        isOpen = false;
    }

    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }
}
