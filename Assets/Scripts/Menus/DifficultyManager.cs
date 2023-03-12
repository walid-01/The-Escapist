using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum difficulty
    {
        easy,
        medium,
        hard
    }

    public difficulty diff;

    [SerializeField] List<GameObject> easyGameObjects;
    [SerializeField] List<GameObject> mediumGameObjects;
    [SerializeField] List<GameObject> hardGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        switch (diff)
        {
            case difficulty.easy:
                ShowGameObjects(easyGameObjects);
                break;
            case difficulty.medium:
                ShowGameObjects(mediumGameObjects);
                break;
            case difficulty.hard:
                ShowGameObjects(hardGameObjects);
                break;
            default:
                break;
        }
    }

    private void ShowGameObjects(List<GameObject> diffGameObjects)
    {
        foreach (GameObject gameObject in diffGameObjects)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
