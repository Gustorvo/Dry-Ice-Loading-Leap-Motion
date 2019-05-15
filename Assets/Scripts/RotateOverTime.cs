using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour {
    public float rotaionSpeed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.Rotate(Vector3.up * (rotaionSpeed * Time.deltaTime));
	}
}

