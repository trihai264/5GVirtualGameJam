using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourScript : MonoBehaviour
{
    public float _moveSpeed = 1.0f;
    public bool _gameOver = false;

    public GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _player;
    private Transform _playerPos;
    //private SpawnManager _spawnManagerScript;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private BoxCollider2D _bc2d;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc2d = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //_spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

    }


    void Update()
    {
        if (!_gameOver)
        {
            transform.position = Vector2.MoveTowards(transform.position, _playerPos.position, _moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _moveSpeed = 0.0f;
            _bc2d.enabled = false;
            _sr.enabled = false;
            Destroy(Instantiate(_explosionPrefab, transform.position, Quaternion.identity), 1.0f);
            Destroy(gameObject, 1.0f);
        }
    }
}
