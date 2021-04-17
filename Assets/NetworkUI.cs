using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;

public class NetworkUI : MonoBehaviour
{
    // Start is called before the first frame update
    Button buttonHost;
    Button buttonJoin;
    InputField inputFieldIp;
    void Start()
    {
        buttonHost = GameObject.Find ("Host").GetComponent<Button>();
        buttonJoin = GameObject.Find ("Join").GetComponent<Button>();
        inputFieldIp = GameObject.Find ("InputFieldIP").GetComponent<InputField>();

        buttonHost.onClick.AddListener (Host);
        buttonJoin.onClick.AddListener (Join);
    }

    // Update is called once per frame
    void Host()
    {
        NetworkManager.Singleton.StartHost ();
        gameObject.SetActive(false);
    }

    void Join()
    {
        if (inputFieldIp.text.Length > 0)
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = inputFieldIp.text;
		}
        else
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
		}
        NetworkManager.Singleton.StartClient ();
        gameObject.SetActive(false);
    }
}
