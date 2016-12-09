using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using Leap.Unity;

public class VRPawn : NetworkBehaviour {

    public Transform Head;


    void Start () {
        if (isLocalPlayer) { 
            //GetComponentInChildren<SteamVR_ControllerManager>().enabled = true;
            GetComponentsInChildren<SteamVR_TrackedObject>(true).ToList().ForEach(x => x.enabled = true);
            /*GameObject cameraRig = GameObject.FindGameObjectWithTag("cameraRig");
            GameObject head = GetComponentInChildren<SteamVR_TrackedObject>().gameObject;
            cameraRig.transform.parent = head.transform;*/
            GetComponentInChildren<HandPool>().enabled = true;
            GetComponentInChildren<LeapHandController>().enabled = true;
            GetComponentInChildren<LeapServiceProvider>().enabled = true;
            Head.GetComponentsInChildren<MeshRenderer>(true).ToList().ForEach(x => x.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly);
            Invoke("Activate", 5f);

        }
        else
        {
            GetComponentsInChildren<HandTransitionBehavior>(true).ToList().ForEach(x => { x.gameObject.SetActive(true); Destroy(x); });
            GetComponentsInChildren<IHandModel>(true).ToList().ForEach(x => Destroy(x));
            GetComponentsInChildren<RiggedFinger>(true).ToList().ForEach(x => Destroy(x));
            GetComponentsInChildren<RiggedHand>(true).ToList().ForEach(x => Destroy(x));
            Destroy(GetComponentInChildren<LeapHandController>().gameObject);
        }
    }
    void Activate()
    {
        LeapVRTemporalWarping leapSpace = GameObject.FindGameObjectWithTag("leapSpace").GetComponentInChildren<LeapVRTemporalWarping>(true);
        leapSpace.enabled = true;
        leapSpace.setProvider(GetComponentInChildren<LeapServiceProvider>());
        LeapServiceProvider lspr = GetComponentInChildren<LeapServiceProvider>();
        lspr.setTempralWarping(leapSpace);
    }

    void OnDestroy()
    {
        //GetComponentInChildren<SteamVR_ControllerManager>().enabled = false;
        GetComponentsInChildren<SteamVR_TrackedObject>(true).ToList().ForEach(x => x.enabled = false);
    }
}
