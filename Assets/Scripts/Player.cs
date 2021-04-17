using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class Player : NetworkBehaviour
{
	public float moveSpeed = 5f;
	NetworkVariableString nameNetworkVariable = new NetworkVariableString (new NetworkVariableSettings { ReadPermission = NetworkVariablePermission.Everyone, WritePermission = NetworkVariablePermission.OwnerOnly });
	NetworkVariableInt killsNetworkVariable = new NetworkVariableInt (new NetworkVariableSettings { ReadPermission = NetworkVariablePermission.Everyone, WritePermission = NetworkVariablePermission.ServerOnly });

	public Rigidbody2D rb;

	public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

	Rect rect = new Rect(0, 0, 300, 100);
	Vector3 offset = new Vector3(-0.2f, -1.0f, 0.5f);

	Camera cam;
	Vector2 movement;
	Vector2 mousePos;
	void Start()
	{	
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		if (IsLocalPlayer)
		{
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraFollow>().target = gameObject;
			nameNetworkVariable.Value = PlayerPrefs.GetString ("name");
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (IsLocalPlayer)
		{
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
			mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

			if (Input.GetButtonDown("Fire1"))
			{ 
				ShootServerRpc();
			}
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

	void OnGUI()
    {
        Vector3 point = cam.WorldToScreenPoint(transform.position + offset);
        rect.x = point.x;
        rect.y = Screen.height - point.y - rect.height;
		GUI.color = Color.red;
        GUI.Label(rect, nameNetworkVariable.Value);
    }

	[ServerRpc]
    void ShootServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        bullet.GetComponent<NetworkObject>().Spawn();
        Destroy(bullet, 5f);
    }
}
