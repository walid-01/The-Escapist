using UnityEngine;

public class IButtonEnter : MonoBehaviour, IInteractable
{
    private ICodeLock codeLock;

    void Start()
    {
        codeLock = GetComponentInParent<ICodeLock>();
    }

    public void Interact()
    {
        codeLock.Unlock();
    }
}
