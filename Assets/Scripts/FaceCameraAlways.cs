using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraAlways : MonoBehaviour
{

    public float delayBeforeActivate;
    private bool activate;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
            transform.LookAt(Camera.main.transform);
    }
    private void OnEnable()
    {
        StartCoroutine(Timer());
    }
    private void OnDisable()
    {
        activate = false;
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(delayBeforeActivate);
        activate = true;



    }
}
