using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] Loader.Scene nextScene;
    public void QuitApp()
    {
        Debug.Log("Quit");

        Application.Quit();
    }

    public void ReturnToMenu()
    {
        Loader.Load(nextScene);
    }
}
