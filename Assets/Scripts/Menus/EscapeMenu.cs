using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private Loader.Scene nextScene;

    public static bool isPaused = false;

    public GameObject pauseMenu;
    public GameObject inventoryBar;
    public GameObject crosshair;
    private DataManager datamanager;

    private void Start()
    {
        datamanager = GameObject.Find("Player").GetComponent<DataManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void ReturnHome()
    {
        Loader.Load(nextScene);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenu.SetActive(false);
        inventoryBar.SetActive(true);
        crosshair.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        datamanager.SaveVariables(false);

        Cursor.lockState = CursorLockMode.None;

        pauseMenu.SetActive(true);
        inventoryBar.SetActive(false);
        crosshair.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitApp()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
