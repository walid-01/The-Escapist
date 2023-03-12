using System;
using UnityEngine;

public class MultiLevers : MonoBehaviour, IUnlockable
{
    public event EventHandler OnToggle;

    [SerializeField] ILever lever1;
    [SerializeField] ILever lever2;
    [SerializeField] ILever lever3;
    [SerializeField] ILever lever4;
    [SerializeField] ILever lever5;
    [SerializeField] ILever lever6;

    private bool isUnlocked = false;
    
    void Start()
    {
        lever1.GetComponent<IUnlockable>().OnToggle += CheckPattern;
        lever2.GetComponent<IUnlockable>().OnToggle += CheckPattern;
        lever3.GetComponent<IUnlockable>().OnToggle += CheckPattern;
        lever4.GetComponent<IUnlockable>().OnToggle += CheckPattern;
        lever5.GetComponent<IUnlockable>().OnToggle += CheckPattern;
        lever6.GetComponent<IUnlockable>().OnToggle += CheckPattern;
    }

    private void CheckPattern(object sender, EventArgs e)
    {
        if (!isUnlocked)
        {
            if (lever1.state == false && lever2.state == true && lever3.state == true && lever4.state == false && lever5.state == true && lever6.state == false)
            {
                Unlock();
                isUnlocked = true;
            }
        }
    }

    public void Unlock()
    {
        OnToggle?.Invoke(this, EventArgs.Empty);
    }
}
