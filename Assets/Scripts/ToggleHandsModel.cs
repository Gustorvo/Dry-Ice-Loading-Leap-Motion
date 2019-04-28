using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class ToggleHandsModel : MonoBehaviour {

    public HandModelManager HandPool;
    public string[] HandGroups;
    [SerializeField]
    private int current;
    public DetectorManager _detectorManagerScript;
   
    


    void Start () {
        HandPool = GetComponent<HandModelManager>(); 
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        ToggleHands();

    }

    public void ToggleHands()
    {
        HandPool.DisableGroup(HandGroups[current]);
        if (current == HandGroups.Length - 1) // reached the last element
            current = 0; // start with the first element
        else current++;
        HandPool.EnableGroup(HandGroups[current]);

        _detectorManagerScript.ToggleDetectors(); // toggle detectors for new hands


    }

    

    
}
