using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMeTimer : MonoBehaviour {

    float time = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        StartCoroutine(DeactivationTimer(time));
    }

    IEnumerator DeactivationTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }
}
