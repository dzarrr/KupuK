using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TextInputManager : MonoBehaviour {

    public InputField username;
    public Text usernameText;

    public void setget()
    {
        string username = usernameText.text.ToString();
        PlayerPrefs.SetString("username", username);
        Debug.Log("Ini usernamenya ya : " + username);
        SceneManager.LoadScene("hang");
    }
}
