using System;
using System.Collections;
using UnityEngine;

public class Trigger : MonoBehaviour, IUnlockable
{
    private DataManager dataManager;
    private bool pauseInteraction = false;
    private bool wantToPress = false;
    private bool wantToUnPress = false;
    private bool isOn = false;

    public event EventHandler OnToggle;

    private void Start()
    {
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            dataManager.BoxPut(name, other.name, true);
            wantToPress = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        dataManager.BoxPut(name, other.name, true);
        wantToUnPress = true;
    }

    

    private void Update()
    {
        Unlock();
    }

    public void Unlock()
    {
        if (!pauseInteraction)
        {
            if (!isOn)
            {
                if (wantToPress)
                {
                    wantToUnPress = false;
                    wantToPress = false;
                    isOn = true;
                    OnToggle?.Invoke(this, EventArgs.Empty);
                    StartCoroutine(PauseDoorInteraction());
                }
            }
            else
            {
                if (wantToUnPress)
                {
                    wantToPress = false;
                    wantToUnPress = false;
                    isOn = false;
                    OnToggle?.Invoke(this, EventArgs.Empty);
                    StartCoroutine(PauseDoorInteraction());
                }
            }
        }
    }

    private IEnumerator PauseDoorInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(1f);
        pauseInteraction = false;
    }
}
