using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPrefab : NetworkBehaviour {

    public GameObject playerObjectSelf;
    public GameObject playerObjectOthers;

    private GameObject spawnedObjectForMySelf;

    private int range = 200;
    [SerializeField]
    private Transform camTransform;
    [SyncVar]
    private GameObject objectID;
    [SyncVar]
    private Transform leftHand;
    [SyncVar]
    private Transform rightHand;
    private NetworkIdentity objNetId;


    [SyncVar]
    public Vector3 scale;
    [SyncVar]
    public Quaternion rotation;
    [SyncVar]
    public Vector3 pos;
    [SyncVar]
    public string objName;

    // Use this for initialization
    void Start ()
    {

        if (isLocalPlayer)
        {
           // transform.localScale = scale;
           // transform.rotation = rotation;
           // transform.position = pos;
            GameObject ownPlayer = Instantiate(playerObjectSelf);
            ownPlayer.transform.parent = this.gameObject.transform;
            ownPlayer.transform.position = this.gameObject.transform.position;
            CmdSpawnOnNetwork(ownPlayer);// NetworkServer.Spawn(ownPlayer);
            spawnedObjectForMySelf = ownPlayer;
            leftHand = ownPlayer.GetComponentInChildren<VRPawn>().LeftController;
           // rightHand = ownPlayer.GetComponentInChildren<VRPawn>().RightController;
        }
        else
        {
            GameObject otherPlayer = Instantiate(playerObjectOthers);
            otherPlayer.transform.parent = this.gameObject.transform;
            otherPlayer.transform.position = this.gameObject.transform.position;
            //NetworkServer.Spawn(otherPlayer);
            CmdSpawnOnNetwork(otherPlayer);// NetworkServer.Spawn(otherPlayer);
            spawnedObjectForMySelf = otherPlayer;
        }
	}
    [Command]
    void CmdSpawnOnNetwork(GameObject gO)
    {

        NetworkPrefab networkPref = gO.GetComponent<NetworkPrefab>();
        networkPref.leftHand = gO.GetComponentInChildren<VRPawn>().LeftController;
        //networkPref.rotation = gO.transform.rotation;
      //  networkPref.scale = gO.transform.localScale;
      //  networkPref.pos = gO.transform.position;
        NetworkServer.Spawn(gO);
    }

    void Update()
    {
        // only do something if it is the local player doing it
        // so if player 1 does something, it will only be done on player 1's computer
        // but the networking scripts will make sure everyone else sees it
        if (isLocalPlayer)
        {
            CheckIfMoving();
        }
    }

    void CheckIfMoving()
    {
        // yes, isLocalPlayer is redundant here, because that is already checked before this function is called
        // if it's the local player and their mouse is down, then they are "painting"
        if (isLocalPlayer && Input.GetKey("up"))// && Input.GetMouseButtonDown(0))
        {
            objectID = this.gameObject;// spawnedObjectForMySelf;// GameObject.Find(hit.transform.name);
            CmdColor(objectID);
            // here is the actual "painting" code
            // "paint" if the Raycast hits something in it's range
           /* if (Physics.Raycast(camTransform.TransformPoint(0, 0, 0.5f), camTransform.forward, out hit, range))
            {
                objectID = GameObject.Find(hit.transform.name);                                    // this gets the object that is hit
                objectColor = new Color(Random.value, Random.value, Random.value, Random.value);    // I select the color here before doing anything else
                CmdPaint(objectID, objectColor);    // carry out the "painting" command
            }*/
        }
        if (isLocalPlayer)
        {
            objectID = this.gameObject;
            //CmdMove(objectID);//, this.gameObject.GetComponentInChildren<VRPawn>().gameObject);
            //UnityEngine.Debug.Log("position.." + Position);
        }
    }

    [ClientRpc]
    void RpcColor(GameObject obj)//, Color col)
    {
        //obj.transform.position = 
        //UnityEngine.Debug.Log(obj.transform.GetComponentInChildren<Transform>().name);
        obj.GetComponentInChildren<Renderer>().material.color = Color.red;      // this is the line that actually makes the change in color happen
    }

    [Command]
    void CmdColor(GameObject obj)//, Color col)
    {
        objNetId = obj.GetComponent<NetworkIdentity>();        // get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);    // assign authority to the player who is changing the color
        RpcColor(obj);//, col);                                    // usse a Client RPC function to "paint" the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);    // remove the authority from the player who changed the color
    }


    [SyncVar]
    public Vector3 Position;
    [SyncVar]
    public Quaternion Rotation;

    [ServerCallback]
    public void FixedUpdate()
    {
        if (!isServer) //Only server will acess at this point to down;
            return;
        if (isLocalPlayer)
        {
            Position = transform.GetComponentInChildren<VRPawn>().LeftController.position;//transform.position; // this will sync Position across all clients at runtime your gameObject that has this script attach;
            Rotation = transform.GetComponentInChildren<VRPawn>().LeftController.rotation; // this will sync Rotation across all clients at runtime your gameObject that has this script attach;
        }
        // Here you put everthing that your non-playerObject want to do atack, life , stats ...

        // Here is just a sample not automatic for Server Pass Info to player.
        /*
        * if(Condition)
        {
            NetworkIdentity Player = (NetworkIdentity)GameObject.Find ("Player").GetComponent <NetworkIdentity> ();
            GiveDamage (Damage,Player);
        }
        */
    }


    [ClientRpc]
    void RpcMove(GameObject obj)
    {
        //not set --> exception
        //UnityEngine.Debug.Log("lefthandposition" + obj.GetComponentInChildren<VRPawn>().LeftController.position.ToString());
       // UnityEngine.Debug.Log(holdsTransform.name);
       // UnityEngine.Debug.Log(holdsTransform.GetComponent<VRPawn>().name);
       // UnityEngine.Debug.Log(holdsTransform.GetComponent<VRPawn>().LeftController.position);
        obj.GetComponentInChildren<VRPawn>().LeftController = obj.GetComponent<VRPawn>().LeftController;// leftHand.position;
        //obj.transform.position = 
        //UnityEngine.Debug.Log(obj.transform.GetComponentInChildren<Transform>().name);
        //obj.transform.position = obj.transform.position;
        //VRPawn script = obj.GetComponentInChildren<VRPawn>();
        //obj.transform.position = script.LeftController.position;
        //obj.GetComponentInChildren<Renderer>().material.color = Color.red;      // this is the line that actually makes the change in color happen
    }

    [Command]
    void CmdMove(GameObject obj)//, GameObject holdsTransform)
    {
        objNetId = obj.GetComponent<NetworkIdentity>();        // get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);    // assign authority to the player who is changing the color
        RpcMove(obj);                                           // usse a Client RPC function to "paint" the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);    // remove the authority from the player who changed the color
    }

}
