using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float dampTime = 10f;
    public Vector3 cameraPos = Vector3.zero;
    public GameObject target;
    public float xOffset = 0;
    public float yOffset = 0;
    public float otherMargin;
    public float camSpeed;
    public float maxCamSpeed;
    public float camAcc;
    public float margin = 0.1f;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (target != null)
        {
            cameraPos = new Vector3(Mathf.SmoothStep(transform.position.x, target.transform.position.x, dampTime), Mathf.SmoothStep(transform.position.y, target.transform.position.y, dampTime));
        }



    }
    void LateUpdate()
    {
       
            transform.position = cameraPos + Vector3.forward * -10;
        

    }
}
