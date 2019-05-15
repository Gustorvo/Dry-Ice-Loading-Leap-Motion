using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Leap.Unity.Interaction;
using Leap.Unity.Encoding;
using System.Linq;
using System;
using TMPro;
using Leap;

public class ContactDetection : MonoBehaviour
{
    Finger.FingerType MiddleFinger = Finger.FingerType.TYPE_MIDDLE;
    Finger.FingerType IndexFinger = Finger.FingerType.TYPE_INDEX;

    public TextMeshPro m_Text;

    public UnityEvent OnHoverBegin;
    public UnityEvent OnHoverEnd;

    public UnityEvent OnContactBegin;
    public UnityEvent OnContactEnd;

    public UnityEvent OnContactCenterBegin;
    public UnityEvent OnContactCenterEnd;

    private float distanceToCenter;

    public float activationDistance;
    public float deactivationDistance;

    bool contactPending;

   public Collider _collider;
    public List<Collider> _cols;

    IDoor parentIDoor;
    DoorsOpenClose parentDoorRotator;




    private InteractionBehaviour _intObj;
    // Use this for initialization
    void Start()
    {       //MiddleFinger

        _intObj = GetComponent<InteractionBehaviour>();
        _intObj.OnHoverBegin += ActivateHover;
        _intObj.OnHoverEnd += DeactivateHover;
        _intObj.OnContactBegin += OnContactBegin.Invoke;
        _intObj.OnContactEnd += OnContactEnd.Invoke;
        _collider = GetComponent<Collider>();
        

        if (_intObj.manager.hoverActivationRadius < deactivationDistance)
        {
            Debug.LogWarning("!!!!!!!!!!!!  Adjust min distance   !!!!!!!!!!!!!!");
            Debug.Break();

        }
    }   

    

    void ActivateHover()
    {
        OnHoverBegin.Invoke();
    }

    void DeactivateHover()
    {
        OnHoverEnd.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

        if (_intObj.isHovered)
        {
            _cols = _intObj.primaryHoverColliders;
            distanceToCenter = Vector3.Distance(_collider.bounds.center, _intObj.primaryHoveringControllerPoint);
            
            if (distanceToCenter < activationDistance && !contactPending)
            {
                StartCoroutine(CheckContactDistance());
                contactPending = true;
            }

        }
    }



    IEnumerator CheckContactDistance()
    {
        OnContactCenterBegin.Invoke();
        yield return new WaitForSecondsRealtime(0.1f);
       
        while (distanceToCenter < deactivationDistance)
        {
            //m_Text.text = distanceToCenter.ToString();
            contactPending = true;
            yield return null;// WaitForSecondsRealtime(0.1f);
        }
        contactPending = false;        
        OnContactCenterEnd.Invoke();
        yield return new WaitForSecondsRealtime(0.1f);       

    }
}
