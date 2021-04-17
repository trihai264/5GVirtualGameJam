using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrderSorter : MonoBehaviour
{
    [SerializeField]
    int sortingOrderBase = 5000;
    public float offset = 0.0f;
    SpriteRenderer renderer;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        renderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
    }
}
