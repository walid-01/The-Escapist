using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Scripting.Python;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class LevelManager : MonoBehaviour
{
    public bool calculateDifficulty;

    public GameObject db;
    private DataBase database;

    public string pythonExeLocation;
    public string pythonFileLocation;

    private string path;

    List<string> latestSession;

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


    void Start()
    {
        path = Application.dataPath;
        pythonFileLocation = path + "/Python/pythonPredict.py";
        //Debug.Log(pythonFileLocation);

        database = db.GetComponent<DataBase>();
        database.CreateDB();
        latestSession = database.GetRow(database.GetActiveSessionID() -1);

        if (calculateDifficulty)
        {
            foreach (string item in latestSession)
            {
                Debug.Log(item);
            }
            RunPythonScript();
        }

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

    public void RunPythonScript()
    {
        //Create Process Info
        ProcessStartInfo psi = new ProcessStartInfo();

        //Provide script and arguments
        psi.FileName = @pythonExeLocation;
        psi.Arguments = $"\"{pythonFileLocation}\" \"{latestSession[0]}\" \"{latestSession[1]}\" \"{latestSession[2]}\" \"{latestSession[3]}\" \"{latestSession[4]}\" " +
            $"\"{latestSession[5]}\" \"{latestSession[6]}\" \"{latestSession[7]}\" \"{latestSession[8]}\" \"{latestSession[9]}\" \"{latestSession[10]}\" \"{latestSession[11]}\"";

        //Process Config
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        //Getting Output
        string errors = "";
        string results = "";

        using (var process = Process.Start(psi))
        {
            errors = process.StandardError.ReadToEnd();
            results = process.StandardOutput.ReadToEnd();
        }

        //Displaying output
        Debug.Log("Errors: " + errors);
        Debug.Log("Results: " + results);

        try
        {
            diff = int.Parse(results) switch
            {
                2 => difficulty.easy,
                1 => difficulty.medium,
                0 => difficulty.hard,
                _ => difficulty.easy,
            };
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
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
