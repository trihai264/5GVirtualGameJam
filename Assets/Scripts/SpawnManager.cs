using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class SpawnManager : NetworkBehaviour
{
    public bool _stopSpawning = false;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerUpContainer;
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private GameObject _powerUpPrefab;
    private EnemyBehaviourScript enemyBehaviourScript;


    private void Start()
    {
        if (IsHost)
        { 
            StartCoroutine(EnemySpawnRoutine());
            StartCoroutine(PowerUpsSpawnRoutine());
        }
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (!_stopSpawning)
        {
            if (IsHost)
            { 
                // Creates Random Position For Enemy To Spawn
                Vector2 posToSpawn = new Vector2(Random.Range(-7.5f, 7.5f), Random.Range(-4.5f, 4.5f));
                // Select which enemy prefab to spawn
                int _enemyID = Random.Range(0, 5);
                GameObject newEnemy = Instantiate(_enemyPrefab[_enemyID], posToSpawn, Quaternion.identity);
                newEnemy.GetComponent<NetworkObject>().Spawn ();
                // Assign Enemy Container as parent
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    IEnumerator PowerUpsSpawnRoutine()
    {
        if (IsHost)
        {
            yield return new WaitForSeconds(5.0f);

            while (!_stopSpawning)
            {
                // Creates Random Position For Enemy To Spawn
                Vector2 posToSpawn = new Vector2(Random.Range(-7.5f, 7.5f), Random.Range(-4.5f, 4.5f));
                // Select which power up prefab to spawn
                //int powerUpID = Random.Range(0, 5);
                GameObject newPowerUp = Instantiate(_powerUpPrefab, posToSpawn, Quaternion.identity);
                newPowerUp.GetComponent<NetworkObject>().Spawn ();
                // Assign PowerUpContainer Container as parent
                newPowerUp.transform.parent = _powerUpContainer.transform;
                yield return new WaitForSeconds(5.0f);
                Destroy(newPowerUp.gameObject);
                newPowerUp.GetComponent<NetworkObject>().Despawn ();
            }
        }
    }

    public void OnPlayerDeatth()
    {
        if (IsHost)
        {
            _stopSpawning = true;
        }
    }
}
