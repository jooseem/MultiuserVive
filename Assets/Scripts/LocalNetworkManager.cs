using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LocalNetworkManager : MonoBehaviour {

    public static bool isRunningAsServer = true;
	public bool runAsServer = true;

    private CustomNetworkManager netManager;
    private CustomNetworkDiscovery netDiscovery;

	void Awake(){
        isRunningAsServer = runAsServer;
	}

    
	void Start () {

		netManager = GetComponent<CustomNetworkManager>();
        netDiscovery = GetComponent<CustomNetworkDiscovery>();
        netDiscovery.Initialize();

		if (isRunningAsServer) {
			SetupServer();
		} else {
			SetupClient();
		}
	}

    void netDiscovery_OnReceivedNetworkBroadcast(string fromAddress, string data)
    {
        Debug.Log("Received fromAddress:" + fromAddress);
        Debug.Log("Received data:" + data);

        netManager.networkAddress = fromAddress;
        netManager.StartClient();
        netDiscovery.StopBroadcast();
    }


	void SetupServer(){
        
        netManager.networkAddress = "localhost";
		netManager.StartHost();

        netDiscovery.StartAsServer();

	}

	void SetupClient(){

        netDiscovery.OnReceivedNetworkBroadcast += netDiscovery_OnReceivedNetworkBroadcast;
        netDiscovery.StartAsClient();

        netManager.ClientConnected += netManager_ClientConnected;
        netManager.ClientDisconnected += netManager_ClientDisconnected;
        netManager.ClientError += netManager_ClientError;
	}

    void netManager_ClientError()
    {
        Debug.Log("ClientError");
    }

    void netManager_ClientDisconnected()
    {
        Debug.Log("ClientDisconnected");

		netManager.StopClient();
		netManager.enabled = false;
		netDiscovery.enabled = false;
        Invoke("ProcessDisconnect", 0.5f);

    }

    void netManager_ClientConnected()
    {
        Debug.Log("ClientConnected");
    }

    void ProcessDisconnect()
    {
		netManager.enabled = true;
		netDiscovery.enabled = true;

        netDiscovery.Initialize();
        netDiscovery.StartAsClient();
    }

}
