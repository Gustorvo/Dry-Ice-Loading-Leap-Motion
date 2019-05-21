using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorsOpenClose))]
public class DoorsSoundEvent : MonoBehaviour {

    [FMODUnity.EventRef]
    public string OnDoorsOpenSound;

    [FMODUnity.EventRef]
    public string OnDoorsCloseSound;

    private DoorsOpenClose doors;
    private string EventSound;

    // Use this for initialization
    void Start () {
        doors = GetComponent<DoorsOpenClose>();
        doors.OnDoorOpen += PlaySound;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(GameObject doorGo, bool doorStatus)
    {        
        if (doorStatus) EventSound = OnDoorsOpenSound;
        else if (!doorStatus) EventSound = OnDoorsCloseSound;
        else Debug.Log("");

        FMODUnity.RuntimeManager.PlayOneShot(EventSound, transform.position);

    }
}
