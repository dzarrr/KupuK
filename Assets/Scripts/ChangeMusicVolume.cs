using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMusicVolume : MonoBehaviour {

    public Slider volume;
    public AudioSource myMusic;
	// Use this for initialization
	// Update is called once per frame

	void Update () {
        myMusic.volume = volume.value;
        PlayerPrefs.SetFloat("BGMVolume", myMusic.volume);
	}
}
