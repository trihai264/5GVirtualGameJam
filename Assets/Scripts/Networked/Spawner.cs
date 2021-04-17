using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class Spawner : NetworkBehaviour
{
    public GameObject[] objects;
    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHost)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = 1.5f;
                GameObject g = Instantiate(objects [Random.Range (0, objects.Length - 1)]);
                g.GetComponent<NetworkObject>().Spawn ();
			}
		}
    }
}
