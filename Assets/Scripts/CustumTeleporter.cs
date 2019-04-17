using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;

public class CustumTeleporter : MonoBehaviour {

    public VRTeleporter teleporter;
    public GameObject RightHandGO;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowTeleport()
    {
        teleporter.ToggleDisplay(true);
    }

    public void TeleportMe()
    {
        if (RightHandGO.activeInHierarchy)
        {
            teleporter.Teleport();
            teleporter.ToggleDisplay(false);
        }
        teleporter.ToggleDisplay(false);
    }
}
