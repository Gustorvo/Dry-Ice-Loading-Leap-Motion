using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using DG.Tweening;
using System;

public class DryIcePlaceholder : MonoBehaviour
{

    public List<GameObject> Ice;

    public List<GameObject> IceUsed;

    IDryIce Iice;
    InteractionBehaviour _IntIce;

    bool shouldTween;
    bool icePalletInside;

    public Action<int> OnCountChanged = delegate { };


    void Start()
    {
        //Debug.Log(Ice.Count);

        DryIce[] iceScript = FindObjectsOfType<DryIce>();
        foreach (var item in iceScript)
        {
            item.OnIceReleased += UpdatePlaceholder;
            item.OnIceGrasped += OutlineEmptyIfGrasped;
        }
    }

    private void UpdatePlaceholder(GameObject go)
    {
        icePalletInside = true;
        StartCoroutine("PlaceIce", go.transform);
    }



    public IEnumerator PlaceIce(Transform ice)
    {
        _IntIce = ice.GetComponent<InteractionBehaviour>();
        shouldTween = true;

        while (icePalletInside) //(Iice != null && _IntIce != null)
        {
            // Highlight
            LoopColor(Ice[0]);

            if (_IntIce == null)
                Debug.Log("int obj is null " + _IntIce.gameObject);
            if (_IntIce.isGrasped)
            {
                _IntIce.ReleaseFromGrasp();
                _IntIce.ignoreGrasping = true;
                yield return new WaitForSeconds(0.25f);
                _IntIce.ignoreGrasping = false;
            }
            if (!_IntIce.isGrasped || _IntIce.isSuspended)
            {
                ice.transform.DOMove(Ice[0].transform.position, 0.25f).OnComplete(MakeKinematic);
                TriggerSoundEvent(_IntIce); // trigger sound event on the DryIce game object
                ice.transform.DORotate(Ice[0].transform.rotation.eulerAngles, 0.1f);// // GetComponent<Rigidbody>().MovePosition(Ice[1].transform.position);

                IceUsed.Add(Ice[0]);
                Ice[0].GetComponent<IceEventListener>().ListenToEvent(ice.gameObject);
                Ice.RemoveAt(0);

                icePalletInside = false;
                break;
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }

        OnCountChanged(Ice.Count);

    }

    void LoopColor(GameObject go)
    {
        if (shouldTween)
        {
            tempGO = go;
            //go.GetComponent<QuickOutline>().enabled = true;
            go.GetComponent<QuickOutline>().OutlineWidth = 4;
            go.GetComponent<MeshRenderer>().enabled = true;
            go.GetComponent<Renderer>().material.DOColor(Color.green, .25f).SetLoops(2, LoopType.Yoyo);
            go.GetComponent<Renderer>().material.DOFade(.001f, .25f).SetLoops(2, LoopType.Yoyo).OnComplete(DisableMeshRenderer);
        }
        shouldTween = false;
    }

    void MakeKinematic()
    {
        _IntIce.GetComponent<Rigidbody>().isKinematic = true;


    }

    void TriggerSoundEvent(InteractionBehaviour intObj)
    {
        Iice = intObj.GetComponent<DryIce>();
        if (Iice != null)
        {
            Iice.PlaySound();
        }
    }

    GameObject tempGO;
    void DisableMeshRenderer()
    {
        tempGO.GetComponent<QuickOutline>().OutlineWidth = 0;
        tempGO.GetComponent<Renderer>().enabled = false;
        //tempGO.GetComponent<QuickOutline>().enabled = false;
        tempGO = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dryicepallet"))
        {

            StartCoroutine(OutlinePlaceholder());
        }
    }

    void OutlineEmptyIfGrasped(GameObject go)
    {
        StartCoroutine(OutlinePlaceholder());
    }

    IEnumerator OutlinePlaceholder()
    {
        yield return new WaitForSecondsRealtime(0.25f);

        if (Ice.Count != 0)
        {
            GameObject go = Ice[0];

            QuickOutline _outline = go.GetComponent<QuickOutline>();
            go.GetComponent<MeshRenderer>().enabled = true;
            // _outline.enabled = true;
            while (_outline.OutlineWidth < 4)
            {
                _outline.OutlineWidth += 0.25f;
                yield return new WaitForSeconds(0.05f);
            }
            while (_outline.OutlineWidth == 0)
            {
                _outline.OutlineWidth -= 0.25f;
                yield return new WaitForSeconds(0.05f);
            }
            //_outline.enabled = false;
            go.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
