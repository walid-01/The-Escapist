using UnityEngine;

public class Area : MonoBehaviour
{
    private DataManager dataManager;

    private void Start()
    {
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("entered area " + gameObject.name);
            DataManager.lastEntered = gameObject.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("exiting area " + gameObject.name);
            DataManager.lastExited = gameObject.name;
            dataManager.IncrementSwitch();
        }
    }
}
