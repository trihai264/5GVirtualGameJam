using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class EnemyBehaviourScript : NetworkBehaviour
{
	public float _moveSpeed = 1.0f;
	public bool _gameOver = false;

	public GameObject _explosionPrefab;
	private Transform _target;
	private Rigidbody2D _rb;
	private SpriteRenderer _sr;
	private BoxCollider2D _bc2d;
	bool hasTarget = false;
	float newTargetTimer = 1.0f;
	// Start is called before the first frame update
	void Start()
	{
		if (IsHost)
		{ 
			_rb = GetComponent<Rigidbody2D>();
			_bc2d = GetComponent<BoxCollider2D>();
			_sr = GetComponent<SpriteRenderer>();
		}

	}


	void Update()
	{
		if (IsHost)
		{
			if (hasTarget == false || newTargetTimer <= 0.0f)
			{ 
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

				int closest = 0;
				float dist = float.MaxValue;
				for (int i = 0; i < players.Length; ++ i) 
				{
					float d = Vector2.Distance (transform.position, players[i].transform.position);

					if (d <= dist)
					{ 
						closest = i;
						dist = d;
					}
				}

				if (players.Length > 0)
				{
					_target = players[closest].transform;
					hasTarget = true;
					newTargetTimer = 1.0f;
				}
			}


			if (hasTarget)
			{ 
				transform.position = Vector2.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);
			}

			newTargetTimer -= Time.deltaTime;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsHost)
		{ 
			if (collision.gameObject.tag == "Bullet")
			{
				_moveSpeed = 0.0f;
				_bc2d.enabled = false;
				_sr.enabled = false;
				Destroy(Instantiate(_explosionPrefab, transform.position, Quaternion.identity), 0.25f);
				Destroy(gameObject, 0.25f);
			}
		}
	}
}
