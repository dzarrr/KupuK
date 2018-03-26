using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using System;
using Mono.Data.Sqlite;
using System.IO;
using TMPro;

public class TheStack : MonoBehaviour {
    public AudioSource myMusic;
    //public Text scoreText = null;
    public Color32[] gameColors = new Color32[4];
    public Material stackMat;
    public GameObject endPanel;

    public Text scoreText;

    public GameObject cameraEasy;
    //public GameObject cameraHard;

    AudioListener cameraEasyAudioLis;
    //AudioListener cameraHardAudioLis;

    private string connectionString;
    private List<HighScoreList> highScoreList = new List<HighScoreList>();

    private const float BOUNDS_SIZE = 3.5f;
    private const float STACK_MOVING_SPEED = 5.0f;
    private const float ERROR_MARGIN = 0.5f;
    private const float STACK_BOUNDS_GAIN = 0.25f;
    private const int COMBO_START_GAIN = 3;
    

    private GameObject[] theStack;
    private Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);
    private int stackIndex;
    private int scoreCount = 0;
    private int combo = 0;

    private float tileTranstition = 0.0f;
    private float tileSpeed = 2.5f;

    private bool isMovingOnX = true;
    private bool gameOver = false;

    private float secondaryPosition;
    private Vector3 desiredPosition;
    private Vector3 lastTilePosition;

    

    // Use this for initialization
    private void Start ()
    {
        //connect to db
        connectionString = "URI=file:" + Application.streamingAssetsPath + "/semogaBisa.db";
        Debug.Log("Ini connection stringnya :  " + connectionString);
        //InsertScore("dZar3", 30);
        GetScores();

        //getCameraListener
        cameraEasyAudioLis = cameraEasy.GetComponent<AudioListener>();
        //cameraHardAudioLis = cameraHard.GetComponent<AudioListener>();

        //int n = transform.childCount;
        theStack = new GameObject[transform.childCount];
        for (int i=0; i< transform.childCount; i++)
        {

            theStack[i] = transform.GetChild(i).gameObject;
            ColorMesh(theStack[i].GetComponent<MeshFilter>().mesh);
        }
        stackIndex = transform.childCount - 1;
        Debug.Log("masuk start   ");


    }

    private void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody> ();

        go.GetComponent<MeshRenderer>().material = stackMat;
        ColorMesh(go.GetComponent<MeshFilter>().mesh);


    }

    // Update is called once per frame
    private void Update ()
    {
        /*if (PlayerPrefs.GetFloat("BGMVolume") != null)
        {
        }*/
        myMusic.volume = PlayerPrefs.GetFloat("BGMVolume");
 
        if (gameOver)
        {
            /*SetScores*/
            //Debug.Log("Ini connection stringnya :  " + connectionString);

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                SoundManagerScript.PlaySound("dropSound");
                //print("stackIndex brp " + stackIndex);
                SpawnTitle();
                scoreCount++;

                scoreText.text = scoreCount.ToString();

                //scoreText.text = data;

            }
            else
            {
                EndGame();
            }
            
        }
        MoveTile();

        //move the stack
        transform.position = Vector3.Lerp(transform.position, desiredPosition, STACK_MOVING_SPEED * Time.deltaTime);
        
	}
    

    private void MoveTile()
    {
        
        tileTranstition += Time.deltaTime * tileSpeed + (0.005f * scoreCount);
        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTranstition) * BOUNDS_SIZE, scoreCount, secondaryPosition);
        }else
        {   
            theStack[stackIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTranstition) * BOUNDS_SIZE);
        }
    }

    private void SpawnTitle()
    {
        lastTilePosition = theStack[stackIndex].transform.localPosition;
        stackIndex--;
        if (stackIndex < 0)
        {
            stackIndex = transform.childCount - 2;
        }
        desiredPosition = (Vector3.down) * scoreCount;
        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
        theStack[stackIndex].transform.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        ColorMesh(theStack[stackIndex].GetComponent<MeshFilter>().mesh);
    }

    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;
        if (isMovingOnX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            if (Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                //cut the cube
                combo = 0;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                CreateRubble
                    (
                    new Vector3((t.position.x > 0) ? t.position.x + (t.localScale.x / 2)
                    :t.position.x - (t.localScale.x /2) ,
                    t.position.y,
                    t.position.z),
                    new Vector3(Mathf.Abs(deltaX),1, t.localScale.z)
                    );
                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
            }else
            {
                SoundManagerScript.PlaySound("Perfect");
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.x += STACK_BOUNDS_GAIN;
                    if (stackBounds.x > BOUNDS_SIZE)
                        stackBounds.x = BOUNDS_SIZE;
                    float middle = lastTilePosition.x + t.localPosition.x / 2;
                    t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                    t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);

                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }

        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                //cut the cube
                combo = 0; 
                stackBounds.y  -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                CreateRubble
                      (
                      new Vector3(
                      t.position.x,
                      t.position.y,
                      (t.position.z > 0) ? t.position.z + (t.localScale.z / 2)
                    : t.position.z - (t.localScale.z / 2)
                      ),
                      new Vector3(t.localScale.x, 1, Mathf.Abs(deltaZ))
                      );
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle- (lastTilePosition.z/2));
            }else
            {
                SoundManagerScript.PlaySound("Perfect");
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.y += STACK_BOUNDS_GAIN;
                    if (stackBounds.y > BOUNDS_SIZE)
                        stackBounds.y = BOUNDS_SIZE;
                    float middle = lastTilePosition.z + t.localPosition.z / 2;
                    t.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);
                    t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));

                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }

        secondaryPosition = (isMovingOnX)
            ? t.localPosition.x
            : t.localPosition.z;
        
        isMovingOnX = !isMovingOnX;
        return true;
    }

    private void ColorMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Color32[] colors = new Color32[vertices.Length];
        float f = Mathf.Sin(scoreCount * 0.25f);

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Lerp4(gameColors[0], gameColors[1], gameColors[2], gameColors[3], f);

        mesh.colors32 = colors;
    }

    private Color32 Lerp4(Color32 a, Color32 b, Color32 c, Color32 d, float t)
    {
        if (t < 0.33f)
            return Color.Lerp(a, b, t / 0.33f);
        else if (t < 0.66f)
            return Color.Lerp(b, c, (t - 0.33f) / 0.33f);
        else
            return Color.Lerp(c, d, (t - 0.66f) / 0.66f);
    }

    private void EndGame()
    {

       /* if (PlayerPrefs.GetInt("score") < scoreCount)
            PlayerPrefs.SetInt("score", scoreCount);*/
        gameOver = true;
        endPanel.SetActive(true);
        theStack[stackIndex].AddComponent<Rigidbody>();
        Debug.Log("playerpref getstring username apa di Endgame nih " + PlayerPrefs.GetString("username"));
        SetScores(PlayerPrefs.GetString("username"), scoreCount);
        Debug.Log("setelah setscore endgame nih ");

        //int data = (int)FindData();
        //print(data);
        //PlayerPrefs.SetInt("score", 20);


    }

    public void OnButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /*private void InsertScore(string name, int newScore)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("INSERT INTO scoreTbl(playerName, playerScore) VALUES(\"{0}\", \"{1}\")", name, newScore);

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
                
            }
        }
    }*/

    private void SetScores(string username, int scorecount)
    {

        /*using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "INSERT INTO highScore(USERNAME, SKOR) VALUES('"+username+"','1000')";

                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();

            }
        }*/

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(connectionString);
        Debug.Log("Ini connection stringnya :  " + connectionString);
        Debug.Log("Ini username sama scorecount  :  " + username + " ,," + scoreCount);

        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        //"INSERT OR REPLACE INTO highScore (USERNAME, SKOR) VALUES ('" + username + "','" + scorecount + "')"
        // 
        string sqlQuery = String.Format("INSERT INTO highScore(USERNAME, SKOR) VALUES(\"{0}\", \"{1}\")", username, scorecount);

        dbcmd.CommandText = sqlQuery;
        /*IDataReader reader = */dbcmd.ExecuteScalar();

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    private void GetScores()
    {
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(connectionString);
        Debug.Log("Ini connection stringnya :  " + connectionString);
        Debug.Log("getScore masuk  ");
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT * FROM highScore ORDER BY SKOR DESC LIMIT 3";

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            int value = reader.GetInt32(0);
            string name = reader.GetString(1);
            int rand = reader.GetInt32(2);

            Debug.Log("value= " + value + " name =" + name + " random =" + rand);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        /*
        WWW loadDB;
        if (File.Exists(connectionString))
        {
            loadDB = new WWW("jar:file://" + connectionString);
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(connectionString, loadDB.bytes);
        }
        string connectionEdited = "URI=file:" + connectionString;
        Debug.Log("URL  connectiom edited apa " + connectionEdited);
        highScoreList.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionEdited))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM scoreTbl ORDER BY playerScore DESC LIMIT 10";
                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log("id " + reader.GetInt32(0) + ",  nama  " + reader.GetString(1) + "  score  " + reader.GetInt32(2));
                        //highScoreList.Add(new HighScoreList(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                    }

                    dbConnection.Close();
                    reader.Close(); 
                }
            }
        }
        */
    }


    


}
