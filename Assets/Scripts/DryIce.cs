using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DryIce : MonoBehaviour, IDryIce {

    public void BePlaced()
    {
       // throw new System.NotImplementedException();
    }

    // Use this for initialization
    IEnumerator Start () {
        yield return new WaitForSeconds(1);
         



    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
