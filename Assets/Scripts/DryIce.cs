using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Leap.Unity.Interaction;
using System;

public class DryIce : MonoBehaviour, IDryIce
{


    private InteractionBehaviour intObj;


    public Action<GameObject> OnIceReleased;
    public Action<GameObject> OnIceGrasped;

    public void BePlaced()
    {

    }

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        intObj = GetComponent<InteractionBehaviour>();

        intObj.OnGraspEnd += UpdateStatus;
        intObj.OnGraspBegin += UpdateStatus;
    }


    private Collider _collider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IcePlaceholder"))
            StartCoroutine(ReleaseIntObjAfterDelay(1));

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("IcePlaceholder"))
        {
            _collider = other;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IcePlaceholder"))
        {
            _collider = null;
        }

    }

    void UpdateStatus()
        
    {        
        if (intObj == null) Debug.Log("int obj is null " + intObj.gameObject);

        if (_collider != null && !intObj.isGrasped)
            OnIceReleased(gameObject);
        else if (_collider != null && intObj.isGrasped)
        {
            if (OnIceGrasped != null)
                OnIceGrasped(gameObject);
        }
    }

    IEnumerator ReleaseIntObjAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        intObj.ReleaseFromGrasp();
        intObj.ignoreGrasping = true;
        yield return new WaitForSeconds(0.5f);
        intObj.ignoreGrasping = false;
    }
}
