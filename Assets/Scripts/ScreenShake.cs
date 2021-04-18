using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Camera cam;
    public float shake;
    public float shakeAmount;
    public float decreaseFactor;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (shake > 0)
        {
            Debug.Log("Shake that cam for me");
            cam.transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0;
        }


    }
}