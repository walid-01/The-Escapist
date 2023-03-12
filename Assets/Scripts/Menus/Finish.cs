using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private DataManager dataManager;

    public Loader.Scene nextScene;

    private void Start()
    {
        dataManager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        dataManager.SaveVariables(true);
        Loader.Load(nextScene);
    }
}
