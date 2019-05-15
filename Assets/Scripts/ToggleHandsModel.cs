using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

using Leap.Unity.Interaction;
using System;

public class ToggleHandsModel : MonoBehaviour {

    private InteractionBehaviour intGloves;
    public HandModelManager HandPool;
    public string[] HandGroups;
    [SerializeField]
    private int current;
    public DetectorManager _detectorManagerScript;
    public Material[] _material;
    Renderer[] _ren;
   




    void Start () {
        
         _ren = GetComponentsInChildren<Renderer>();
        intGloves = gameObject.GetComponent<InteractionBehaviour>();
         intGloves.OnHoverBegin += ToggleHands;

       // intGloves.OnHoverBegin(ToggleHands);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        ToggleHands();
        

    }

    public Action<bool> OnGlovesOn = delegate { };
    

    public void ToggleHands()
    {
        //Debug.Log("shifting hands");
        HandPool.DisableGroup(HandGroups[current]);
        if (current == HandGroups.Length - 1) // reached the last element
        {
            current = 0; // start with the first element
            OnGlovesOn(false);
        }
        else
        {
            current++;
            OnGlovesOn(true);
        }
        HandPool.EnableGroup(HandGroups[current]);

        
        foreach (var item in _ren)
        {
            item.material = _material[current];
            
        }

        _detectorManagerScript.ToggleDetectors(); // toggle detectors for new hands   
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("gloves"))
    //    {
    //        ToggleHands();
            
    //    }
    //}

    IEnumerator DisableCollider()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Collider>().enabled = true;
    }



}
