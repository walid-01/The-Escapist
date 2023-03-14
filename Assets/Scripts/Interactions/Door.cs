using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float animationDuration = 1.2f;
    private Animator animator;
    private DataManager dataManager;
    private bool pauseInteraction = false;

    private void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();

        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void PlayAnimation()
    {
        if (!pauseInteraction)
        {
            // dataManager.SucceededInteract(name, false);
            // Debug.Log(animator);
            // Debug.Log(gameObject.GetComponentInParent<Animator>());
            animator.SetTrigger("On Interact");

            StartCoroutine(PauseInteraction());
        }
        else
        {
            dataManager.PausedInteractionInteract(name);
        }
    }

    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(animationDuration);
        pauseInteraction = false;
    }
}
