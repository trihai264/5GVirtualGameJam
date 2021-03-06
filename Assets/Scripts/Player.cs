using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class Player : NetworkBehaviour
{
	public float moveSpeed = 5f;
	public NetworkVariableString nameNetworkVariable = new NetworkVariableString (new NetworkVariableSettings { ReadPermission = NetworkVariablePermission.Everyone, WritePermission = NetworkVariablePermission.OwnerOnly });
	public NetworkVariableInt killsNetworkVariable = new NetworkVariableInt (new NetworkVariableSettings { ReadPermission = NetworkVariablePermission.Everyone, WritePermission = NetworkVariablePermission.ServerOnly });
	public NetworkVariableBool deadNetworkVariable = new NetworkVariableBool (new NetworkVariableSettings { ReadPermission = NetworkVariablePermission.Everyone, WritePermission = NetworkVariablePermission.ServerOnly });

	public Rigidbody2D rb;

	public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

	Rect rect = new Rect(0, 0, 300, 100);
	Vector3 offset = new Vector3(-0.2f, -1.0f, 0.5f);

	float deadTimer = 0.0f;
	Camera cam;
	Vector2 movement;
	Vector2 mousePos;
	void Start()
	{	
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		if (IsLocalPlayer)
		{
			GameObject.FindGameObjectWithTag ("CameraParent").GetComponent<CameraFollow>().target = gameObject;
			nameNetworkVariable.Value = PlayerPrefs.GetString ("name");
		}

		if (IsServer)
		{
			killsNetworkVariable.Value = 0;
		}

		deadNetworkVariable.OnValueChanged += DeadChanged;
	}

	void DeadChanged (bool old, bool now)
	{
		if (now)
		{
			deadTimer = 5.0f;
			GetComponent<Renderer>().material.color = Color.red;
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (IsLocalPlayer && !deadNetworkVariable.Value)
		{
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
			mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

			if (Input.GetButtonDown("Fire1"))
			{ 
				ShootServerRpc();
			}
		}

		if (deadNetworkVariable.Value)
		{ 
			if (deadTimer <= 0.0f)
			{
				deadNetworkVariable.Value = false;
				GetComponent<Renderer>().material.color = Color.white;
				GetComponent<BoxCollider2D>().enabled = true;
			}

			deadTimer -= Time.deltaTime;
		}
	}

	void FixedUpdate()
	{
		if (IsLocalPlayer && !deadNetworkVariable.Value)
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
        GUI.Label(rect, nameNetworkVariable.Value + ": " + killsNetworkVariable.Value);
    }

	
	[ServerRpc]
    void ShootServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bullet.GetComponent<Bullet>().killCount = killsNetworkVariable;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        bullet.GetComponent<NetworkObject>().Spawn();
        Destroy(bullet, 5f);
		GetComponent<AudioSource>().Play();
		ShootSoundClientRpc ();
    }


	[ClientRpc]
	void ShootSoundClientRpc()
	{
		GetComponent<AudioSource>().Play();
	}
}
