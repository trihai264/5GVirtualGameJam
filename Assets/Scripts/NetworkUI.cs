using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
using MLAPI.Connection;

public class NetworkUI : MonoBehaviour
{
    static string ErrorNameTooShort = "Name needs to be more than 3 characters long";
    // Start is called before the first frame update
    Button buttonHost;
    Button buttonJoin;
    InputField inputFieldIp;
    InputField inputFieldName;
    Text errorText;
    float t = 0.0f;
    void Start()
    {
        buttonHost = GameObject.Find ("Host").GetComponent<Button>();
        buttonJoin = GameObject.Find ("Join").GetComponent<Button>();
        inputFieldIp = GameObject.Find ("InputFieldIP").GetComponent<InputField>();
        inputFieldName = GameObject.Find ("InputFieldName").GetComponent<InputField>();
        inputFieldName.text = PlayerPrefs.GetString ("name");
        errorText = GameObject.Find ("Error").GetComponent<Text>();
        errorText.text = "";
        buttonHost.onClick.AddListener (Host);
        buttonJoin.onClick.AddListener (Join);

        NetworkManager.Singleton.OnServerStarted += ServerStarted;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }

	private void OnDestroy()
	{
		NetworkManager.Singleton.OnServerStarted -= ServerStarted;
        NetworkManager.Singleton.OnClientDisconnectCallback -= ClientDisconnected;
        NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnected;
	}

	private void Update()
	{
		if (gameObject.activeInHierarchy == true && (NetworkManager.Singleton.IsClient))
        {
            if (Time.time - t >= 5.0f)
            {
                errorText.text = "Failed to Join";
                NetworkManager.Singleton.StopClient ();
			}
		}
	}

	// Update is called once per frame
	void Host()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient ();
		}

        else if (NetworkManager.Singleton.IsHost)
        {
            
            NetworkManager.Singleton.StopHost ();
		}

        if (inputFieldName.text.Length < 3)
        {
            errorText.text = ErrorNameTooShort;
		}

        else
        { 
            PlayerPrefs.SetString ("name", inputFieldName.text);
            PlayerPrefs.Save ();
            errorText.text = "";
            NetworkManager.Singleton.StartHost ();
            gameObject.SetActive(false);
        }
    }

    void Join()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient ();
		}

        else if (NetworkManager.Singleton.IsHost)
        {
            
            NetworkManager.Singleton.StopHost ();
		}

        if (inputFieldName.text.Length < 3)
        {
            errorText.text = ErrorNameTooShort;
		}

        else
        {
            PlayerPrefs.SetString ("name", inputFieldName.text);
            PlayerPrefs.Save ();
            errorText.text = "Connecting...";
            if (inputFieldIp.text.Length > 0)
            {
                NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = inputFieldIp.text;
		    }
            else
            {
                NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = "127.0.0.1";
		    }

            NetworkManager.Singleton.StartClient ();
            t = Time.time;
        }
    }

    void ServerStarted ()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(false);
		}
	}
    void ClientConnected (ulong v)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(false);
		}
	}

    void ClientDisconnected (ulong v)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(true);
            errorText.text = "Disconnected from the Host";
		}
	}
}
