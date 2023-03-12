using System;
using System.Collections;
using UnityEngine;

public class DoubleAnimationController : MonoBehaviour
{
    [SerializeField] private ILever firstUnlocker;
    [SerializeField] private ILever secondUnlocker;
    [SerializeField] private string animationOn;
    [SerializeField] private string animationOff;
    [SerializeField] private float animationDuration;

    private Animator animator;
    [SerializeField] private bool isOn = false;
    private bool pauseInteraction = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        firstUnlocker.OnToggle += ChangeAnimation;
        secondUnlocker.OnToggle += ChangeAnimation;
    }

    public void ChangeAnimation(object sender, EventArgs e)
    {
        if (!pauseInteraction)
        {
            if (isOn)
            {
                animator.Play(animationOff, 0, 0.0f);
            }
            else
            {
                animator.Play(animationOn, 0, 0.0f);
            }
            isOn = !isOn;
            StartCoroutine(PauseInteraction());
        }
    }

    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(animationDuration);
        pauseInteraction = false;
    }
}
