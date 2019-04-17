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

    // Use this for initialization
    void Start()
    {
        if (TargetTransform == null)
            TargetTransform = gameObject.transform;

        InitialPos = TargetTransform.localPosition;
        InitialRot = TargetTransform.localEulerAngles;
        InitialScale = TargetTransform.localScale;

        DoHideImmediately();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoShowWithDelay(float delay)
    {
        activatingRoutine = new Task(ShowRoutine(0.35f, delay));
    }

    public void DoShow(float duration)
    { 
        activatingRoutine = new Task(ShowRoutine(duration, 0));
    }

    IEnumerator ShowRoutine(float duration, float delay)
    { 
        yield return new WaitForSecondsRealtime(delay);
        if (deactivatingRoutine != null && deactivatingRoutine.Running) deactivatingRoutine.Stop();


        //if (myTween != null) myTween.TogglePause();
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
        deactivatingRoutine = new Task(HideRoutine(0.15f, delay));
    }

    IEnumerator HideRoutine(float duration, float delay)
    {        
        yield return new WaitForSecondsRealtime(delay);
        if (activatingRoutine != null && activatingRoutine.Running) activatingRoutine.Stop();

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
