using System;
using System.Collections;
using UnityEngine;

public class ILever : MonoBehaviour, IInteractable, IUnlockable
{
    public event EventHandler OnToggle;
    public bool state = false;
    private bool pauseInteraction = false;
    private DataManager dataManager;

    private void Start()
    {
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void Interact()
    {
        if (!pauseInteraction)
        {
            state = !state;
            dataManager.SucceededInteract(name, state);
            Unlock();
        }
        else
        {
            dataManager.PausedInteractionInteract(name);
        }
    }

    public void Unlock()
    {
        OnToggle?.Invoke(this, EventArgs.Empty);
        StartCoroutine(PauseInteraction());
    }

    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(1f);
        pauseInteraction = false;
    }
}
