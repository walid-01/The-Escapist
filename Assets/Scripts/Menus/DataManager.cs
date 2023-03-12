using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] int level;

    public GameObject db;
    private DataBase database;

    public static string lastEntered = "nothing";
    public static string lastExited = "nothing";

    private int numberOfAreaSwitches = 0;

    private int totalInteractions = 0;
    private int nbOfFailedInteractions = 0;
    private int nbOfSucceededInteractions = 0;

    private int timesHoldBox = 0;
    private int timesTriggered = 0;

    private int timesPicked = 0;

    private int totalFailedCodes = 0;
    private int totalSucceededCodes = 0;
    private int nbOfFailedCodes = 0;

    private float areaPartialTime;
    private float totalTime;

    private float travelledDistance = 0;
    private float partialTravelledDistance = 0;
    private Vector3 previousLoc;

    [SerializeField] GameObject levelManager;
    private LevelManager lm;

    private void Awake()
    {
        lm = levelManager.GetComponent<LevelManager>();
        //Debug.Log((int)lm.diff);

        previousLoc = gameObject.transform.position;

        database = db.GetComponent<DataBase>();
        database.CreateDB();
        database.CreateSession(level, (int)lm.diff);
    }

    private void Update()
    {
        totalTime += Time.deltaTime;
        areaPartialTime += Time.deltaTime;

        CalculateDistance();

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("saving...");
            SaveVariables(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SaveVariables(false);
        }
    }

    public void IncrementSwitch()
    {
        if (lastExited != lastEntered)
        {
            numberOfAreaSwitches++;
            Debug.Log("IncrementSwitch");

            try
            {
                database.SaveAreaSwitch(numberOfAreaSwitches, lastExited, lastEntered, areaPartialTime, totalTime, partialTravelledDistance, travelledDistance);
            }
            catch (Exception e)
            {
                Debug.Log("error calling save area: " + e.Message);
            }

            areaPartialTime = 0;
            partialTravelledDistance = 0;
        }
    }

    private void CalculateDistance()
    {
        travelledDistance += Vector3.Distance(transform.position, previousLoc);
        partialTravelledDistance += Vector3.Distance(transform.position, previousLoc);
        previousLoc = transform.position;
    }

    public void SucceededUse(string gameobjectName, string heldItem)
    {
        Debug.Log("SucceededUse");

        database.SaveSucceededUse(gameobjectName, heldItem, totalTime);
        nbOfSucceededInteractions++;
        totalInteractions++;
    }

    public void FailedUse(string gameobjectName, string heldItem, string requiredItem)
    {
        Debug.Log("FailedUse");

        database.FailedUse(gameobjectName, requiredItem, heldItem, totalTime);
        nbOfFailedInteractions++;
        totalInteractions++;
    }

    public void SucceededInteract(string gameobjectName, bool TurnedOn)
    {
        Debug.Log("SucceededInteract");

        database.SaveSucceededInteract(gameobjectName, TurnedOn, totalTime);
        nbOfSucceededInteractions++;
        totalInteractions++;
    }

    public void PausedInteractionInteract(string gameobjectName)
    {
        //Single Value
        Debug.Log("Interacted with " + gameobjectName + ", while it was on pausedInteraction");
        totalInteractions++;
    }

    public void FailedCode(string gameobjectName, string attemptedCode, string realCode)
    {
        Debug.Log("FailedCode");

        database.SaveFailedCode(gameobjectName, nbOfFailedCodes, attemptedCode, realCode, totalTime);
        totalFailedCodes++;
        nbOfFailedCodes++;
        totalInteractions++;
    }

    public void SucceededCode(string gameobjectName)
    {
        Debug.Log("SucceededCode");

        database.SaveSucceededCode(gameobjectName, nbOfFailedCodes, totalTime);
        nbOfSucceededInteractions++;
        totalSucceededCodes++;
        totalInteractions++;
        nbOfFailedCodes = 0;
    }

    public void BoxHold(string gameobjectName, bool isHold)
    {
        Debug.Log("BoxHold");
        totalInteractions++;

        database.SaveBoxInfos(gameobjectName, isHold, totalTime);
        if (isHold)
        {
            timesHoldBox++;
        }
    }

    public void BoxPut(string triggerName, string gameobjectName, bool isPut)
    {
        Debug.Log("BoxPut");
        totalInteractions++;

        database.SaveBoxTrigger(gameobjectName, triggerName, isPut, totalTime);
        if (isPut)
        {
            timesTriggered++;
        }
    }

    public void PickedItem(string itemName)
    {
        Debug.Log("PickedItem");
        totalInteractions++;

        database.SavePickedItem(itemName, totalTime);
        timesPicked++;
    }

    public void SaveVariables(bool isFinished)
    {
        Debug.Log("SaveVariables");

        database.UpdateVariables(isFinished, numberOfAreaSwitches, totalInteractions, nbOfSucceededInteractions, nbOfFailedInteractions, timesHoldBox, timesTriggered, timesPicked, totalSucceededCodes, totalFailedCodes, totalTime, travelledDistance);
    }
}

