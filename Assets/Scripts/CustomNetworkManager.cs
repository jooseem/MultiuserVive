using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public event System.Action ClientConnected = delegate { };
    public event System.Action ClientDisconnected = delegate { };
    public event System.Action ClientError = delegate { };

    override public void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ClientConnected();
    }

    override public void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ClientDisconnected();
    }

    override public void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        ClientError();
    }
}
