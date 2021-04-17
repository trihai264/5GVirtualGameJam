using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;
	void Start()
	{
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
    {
        if (IsLocalPlayer)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void FixedUpdate()
    {
        if (IsLocalPlayer)
        { 
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle; 
        }
    }
}
