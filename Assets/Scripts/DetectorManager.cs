using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class DetectorManager : MonoBehaviour
{
    public HandModelBase[] CapsulHands;
    public HandModelBase[] RiggedHands;

    public GameObject PalmDetector;
    public GameObject PinchDetector;

    private int current = 1; // first element

    // Use this for initialization


   public void ToggleDetectors()
    {
        if (current == 2) // reached the last lement, toggle back
        {
            DoJob(CapsulHands[1]);// first element [0] of Hands Array is a left hand, [1] second - right hand
            current = 1;
        }
        else
        {
            DoJob(RiggedHands[1]);
            current = 2;
        }
    }

    void DoJob(HandModelBase model)
    {
        PalmDetector.GetComponent<PalmDirectionDetector>().HandModel = model;
        PinchDetector.GetComponent<PinchDetector>().HandModel = model;
    }
}
