using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoreScript : MonoBehaviour {

    private string connectionString;

    // Use this for initialization
    void Start()
    {
        connectionString = "URI=file:" + Application.streamingAssetsPath + "/semogaBisa.db";
        GetScoresAndChangeText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GetScoresAndChangeText()
    {
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(connectionString);
        Debug.Log("Ini connection string yang di highscore :  " + connectionString);
        Debug.Log("getScore masuk  ");
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT * FROM highScore ORDER BY SKOR DESC LIMIT 5";

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        int i = 1;

        GameObject temp;
        Text tempText;

        while (reader.Read())
        {
            int value = reader.GetInt32(0);
            string name = reader.GetString(1);
            int rand = reader.GetInt32(2);

            Debug.Log("value= " + value + " name =" + name + " random =" + rand);

            temp = GameObject.Find("Name (" + i.ToString() + ")");
            tempText = temp.GetComponent<Text>();
            tempText.text = name;

            temp = GameObject.Find("Score (" + i.ToString() + ")");
            tempText = temp.GetComponent<Text>();
            tempText.text = rand.ToString();

            i++;
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void OnButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
