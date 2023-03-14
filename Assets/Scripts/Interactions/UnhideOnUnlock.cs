using System;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObj : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectsList;

    private void Awake()
    {
        GetComponent<IUnlockable>().OnToggle += Unhide;
    }

    private void Unhide(object sender, EventArgs e)
    {
        foreach (GameObject gameObject in gameObjectsList)
        {
            gameObject.SetActive(true);
        }
        GetComponent<IUnlockable>().OnToggle -= Unhide;
        this.tag = "Untagged";
    }
}
