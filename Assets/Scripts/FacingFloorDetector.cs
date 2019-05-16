using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class FacingFloorDetector : Detector
{
    public LayerMask target;

    public GameObject StartPoint;
    private LineRenderer line;
    public bool debugMode;

    // Use this for initialization
    void Start()
    {

        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(StartPoint.transform.position, StartPoint.transform.forward, out hit, 20, target))
        {
            Activate();
            
            
            if (debugMode)
            {
                line.enabled = true;
                line.SetPosition(0, StartPoint.transform.position);
                line.SetPosition(1, hit.point);
            }
        }
        else
        {
            line.enabled = false;
            Deactivate();
        }

    }

    public void TestDebugerActiveate()
    {
        Debug.Log("Activated");
    }

    public void TestDebugerDeactivate()
    {
        Debug.Log("Deactivate");
    }
}
