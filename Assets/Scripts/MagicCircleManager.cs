using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using UnityEngine.Events;


public class MagicCircleManager : MonoBehaviour
{
    private Task hintHandler;
    public GameObject Hint;


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
    public PalmDirectionDetector palmScript;


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
        hintHandler = new Task(HintTimer());
        hintHandler.Pause();
    }
    private void Awake()
    {
        if (Target == null)
            Target = gameObject;

        _myAnim = GetComponent<Animation>();
        HideMagicCircle();
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
        Debug.Log("Started to listen to Grab mechanic");
        isGrabbing = false;
        while (pinching)
        {
            if (ReturnHand() == null) // skip empty frames
            {
                break;
            }
            float probability = ReturnHand().GrabStrength;
            if (probability > .99f)
                isGrabbing = true;
            else isGrabbing = false;
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
            OnCircleShow.Invoke();
            hintHandler = new Task(HintTimer()); // start the hint timer
            objectHidden = false;
            StartCoroutine(CheckPlayingState());
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
            StopCoroutine(CheckPlayingState());
            StopCoroutine(CheckPalm());
            if (palmScript.IsActive && !isGrabbing) OnCircleHide.Invoke(); // teleprot
            else if (isGrabbing) OnPalmAcriveButGrabBegin.Invoke(); // red color
            else
            {
                OnCircleHideButPalmEndFacingFloor.Invoke();
                Hint.SetActive(false);
            }

            TerminateAll.Invoke(); // just to make sure that nothing is missed.

            _myAnim.AddClip(_animation, "Hide");
            _myAnim["Hide"].speed = speed * 1;
            _myAnim["Hide"].time = 0; // _myAnim["Hide"].length;
            _myAnim.Play("Hide");
        }

    }

    IEnumerator CheckPlayingState()
    {
        while (_myAnim.isPlaying)
        { yield return new WaitForEndOfFrame(); }
        // if (!objectHidden)
        // OnCircleShow.Invoke();
    }
    IEnumerator CheckPalm()
    {
        bool invoked = false;
        while (!objectHidden)
        {
            if (palmScript.IsActive && !invoked)
            {
                OnCircleActiveAndPalmFacingFloor.Invoke(); // green
                hintHandler.Stop();
                Hint.SetActive(false);
                invoked = true;
                //Debug.Log("Invoking");

            }
            else if (!palmScript.IsActive && invoked)
            {
                OnPalmEndFacingFloor.Invoke(); // yellow
                hintHandler.Start();
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
        pinching = pinchScript.IsPinching;
        startPinch = pinchScript.DidStartPinch;
        endPinch = pinchScript.DidEndPinch;


        if (startPinch) StartCoroutine(isGrabbingSmth());
        if (endPinch) { StopCoroutine(isGrabbingSmth()); HideMagicCircle(); }
       

        if (isGrabbing && !objectHidden) HideMagicCircle();
        if (pinching && objectHidden && !isGrabbing) ShowMagicCircle();
       


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
        Hint.SetActive(true);
    }
}
