using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Leap.Unity.Interaction;
using System;

public class DryIce : MonoBehaviour, IDryIce
{

    [FMODUnity.EventRef]
    public string OnIceDroppedSound;

    [FMODUnity.EventRef]
    public string OnIceGraspedSound;


    private InteractionBehaviour intObj;


    public Action<GameObject> OnIceReleased = delegate { };
    public Action<GameObject> OnIceGrasped = delegate { };

    public void PlaySound()
    {
        string EventSound;
        if (intObj.isGrasped) EventSound = OnIceGraspedSound;
        else EventSound = OnIceDroppedSound;

        FMODUnity.RuntimeManager.PlayOneShot(EventSound, transform.position);

    }

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        intObj = GetComponent<InteractionBehaviour>();

        intObj.OnGraspEnd += UpdateStatus;
        intObj.OnGraspBegin += UpdateStatus;
        intObj.OnGraspBegin += PlaySound;
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
