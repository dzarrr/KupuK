using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip dropSound, perfectSound,fallSound;
    static AudioSource audioSrc;

	// Use this for initialization
	void Start () {
        dropSound = Resources.Load<AudioClip>("dropSound");
        perfectSound = Resources.Load<AudioClip>("Perfect");
        audioSrc = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound (string clip)
    {
        if (clip == "dropSound")
        {
            audioSrc.PlayOneShot(dropSound);
        }
        if (clip == "Perfect")
        {
            audioSrc.PlayOneShot(perfectSound);
        }
       
    }
}
