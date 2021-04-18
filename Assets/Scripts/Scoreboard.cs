using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using System;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public  GameObject scoreText;
    GameObject content;
    int numberOfPlayers = 0;
    // Start is called before the first frame update
    void Start()
    {
        content = GameObject.Find ("Content");
        for (int i = 0; i < 100; ++ i)
        {
            GameObject g = Instantiate(scoreText, content.transform);
            g.GetComponent<Text>().text = "";
            g.transform.position -= new Vector3 (0.0f, 16.0f * (i), 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
        for (int i = 0; i < players.Length && i < content.transform.childCount; ++ i)
        {
            Player p = players[i].GetComponent<Player>();
            GameObject g = content.transform.GetChild (i).gameObject;
            g.GetComponent<Text>().text = p.nameNetworkVariable.Value + ": " + p.killsNetworkVariable.Value;
        }
    }
}
