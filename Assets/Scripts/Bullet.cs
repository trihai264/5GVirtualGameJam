using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

public class Bullet : NetworkBehaviour
{
    public GameObject HitEffect;
    public NetworkVariableInt killCount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsServer || IsHost)
        {
            GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);
            effect.GetComponent<NetworkObject>().Spawn();
            Destroy(effect, 0.25f);
            Destroy(gameObject);

            if (collision.gameObject.tag == "Crab" && killCount != null)
            {
                ++ killCount.Value;
			}

            if (collision.gameObject.tag == "Player")
			{
				if (collision.gameObject.GetComponent<Player>().killsNetworkVariable != null)
				{
					collision.gameObject.GetComponent<Player>().killsNetworkVariable.Value /= 2;
                    collision.gameObject.GetComponent<Player>().deadNetworkVariable.Value = true;
				}

                if (killCount != null)
                {
                    ++ killCount.Value;
				}
			}
        }
    }
}
