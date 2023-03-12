using UnityEngine;

public class IButtonInput : MonoBehaviour, IInteractable
{
    private string value;
    private ICodeLock codeLock;

    void Start()
    {
        value = transform.name;
        codeLock = GetComponentInParent<ICodeLock>();
    }

    public void Interact()
    {
        codeLock.AddValue(value);
    }
}
