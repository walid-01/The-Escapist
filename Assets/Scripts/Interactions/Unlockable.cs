using System;

public interface IUnlockable
{
    public event EventHandler OnToggle;

    void Unlock();
}
