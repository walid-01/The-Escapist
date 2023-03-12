using System;
using TMPro;
using UnityEngine;

public class ICodeLock : MonoBehaviour, IUnlockable
{
    [SerializeField] private string realCode;
    private int currentChar = 0;
    private string attemptedCode;
    public event EventHandler OnToggle;
    public TextMeshProUGUI display;
    private bool unlocked = false;
    private DataManager dataManager;


    private void Start()
    {
        display.text = "";
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    public void AddValue(string value)
    {
        if (!unlocked && currentChar < realCode.Length)
        {
            attemptedCode += value;
            currentChar++;
            display.text = attemptedCode;
        }        
    }

    public void Unlock()
    {
        if (!unlocked && attemptedCode == realCode)
        {
            dataManager.SucceededCode(name);
            OnToggle?.Invoke(this, EventArgs.Empty);
            unlocked = true;
        }
        else
        {
            ResetCode();
        }
    }

    public void ResetCode()
    {
        if (!unlocked)
        {
            dataManager.FailedCode(name, attemptedCode, realCode);
            attemptedCode = "";
            currentChar = 0;
            display.text = "";
        }
    }
}
