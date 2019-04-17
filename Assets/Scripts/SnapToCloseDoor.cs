using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

[RequireComponent(typeof(InteractionBehaviour))]

public class SnapToCloseDoor : MonoBehaviour
{

    private InteractionBehaviour _intObj;
    private HingeJoint _joint;
    bool isClosing;
    Vector3 origPos;
    Vector3 origRot;
    float prAngle;
    bool snapped = true;

    // Use this for initialization
    void Start()
    {
        _joint = GetComponent<HingeJoint>();
        prAngle = _joint.angle;

        StartCoroutine(RotationDirection());
    }
    private void OnEnable()
    {
        _intObj = GetComponent<InteractionBehaviour>();
        origPos = transform.position;
        origRot = transform.rotation.eulerAngles;

    }
    private void OnDisable()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (_intObj.isHovered && isClosing)
        {
            if (_joint.angle - _joint.limits.min < 10)
            {
                SnapToClose();
            }
        }
    }

    void SnapToClose()
    {
        if (!snapped)
        {
            _intObj.rigidbody.transform.position = origPos;
            _intObj.rigidbody.rotation = Quaternion.Euler(origRot);
            _intObj.ignoreGrasping = true;
            _intObj.ignorePrimaryHover = true;
            _intObj.ignoreGrasping = false;
            _intObj.ignorePrimaryHover = false;

            Debug.Log("Snaped");
        }

    }
    IEnumerator RotationDirection()
    {
        while (true)
        {
            prAngle = _joint.angle;
            yield return new WaitForSecondsRealtime(0.15f);
            if (_joint.angle < prAngle) isClosing = true;
            else isClosing = false;
            if ((transform.rotation.eulerAngles - origRot) == Vector3.zero)
            {
                snapped = true;
            }
            else
            {
                snapped = false;
                
               // Debug.Log(transform.rotation.eulerAngles);
            }
        }
    }

    //bool CheckAngle()
    //{
    //    crAngle = _joint.angle;
    //    if (crAngle < prAngle) return true;

    //    else if (crAngle > prAngle) return false;

    //    else return false;        

    //}


}
