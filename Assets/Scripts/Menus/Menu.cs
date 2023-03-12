using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Menu : MonoBehaviour
{
    private DataBase database;

    [SerializeField] private Loader.Scene nextScene;

    //private DataManager dataManager;

    [Header("Others")]
    public GameObject db;
    public GameObject mainPanel;
    public GameObject logInPanel;
    public GameObject signUpPanel;

    [Header("Sign in")]
    public TMP_InputField liUsername;
    public TMP_InputField liPassword;
    //public TMP_Text liError;

    [Header("Sign up")]
    public TMP_InputField suUsername;
    public TMP_InputField suPassword;
    public TMP_InputField suCheckPassword;
    public TMP_InputField suAge;
    //public TMP_Text suError;

    private int userid;

    private void Start()
    {
        database = db.GetComponent<DataBase>();
        database.CreateDB();
        userid = database.GetActiveId();
        if (userid != -1)
        {
            mainPanel.SetActive(true);
            logInPanel.SetActive(false);
            signUpPanel.SetActive(false);
        }
    }

    public void Login()
    {
        bool accountExists;
        try
        {
            if (liUsername.text.Trim().Length == 0 || liPassword.text.Trim().Length == 0)
            {
                Debug.Log("Fill all the fields");
            }
            else
            {
                accountExists = database.DBLogin(liUsername.text, liPassword.text);
                if (accountExists)
                {
                    mainPanel.SetActive(true);
                    logInPanel.SetActive(false);
                }
                else
                {
                    Debug.Log("Account Not Found");
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void SignUp()
    {
        int result = 0;
        try
        {
            if (suUsername.text.Trim().Length == 0 || suPassword.text.Trim().Length == 0 || suCheckPassword.text.Trim().Length == 0 || suAge.text.Trim().Length == 0)
            {
                Debug.Log("Fill all the fields");
            }
            else if (suPassword.text != suCheckPassword.text)
            {
                Debug.Log("Passwords doesn't match");
            }
            else
            {
                result = database.DBSignUp(suUsername.text.Trim(), suPassword.text.Trim(), suAge.text.Trim());
                //Debug.Log("result " + result);
                if (result == 1)
                {
                    mainPanel.SetActive(true);
                    signUpPanel.SetActive(false);
                }
                else
                    Debug.Log("Username already exists");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void StartGame()
    {
        Loader.Load(nextScene);
    }

    public void LogOut()
    {
        mainPanel.SetActive(false);
        logInPanel.SetActive(true);
        database.SetActiveId(-1);
    }

    public void QuitApp()
    {
        Debug.Log("Quit");
        database.SetActiveId(-1);
        Application.Quit();
    }
}
