using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
using Leap.Unity.Interaction;




public class ButtonVisibilityTween : MonoBehaviour
{
    bool allowTween = true;

    public bool Move;
    public bool Rotate;
    public bool Scale;
    public bool ScaleOnOutOnly;

    public bool isUsable { get; private set; }
    

    public Transform HiddenTransform;
    private Vector3 InitialPos;
    private Vector3 InitialRot;
    private Vector3 InitialScale;
    Rigidbody rb;
    Collider col;
    Tween myTween;
    public AnimationCurve curveIN;
    public AnimationCurve curveOUT;
    InteractionBehaviour _intObj;
    
    Task deactivatingRoutine; // controlls CoRoutine;

    HingeJoint myHinge;

    private void Awake()
    {
        _intObj = GetComponent<InteractionBehaviour>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
    void Start()
    {

        //yield return new WaitForSeconds(0.1f);

        InitialPos = transform.localPosition;
        InitialRot = transform.localEulerAngles;
        InitialScale = transform.localScale;
        myHinge = GetComponent<HingeJoint>();
        myHinge.connectedBody = null;

        DoHideImmediately();
    }

    private void Update()
    {

    }

    private void OnEnable()
    {
        // _intObj.OnContactStay -= AllowTween;
        // _intObj.OnContactEnd -= DisallowTween;
        _intObj.OnHoverBegin -= StartTrackingWhileOnHover;
        _intObj.OnHoverEnd -= EndTrackingAfterOnHover;

        //_intObj.OnContactStay += AllowTween;
        //_intObj.OnContactEnd += DisallowTween;
        _intObj.OnHoverBegin += StartTrackingWhileOnHover;
        _intObj.OnHoverEnd += EndTrackingAfterOnHover;
    }

    private void OnDisable()
    {
        _intObj.OnContactStay -= AllowTween;
        _intObj.OnContactEnd -= DisallowTween;
        _intObj.OnHoverBegin -= StartTrackingWhileOnHover;
        _intObj.OnHoverEnd -= EndTrackingAfterOnHover;
    }

    void StartTrackingWhileOnHover()
    {
        //StartCoroutine(PauseTween());
    }

    void EndTrackingAfterOnHover()
    {
        //StopCoroutine(PauseTween());
    }

    public void AllowTween()
    {
        allowTween = true;
    }

    public void DisallowTween()
    {
        //allowTween = false;
    }

    IEnumerator PauseTween()
    {
        while (!allowTween)
        {
            if (myTween != null) myTween.Pause();
            yield return null;
        }
    }



    public void DoShow(float duration)
    {
        if (deactivatingRoutine.Running)
        {
            deactivatingRoutine.Stop();
        }
                        
        if (myTween != null) myTween.TogglePause();
        gameObject.SetActive(true);
        rb.isKinematic = true;
        col.enabled = false;

        if (Move) myTween = gameObject.transform.DOLocalMove(InitialPos, duration).SetEase(curveIN);
        if (Scale || ScaleOnOutOnly) myTween = gameObject.transform.DOScale(InitialScale, duration);
        if (Rotate) myTween = gameObject.transform.DOLocalRotate(InitialRot, duration);

        myTween.OnComplete(AttachConnectedBody);

    }


    public void DoHide(float duration)
    {        
        deactivatingRoutine = new Task(HideRoutine(duration, 0));
    }

    public void DoHideWithDelay(float delay)
    {        
        if (gameObject.activeInHierarchy) deactivatingRoutine = new Task(HideRoutine(0.15f, delay));
    }



    IEnumerator HideRoutine(float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        //yield return new WaitUntil(() => allowTween);

        if (myTween != null) myTween.TogglePause();

        isUsable = false;
        myHinge.connectedBody = null;
        rb.isKinematic = true;
        col.enabled = false;

        if (Scale || ScaleOnOutOnly) myTween = gameObject.transform.DOScale(Vector3.zero, duration);
        if (Move) myTween = gameObject.transform.DOLocalMove(HiddenTransform.localPosition, duration).SetEase(curveOUT);
        if (Rotate) myTween = gameObject.transform.DOLocalRotate(HiddenTransform.localEulerAngles, duration);

        myTween.OnComplete(DoFinal);

    }

    public void DoHideImmediately()
    {
        DoHide(0);
    }

    private void AttachConnectedBody()
    {
        myHinge.connectedBody = transform.root.GetComponent<Rigidbody>();
        isUsable = true;
        rb.isKinematic = false;
        col.enabled = true;        
    }


    private void DoFinal()
    {
        gameObject.SetActive(false);
        if (ScaleOnOutOnly) transform.DOScale(InitialScale, 0);        
    }

    public void PauseDeactivatingRoutine()
    {
        if (deactivatingRoutine != null && deactivatingRoutine.Running)
            DoShow(0);

    }
}

