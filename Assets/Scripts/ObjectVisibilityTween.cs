using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectVisibilityTween : MonoBehaviour
{
    public bool Move;
    public bool Rotate;
    public bool Scale;
    public bool ScaleOnOutOnly;
    public bool useWorldCoordinates;
    public bool debugShowHide;
    private bool m_debugShowHide;
    public float debugDuration;


    public bool isUsable { get; private set; }

    public Transform HiddenTransform;
    public Transform TargetTransform;

    private Vector3 InitialPos;
    private Vector3 InitialRot;
    private Vector3 InitialScale;

    Tween myTween;
    public AnimationCurve curveIN;
    public AnimationCurve curveOUT;

    Task deactivatingRoutine; // controlls CoRoutine;
    Task activatingRoutine;


    private void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        // myTween.SetAutoKill(false);
        if (TargetTransform == null)
            TargetTransform = gameObject.transform;

        GetInitialTransform();
        //InitialPos = TargetTransform.localPosition;
        //InitialRot = TargetTransform.localEulerAngles;
        //InitialScale = TargetTransform.localScale;

        DoHideImmediately();

    }

    public void DebugOpenClose(float duration)
    {
        if (debugShowHide)
            DoShow(duration);
        else
            DoHide(duration);
    }

    // Update is called once per frame
    void Update()
    {
        if (debugShowHide != m_debugShowHide)
        {
            m_debugShowHide = debugShowHide;
            DebugOpenClose(debugDuration);
        }
    }

    private void GetInitialTransform()
    {
        if (useWorldCoordinates)
        {
            InitialPos = TargetTransform.position;
            InitialRot = TargetTransform.eulerAngles;
            InitialScale = TargetTransform.lossyScale;
        }
        else
        {
            InitialPos = TargetTransform.localPosition;
            InitialRot = TargetTransform.localEulerAngles;
            InitialScale = TargetTransform.localScale;
        }
    }

    public void DoShowWithDelay(float delay)
    {
        activatingRoutine = new Task(ShowRoutine(0.5f, delay));
    }

    public void DoShow(float duration)
    {
        if (deactivatingRoutine != null && deactivatingRoutine.Running) deactivatingRoutine.Stop();
        activatingRoutine = new Task(ShowRoutine(duration, 0));

    }

    IEnumerator ShowRoutine(float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);


        if (myTween != null) myTween.TogglePause();
        TargetTransform.gameObject.SetActive(true);


        if (Move) myTween = TargetTransform.DOLocalMove(InitialPos, duration).SetEase(curveIN);
        if (Scale || ScaleOnOutOnly) myTween = TargetTransform.DOScale(InitialScale, duration);
        if (Rotate) myTween = TargetTransform.DOLocalRotate(InitialRot, duration);

        myTween.OnComplete(MakeUsable);

    }

    void MakeUsable()
    {
        isUsable = true;
    }

    public void DoHide(float duration)
    {       // if (deactivatingRoutine != null && deactivatingRoutine.Running) deactivatingRoutine.Stop();
        deactivatingRoutine = new Task(HideRoutine(duration, 0));
    }

    public void DoHideWithDelay(float delay)
    {
        deactivatingRoutine = new Task(HideRoutine(0.5f, delay));
    }

    IEnumerator HideRoutine(float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (activatingRoutine != null && activatingRoutine.Running)
        {
            activatingRoutine.Stop();
            activatingRoutine = null;
        }

        //yield return new WaitUntil(() => allowTween);

        if (myTween != null && myTween.IsPlaying()) myTween.Pause();
        isUsable = false;


        if (Scale || ScaleOnOutOnly) myTween = TargetTransform.DOScale(Vector3.zero, duration);
        if (Move) myTween = TargetTransform.DOLocalMove(HiddenTransform.localPosition, duration).SetEase(curveOUT);
        if (Rotate) myTween = TargetTransform.DOLocalRotate(HiddenTransform.localEulerAngles, duration);

        myTween.OnComplete(DoFinal);

    }

    public void DoHideImmediately()
    {

        DoHide(0);
    }




    private void DoFinal()
    {
        TargetTransform.gameObject.SetActive(false);
        if (ScaleOnOutOnly) TargetTransform.DOScale(InitialScale, 0);
    }
}
