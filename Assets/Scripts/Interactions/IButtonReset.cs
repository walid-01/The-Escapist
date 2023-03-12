using UnityEngine;

public class IButtonReset : MonoBehaviour, IInteractable
{
    private ICodeLock codeLock;

    void Start()
    {
        codeLock = GetComponentInParent<ICodeLock>();
    }

    public void Interact()
    {
        codeLock.ResetCode();
    }
}
