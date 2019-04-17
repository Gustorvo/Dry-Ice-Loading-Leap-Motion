using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using DG.Tweening;

public class DryIcePlaceholder : MonoBehaviour
{

    public List<GameObject> Ice;

    IDryIce Iice;
    InteractionBehaviour _IntIce;

    bool shouldTween;

    void Start()
    {

    }


    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Iice = other.GetComponent<IDryIce>();
        if (Iice != null)
        {
            Iice.BePlaced();
            StartCoroutine("PlaceIce", other.transform);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Iice != null)
        {
            Iice = null;
            StopCoroutine("PlaceIce");
        }
    }

    IEnumerator PlaceIce(Transform ice)
    {
        _IntIce = ice.GetComponent<InteractionBehaviour>();
        shouldTween = true;
        while (Iice != null && _IntIce != null)
        {
            // Highlight
            LoopColor();
           

            if (!_IntIce.isGrasped)
            {
                ice.transform.DOMove(Ice[0].transform.position, 0.25f).OnComplete(MakeKinematic);
                ice.transform.DORotate(Ice[0].transform.rotation.eulerAngles, 0.1f);// // GetComponent<Rigidbody>().MovePosition(Ice[1].transform.position);

                Ice.RemoveAt(0);

                break;
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }

    }

    void LoopColor()
    {
        if (shouldTween)
        {           
            Ice[0].GetComponent<Renderer>().material.DOColor(Color.green, .5f).SetLoops(4, LoopType.Yoyo);
            Ice[0].GetComponent<Renderer>().material.DOFade(.5f, .5f).SetLoops(4, LoopType.Yoyo).OnComplete(DisableMeshRenderer);
        }
        shouldTween = false;
    }

    void MakeKinematic()
    {
        _IntIce.GetComponent<Rigidbody>().isKinematic = true;
    }

    void DisableMeshRenderer()
    {
        Ice[0].GetComponent<Renderer>().enabled = false;
    }
}
