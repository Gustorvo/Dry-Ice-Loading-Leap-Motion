using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;


public class MagicCircleManager : MonoBehaviour
{

    public AnimationClip _animation;
    private Animation _myAnim;
    public float speed = 1;
    public PinchDetector pinchScript;
    public GameObject Target;
    protected bool pinching;
    protected bool startPinch;
    protected bool endPinch;
    protected bool objectHidden;

    Leap.Hand _hand;
    Controller controller = new Controller();
    private bool isGrabbing;

    private void Awake()
    {
        if (Target == null)
            Target = gameObject;

        _myAnim = GetComponent<Animation>();
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

    IEnumerator isGrabbingSmth()
    {
        while (pinching)
        {
            float probability = ReturnHand().GrabStrength;
            if (probability > 0.5f)
                isGrabbing = true;
            yield return null;
        }
        isGrabbing = false;
    }


    public void CheckGrabState()
    {
        Hand _hand = ReturnHand();
        Debug.Log(_hand.GrabStrength);
    }

    public void ShowMagicCircle()
    {
        if (!isGrabbing)
        {
            objectHidden = false;
            _myAnim.AddClip(_animation, "Show");
            _myAnim["Show"].speed = speed * -1;
            _myAnim["Show"].time = _myAnim["Show"].length;
            _myAnim.Play("Show");
        }

    }

    public void HideMagicCircle()
    {
        if (!objectHidden)
        {
            objectHidden = true;
            _myAnim.AddClip(_animation, "Hide");
            _myAnim["Hide"].speed = speed * 1;
            _myAnim["Hide"].time = 0; // _myAnim["Hide"].length;
            _myAnim.Play("Hide");
        }

    }



    void Update()
    {
        pinching = pinchScript.IsPinching;
        startPinch = pinchScript.DidStartPinch;
        endPinch = pinchScript.DidEndPinch;

        if (pinching)
        {
            Target.transform.position = pinchScript.Position;
        }

        if (startPinch) StartCoroutine(isGrabbingSmth());
        if (endPinch) { StopCoroutine(isGrabbingSmth()); HideMagicCircle(); }
        if (isGrabbing && !objectHidden) HideMagicCircle();
        if (pinching && objectHidden && !isGrabbing) ShowMagicCircle(); // smth wrong here
    }
}
