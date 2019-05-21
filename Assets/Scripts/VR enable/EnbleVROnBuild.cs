using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnbleVROnBuild : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Awake()
    {
        //EditorUserBuildSettings.selectedBuildTargetGroup = BuildTargetGroup.Standalone;
        //EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget.StandaloneWindows64;
        //UnityEditorInternal.VR.VREditor.SetVREnabledOnTargetGroup(BuildTargetGroup.Standalone, true);
        //UnityEngine.XR.XRSettings.enabled = true;
        //PlayerSettings.virtualRealitySupported = true;
    }
}
