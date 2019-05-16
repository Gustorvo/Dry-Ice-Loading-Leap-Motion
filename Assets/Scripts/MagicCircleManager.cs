using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using Leap.Unity.Interaction;
using UnityEngine.Events;


public class MagicCircleManager : MonoBehaviour
{
    
    
    private bool intersectsWithIntObj;

    public LayerMask ignoreLayers;
    public UnityEvent OnCircleShow;
    public UnityEvent OnCircleHide;
    public UnityEvent OnCircleHideButPalmEndFacingFloor;
    public UnityEvent OnCircleActiveAndPalmFacingFloor;
    public UnityEvent OnPalmAcriveButGrabBegin;
    public UnityEvent OnPalmEndFacingFloor;
    public UnityEvent TerminateAll;

    public AnimationClip _animation;
    private Animation _myAnim;
    public float speed = 1;
    public PinchDetector pinchScript;
    public Detector palmScript;
    public InteractionController intController;


    public GameObject Target;
    protected bool pinching;
    protected bool startPinch;
    protected bool endPinch;
    protected bool objectHidden;
    protected bool palmFacingFloor;



    Leap.Hand _hand;
    Controller controller = new Controller();
    private bool isGrabbing;

    private void Start()
    {
        if (Target == null)
            Target = gameObject;

        _myAnim = GetComponent<Animation>();
        HideMagicCircle();

        
    }
    private void Awake()
    {
        
    }

    Hand ReturnHand()
    {
        Frame frame = controller.Frame();  // controller is a Controller object
        if (frame.Hands.Count > 0)
        {
            List<Hand> hands = frame.Hands;
            Hand firstHand = hands[0];
            return firstHand;
        }

        else
        {
            return null;
        }


    }

    IEnumerator isGrabbingSmth()
    {
        
        //isGrabbing = false;
        while (pinching)
        {
            if (intController.isPrimaryHovering) // can be ommitted since IntersectsWithIntObj() does that job already
            {                
                isGrabbing = true;
                break;
            }
            if (ReturnHand() != null) // skip empty frames
            {
                float probability = ReturnHand().GrabStrength;
                if (probability > .9f)
                    isGrabbing = true;
                else isGrabbing = false;
                
            }            
            yield return null;
        }
        //isGrabbing = false;

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
            OnCircleShow.Invoke();
            
            objectHidden = false;
            
            StartCoroutine(CheckPalm());


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
           
            StopCoroutine(CheckPalm());
            if (palmScript.IsActive && !isGrabbing && !IntersectsWithIntObj()) OnCircleHide.Invoke(); // teleprot
            else if (isGrabbing) OnPalmAcriveButGrabBegin.Invoke(); // red color
            else
            {
                OnCircleHideButPalmEndFacingFloor.Invoke();

            }

            TerminateAll.Invoke(); // just to make sure that nothing is missed.

            _myAnim.AddClip(_animation, "Hide");
            _myAnim["Hide"].speed = speed * 1;
            _myAnim["Hide"].time = 0; // _myAnim["Hide"].length;
            _myAnim.Play("Hide");
        }
        

    }

   
    IEnumerator CheckPalm()
    {
        bool invoked = false;
        while (!objectHidden)
        {
            if (palmScript.IsActive && !invoked)
            {
                OnCircleActiveAndPalmFacingFloor.Invoke(); // green
                

                invoked = true;
                //Debug.Log("Invoking");

            }
            else if (!palmScript.IsActive && invoked)
            {
                OnPalmEndFacingFloor.Invoke(); // yellow
               
                invoked = false;
            }
            yield return null;
        }
    }
    private void Terminate()
    {

    }



    void Update()
    {
        IntersectsWithIntObj();

        pinching = pinchScript.IsPinching;
        startPinch = pinchScript.DidStartPinch;
        endPinch = pinchScript.DidEndPinch;

        if (!IntersectsWithIntObj())
        {
            if (startPinch) StartCoroutine(isGrabbingSmth());
            if (endPinch) { StopCoroutine(isGrabbingSmth()); HideMagicCircle(); }


            if (isGrabbing && !objectHidden) HideMagicCircle();
            if (pinching && objectHidden && !isGrabbing) ShowMagicCircle();
        }


    }

    private bool IntersectsWithIntObj()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.15f, ignoreLayers);

        if (hitColliders.Length > 0)
        {
            //Debug.Log(hitColliders.Length);
            isGrabbing = true;
            HideMagicCircle();
            return intersectsWithIntObj = true;    
        }
        else  return intersectsWithIntObj = false;

    }

    private void FixedUpdate()
    {
        if (pinching)
        {
            Target.transform.position = pinchScript.Position;
        }
    }

    private IEnumerator HintTimer()
    {

        float duration = 3;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;

            yield return null;
        }

    }

    

}
