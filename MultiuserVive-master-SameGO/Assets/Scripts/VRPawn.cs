using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using Leap.Unity;

public class VRPawn : NetworkBehaviour {

    public Transform Head;
    public Transform LeftController;
    public Transform RightController;
    public RiggedHand left, right;


    void Start () {
        if (isLocalPlayer) { 
            //GetComponentInChildren<SteamVR_ControllerManager>().enabled = true;
            GetComponentsInChildren<SteamVR_TrackedObject>(true).ToList().ForEach(x => x.enabled = true);
            
           // Head.GetComponentsInChildren<MeshRenderer>(true).ToList().ForEach(x => x.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly);

        }else
        {
            GetComponentsInChildren<HandTransitionBehavior>(true).ToList().ForEach(x => x.enabled = false);
            GetComponentsInChildren<IHandModel>(true).ToList().ForEach(x => x.enabled = false);
            GetComponentsInChildren<RiggedFinger>(true).ToList().ForEach(x => x.enabled = false);
            GetComponentsInChildren<RiggedHand>(true).ToList().ForEach(x => x.enabled = false);
        }
    }

    void OnServerAddPlayer()
    {
        UnityEngine.Debug.Log("in on server add player");
    }

    void OnDestroy()
    {
        //GetComponentInChildren<SteamVR_ControllerManager>().enabled = false;
        GetComponentsInChildren<SteamVR_TrackedObject>(true).ToList().ForEach(x => x.enabled = false);
    }
}
