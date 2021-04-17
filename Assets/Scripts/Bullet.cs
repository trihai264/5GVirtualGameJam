using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Bullet : NetworkBehaviour
{
    public GameObject HitEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsServer || IsHost)
        {
            GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);
            effect.GetComponent<NetworkObject>().Spawn();
            Destroy(effect, 0.25f);
            Destroy(gameObject);
        }
    }
}
