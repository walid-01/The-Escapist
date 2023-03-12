using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class DataBase : MonoBehaviour
{
    private readonly string dbName = "URI=file:gameDB.db";
    private IDbConnection dbconnnection;
    private IDbCommand command;
    private IDataReader reader;

    private string path;

    private void Start()
    {
        CreateDB();
        path = Application.dataPath;
    }

    public void CreateDB()
    {
        OpenDB();
        try
        {
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS Players(playerID INTEGER PRIMARY KEY, username TEXT NOT NULL UNIQUE, password TEXT NOT NULL, age INT NOT NULL);" +
                "CREATE TABLE IF NOT EXISTS ActiveUser(id INTEGER PRIMARY KEY);" +
                "CREATE TABLE IF NOT EXISTS Session(sessionID INTEGER PRIMARY KEY, playerID INTEGER NOT NULL, isFinished INTEGER NOT NULL, level INTEGER NOT NULL, difficulty INTEGER NOT NULL, areaSwitches INTEGER NOT NULL, interactions INTEGER NOT NULL, succeededInteractions INTEGER NOT NULL, failedInteractions INTEGER NOT NULL, timesHoldBoxes INTEGER NOT NULL, " +
                "timesTriggeredTriggers INTEGER NOT NULL, pickedItems INTEGER NOT NULL, succeededCodes INTEGER NOT NULL, failedCodes INTEGER NOT NULL, totalTime float NOT NULL, travelledDistance float NOT NULL, FOREIGN KEY(playerID) REFERENCES Players(playerID));" +
                "CREATE TABLE IF NOT EXISTS AreaSwitches(switchID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, areaSwitchNumber INTEGER NOT NULL, lastExited TEXT NOT NULL, lastEntered TEXT NOT NULL, areaTime FLOAT NOT NULL, totalTime FLOAT NOT NULL, areaTraveledDistance FLOAT NOT NULL, totalTraveledDistance FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS SucceededUse(succeededUseID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, requiredItem TEXT NOT NULL, useTime FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS FailedUse(failedUseID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, requiredItem TEXT NOT NULL, usedItem TEXT NOT NULL, useTime FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS SucceededInteract(succeededInteractID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, turnedOn BOOLEAN, interactionTime FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS SucceededCode(succeededCodeID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, numberOfAttempts INT NOT NULL, interactionTime FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS FailedCode(ailedCodeID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, attemptNumber INTEGER NOT NULL, attemptedCode TEXT NOT NULL, actualCode TEXT NOT NULL, interactionTime FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS BoxHold(boxHoldID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, time FLOAT NOT NULL, isHold BOOLEAN NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS BoxTrigger(boxTriggerID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, triggerName TEXT NOT NULL, isPut BOOLEAN NOT NULL, time FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));" +
                "CREATE TABLE IF NOT EXISTS PickedItem(pickedItemID INTEGER PRIMARY KEY, sessionID INTEGER NOT NULL, gameObjectName TEXT NOT NULL, pickedTime FLOAT NOT NULL, FOREIGN KEY(sessionID) REFERENCES Session(sessionID));";

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        CloseDB();

        int count = -1;

        try
        {
            OpenDB();
            command.CommandText = "SELECT COUNT(*) FROM ActiveUser;";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }

            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        try
        {
            OpenDB();
            if (count == 0)
            {
                Debug.Log("has been added");
                command.CommandText = "insert into ActiveUser(id) values(-1);";
                command.ExecuteNonQuery();
            }
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        try
        {
            OpenDB();
            command.CommandText = "SELECT COUNT(*) FROM ActiveUser;";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        //Debug.Log("count = " + count);
    }

    public int DBSignUp(string username, string password, string age)
    {
        int result = 0;
        OpenDB();
        try
        {
            command.CommandText = "INSERT INTO Players(username, password, age) VALUES('" + username + "', '" + password + "', '" + int.Parse(age) + "');";
            result = command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        CloseDB();
        if (result != 0)
        {
            SetActiveId(GetNewPlayerId());
        }
        return result;
    }

    public bool DBLogin(string username, string password)
    {
        int id = -1;
        OpenDB();
        try
        {
            command.CommandText = "SELECT playerID FROM Players WHERE username=='" + username + "' AND password=='" + password + "';";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                id = reader.GetInt32(0);
                Debug.Log(id);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        CloseDB();

        if (id != -1)
        {
            Debug.Log("setting id");
            SetActiveId(id);
            return true;
        }
        return false;
    }

    public void CreateSession(int level, int difficulty)
    {
        try
        {
            int id = GetActiveId();
            OpenDB();
            command.CommandText = "insert into Session(playerID, isFinished, level, difficulty, areaSwitches, interactions, succeededInteractions, failedInteractions, timesHoldBoxes, timesTriggeredTriggers, pickedItems, succeededCodes, failedCodes, totalTime, travelledDistance) " +
                "values(" + id + ", 0, " + level + ", " + difficulty + ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0, 0.0);";
            command.ExecuteNonQuery();
            CloseDB();
            Debug.Log("New Session Created");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void SaveAreaSwitch(int numberOfAreaSwitches, string lastExited, string lastEntered, float areaPartialTime, float toatlTime, float partialTravelledDistance, float travelledDistance)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("active id : " + id);
            command.CommandText = "INSERT INTO AreaSwitches(sessionID, areaSwitchNumber, lastExited, lastEntered, areaTime, totalTime, areaTraveledDistance, totalTraveledDistance) VALUES('" + id + "', '" + numberOfAreaSwitches + "', '" + lastExited + "', '" + lastEntered + "', '" + areaPartialTime + "', '" + toatlTime + "', '" + partialTravelledDistance + "', '" + travelledDistance + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SaveSucceededUse(string gameObjectName, string requiredItem, float useTime)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("active id : " + id);
            command.CommandText = "INSERT INTO SucceededUse(sessionID, gameObjectName, requiredItem, useTime) VALUES('" + id + "', '" + gameObjectName + "', '" + requiredItem + "', '" + useTime + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void FailedUse(string gameObjectName, string requiredItem, string usedItem, float useTime)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("active id : " + id);
            command.CommandText = "INSERT INTO FailedUse(sessionID, gameObjectName, requiredItem, usedItem, useTime) VALUES('" + id + "', '" + gameObjectName + "', '" + requiredItem + "', '" + usedItem + "', '" + useTime + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SaveSucceededInteract(string gameObjectName, bool turnedOn, float useTime)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("session id : " + id);
            command.CommandText = "INSERT INTO SucceededInteract(sessionID, gameObjectName, turnedOn, interactionTime) VALUES('" + id + "', '" + gameObjectName + "', '" + turnedOn + "', '" + useTime + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SaveSucceededCode(string gameObjectName, int numberOfattempts, float useTime)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("session id : " + id);
            command.CommandText = "INSERT INTO SucceededCode(sessionID, gameObjectName, numberOfattempts, interactionTime) VALUES('" + id + "', '" + gameObjectName + "', '" + numberOfattempts + "', '" + useTime + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SaveFailedCode(string gameObjectName, int attemptNumber, string attemptedCode, string actualCode, float useTime)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("session id : " + id);
            command.CommandText = "INSERT INTO FailedCode(sessionID, gameObjectName, attemptNumber, attemptedCode, actualCode, interactionTime) " +
                                    "VALUES('" + id + "', '" + gameObjectName + "', '" + attemptNumber + "', '" + attemptedCode + "', '" + actualCode + "', '" + useTime + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SaveBoxInfos(string gameObjectName, bool isHold, float time)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("session id : " + id);
            command.CommandText = "INSERT INTO BoxHold(sessionID, gameObjectName, isHold, time) " +
                                    "VALUES('" + id + "', '" + gameObjectName + "', '" + isHold + "', '" + time + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SaveBoxTrigger(string gameObjectName, string triggerName, bool isPut, float time)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("session id : " + id);
            command.CommandText = "INSERT INTO BoxTrigger(sessionID, gameObjectName, triggerName, isPut, time) " +
                                    "VALUES('" + id + "', '" + gameObjectName + "', '" + triggerName + "', '" + isPut + "', '" + time + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SavePickedItem(string gameObjectName, float pickTime)
    {
        try
        {
            int id = GetActiveSessionID();
            OpenDB();

            //Debug.Log("session id : " + id);
            command.CommandText = "INSERT INTO PickedItem(sessionID, gameObjectName, pickedTime) " +
                                    "VALUES('" + id + "', '" + gameObjectName + "', '" + pickTime + "');";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void UpdateVariables(bool isFinished, int areaSwitches, int interactions, int succeededInteractions, int failedInteractions, int timesHoldBoxes, int timesTriggeredTriggers, int pickedItems, int succeededCodes, int failedCodes, float totalTime, float travelledDistance)
    {
        int finished = (isFinished) ? 1 : 0;
        try
        {
            int id = GetActiveSessionID();
            OpenDB();
            command.CommandText = "UPDATE Session SET isFinished =" + finished + ", areaSwitches =" + areaSwitches + ", interactions =" + interactions + ", succeededInteractions =" + succeededInteractions + ", failedInteractions =" + failedInteractions + ", timesHoldBoxes =" + timesHoldBoxes + ", timesTriggeredTriggers =" + timesTriggeredTriggers + ", pickedItems =" + pickedItems + ", succeededCodes =" + succeededCodes + ", failedCodes =" + failedCodes + ", totalTime =" + totalTime + ", travelledDistance =" + travelledDistance + " WHERE sessionID=" + id + " ;";
            command.ExecuteNonQuery();
            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void DisplayAccounts()
    {
        try
        {
            OpenDB();

            command.CommandText = "SELECT * FROM Players;";
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Debug.Log("id: " + reader["playerID"] + "\tusername: " + reader["username"] + "\tpassword: " + reader["password"] + "\tage: " + reader["age"]);
                }
            }

            CloseDB();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    private int GetNewPlayerId()
    {
        int id = 1;
        OpenDB();
        try
        {
            command.CommandText = "SELECT playerID FROM Players ORDER BY playerID DESC limit 1;";
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                id = reader.GetInt32(0);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("GetNewPlayerId():" + e.Message);
        }
        CloseDB();
        return id;
    }

    private void OpenDB()
    {
        try
        {
            dbconnnection = new SqliteConnection(dbName);
            dbconnnection.Open();
            command = dbconnnection.CreateCommand();
        }
        catch (Exception e)
        {
            Debug.LogWarning("openDB" + e.Message);
        }
    }

    private void CloseDB()
    {
        try
        {
            dbconnnection.Close();
            //command.Dispose();
            //if (reader.IsClosed == false)
            //{
            //    reader.Close();
            //}
        }
        catch (Exception e)
        {
            Debug.LogWarning("closeDB " + e.Message);
        }
    }

    public int GetActiveSessionID()
    {
        int sessionId = -1;

        OpenDB();
        try
        {
            command.CommandText = "SELECT sessionID FROM Session ORDER BY sessionID DESC limit 1;";
            reader = command.ExecuteReader();
            if (reader.Read())
                sessionId = reader.GetInt32(0);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        CloseDB();

        return sessionId;
    }

    public List<string> GetRow(int ID)
    {
        List<string> row = new();
        OpenDB();
        try
        {
            command.CommandText = "SELECT * FROM Session WHERE sessionID='"+ ID +"'";
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //row.Add(float.Parse(reader["sessionID"].ToString()));
                    //row.Add(float.Parse(reader["playerID"].ToString()));
                    row.Add(reader["isFinished"].ToString());
                    row.Add(reader["areaSwitches"].ToString());
                    row.Add(reader["interactions"].ToString());
                    row.Add(reader["succeededInteractions"].ToString());
                    row.Add(reader["failedInteractions"].ToString());
                    row.Add(reader["timesHoldBoxes"].ToString());
                    row.Add(reader["timesTriggeredTriggers"].ToString());
                    row.Add(reader["pickedItems"].ToString());
                    row.Add(reader["succeededCodes"].ToString());
                    row.Add(reader["failedCodes"].ToString());
                    row.Add(reader["totalTime"].ToString());
                    row.Add(reader["travelledDistance"].ToString());
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        CloseDB();

        //foreach (float item in row)
        //{
        //    Debug.Log(item);
        //}
        return row;
    }

    public void WriteCSV()
    {
        List<string> row = GetRow(GetActiveSessionID());
        string csvFileName = path + "/Python/latestSession.csv";

        if (row.Count == 12)
        {
            TextWriter tw = new StreamWriter(csvFileName, false);
            //Debug.Log(string.Join(",", row).ToString());
            tw.WriteLine(string.Join(",", row));
            tw.Close();
        }
        else
        {
            Debug.Log("Error");
        }
    }

    public int GetActiveId()
    {
        int id = -1;
        OpenDB();
        try
        {
            command.CommandText = "select id from ActiveUser";
            reader = command.ExecuteReader();
            if (reader.Read())
                id = reader.GetInt32(0);

        }
        catch (Exception e)
        {
            Debug.LogWarning("GetActiveId(): " + e.Message);
        }
        CloseDB();
        //Debug.Log(id);
        return id;
    }

    public void SetActiveId(int id)
    {
        OpenDB();
        try
        {
            command.CommandText = "UPDATE ActiveUser SET id =" + id + ";";
            command.ExecuteNonQuery();

        }
        catch (Exception e)
        {
            Debug.LogWarning("SetActiveId(): " + e.Message);
        }
        CloseDB();
    }
}
