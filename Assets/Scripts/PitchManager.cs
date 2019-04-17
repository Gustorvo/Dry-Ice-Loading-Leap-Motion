using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity.Attributes;
using Leap.Unity;
using Leap;

public class PitchManager : MonoBehaviour  {

    Leap.Hand _hand;
    [SerializeField]
    protected HandModelBase _handModel;
    public HandModelBase HandModel { get { return _handModel; } set { _handModel = value; } }

    Controller controller = new Controller();
    Vector3 pinchPos;
    

    public GameObject leapHand;
    protected PinchDetector pinchScript;

    protected Vector3 pinch_position;
    protected float pinch_distance;
    protected Vector3 new_position;
    protected bool pinching;
    protected bool startPinch;
    protected bool endPinch;

   // Vector3 pinch_pos;
    // Use this for initialization
    void Start () {
        pinching = false;
       
        pinchScript = leapHand.GetComponent<PinchDetector>();

    }



	
	// Update is called once per frame
	void Update () {

        // Gets the state of your pinch
        startPinch = pinchScript.DidStartPinch;
        endPinch = pinchScript.DidEndPinch;
        pinching = pinchScript.IsPinching;

        // Does something once the state of the pinch changes
        //if (startPinch)
        //{                        
        //    Target.SetActive(true);
        //    Debug.Log(startPinch);
        //}
        //else if (endPinch)
        //{
        //    Target.SetActive(false);
        //    Target.transform.localScale = Vector3.one;
        //}

        if (pinching)
        {
            pinch_position = pinchScript.Position;
            //pinch_distance = pinchScript.Distance;
            //Target.transform.position = pinch_position;// + (Vector3.back * 0.01f);
           // Debug.Log(GetRelativeDistance());
            //Target.transform.localScale = GetRelativeDistance() * Vector3.one; 
        }

    }

    float GetRelativeDistance()
    {
        float minValue = pinchScript.ActivateDistance;
        float maxValue = pinchScript.DeactivateDistance;
        float actualValue = pinchScript.Distance;
        float persentage = actualValue / minValue;// 100;
        return persentage;

    }


    Hand ReturnHand()
    {
        Frame frame = controller.Frame(); // controller is a Controller object
        if (frame.Hands.Count > 0)
        {
            List<Hand> hands = frame.Hands;
            Hand firstHand = hands[0];
            return firstHand;
        }
        else return null;

    }

    public void ShowPosition()
    {
        Hand _hand = ReturnHand();
        Debug.Log(_hand.GrabStrength);
        //Vector3 pinchPos = _hand.GetPinchPosition();       
        
        
    }

    public void HidePos()
    {
        //Target.SetActive(false);
    }

    
}
