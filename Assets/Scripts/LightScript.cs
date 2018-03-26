using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    public float duration = 1.0F;
    public Light lt,lt2;

	// Use this for initialization
	void Start () {
        lt = GetComponent<Light>();
        lt2 = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        
        float phi =  Time.time / duration * 2 * Mathf.PI;
        float amplitude = 50 * Mathf.Sin(phi);// * 0.5F + 0.5F;
        float amplitude2 = 50 * Mathf.Sin(-phi);
        lt.intensity = amplitude;
        lt2.intensity = amplitude2;

    }
}
