using System;
using UnityEngine;

public class CodedDoor : Door
{
    [SerializeField] private ICodeLock codeLock;

    private void Awake()
    {
        codeLock.OnToggle += OpenDoor;
    }

    private void OpenDoor(object sender, EventArgs e)
    {
        PlayAnimation();
        codeLock.OnToggle -= OpenDoor;
    }
}
