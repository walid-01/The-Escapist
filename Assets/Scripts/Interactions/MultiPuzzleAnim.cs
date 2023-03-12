using System;
using System.Collections;
using UnityEngine;

public class MultiPuzzleAnim : MonoBehaviour
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject keyLock;
    [SerializeField] private string animationOn;
    [SerializeField] private string animationOff;

    private IUnlockable firstUnlockable;
    private IUnlockable secondUnlockable;

    private bool pauseInteraction = false;
    private Animator animator;
    private bool isOn = false;
    private bool keyLockUnlocked = false;
    private bool triggerUnlocked = false;

    //optimizable -> can group with other classes that have same functionality
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        firstUnlockable = trigger.GetComponent<IUnlockable>();
        firstUnlockable.OnToggle += ToggleTrigger;

        secondUnlockable = keyLock.GetComponent<IUnlockable>();
        secondUnlockable.OnToggle += UnlockKeyLock;
    }

    private void Update()
    {
        if (keyLockUnlocked && triggerUnlocked)
        {
            if (!isOn)
            {
                PlayAnimation();
            }
        }
        else
        {
            if (isOn)
            {
                PlayAnimation();
            }
        }
    }

    private void UnlockKeyLock(object sender, EventArgs e)
    {
        keyLockUnlocked = true;
        secondUnlockable.OnToggle -= UnlockKeyLock;
    }
    private void ToggleTrigger(object sender, EventArgs e) => triggerUnlocked = !triggerUnlocked;

    public void PlayAnimation()
    {
        if (!pauseInteraction)
        {
            if (!isOn)
            {
                animator.Play(animationOn, 0, 0.0f);
                isOn = !isOn;
            }
            else
            {
                animator.Play(animationOff, 0, 0.0f);
                isOn = !isOn;
            }
            StartCoroutine(PauseInteraction());
        }
    }
    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(1f);
        pauseInteraction = false;
    }
}
