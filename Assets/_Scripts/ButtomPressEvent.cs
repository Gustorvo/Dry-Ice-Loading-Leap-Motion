using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Leap.Unity.Interaction;
using DG.Tweening;
using FMODUnity;
using TMPro;

[RequireComponent(typeof(InteractionBehaviour))]
[RequireComponent(typeof(HingeJoint))]

public class ButtomPressEvent : MonoBehaviour
{
    [EventRef]
    public string ButtonPressSound;

    public UnityEvent OnPress = new UnityEvent();
    HingeJoint _joint;
    InteractionBehaviour _intBtn;
    private bool _buttonEnabled;
    private ButtonVisibilityTween _tween;
    Tween myTween;

    bool pressed;
    Material _mat;
    Color _color;
    public Color CoorToBlendTo;

    public TextMeshPro m_Text;

    private float offsetAngle;





    // Use this for initialization
    void Start()
    {
        _joint = GetComponent<HingeJoint>();
        _intBtn = GetComponent<InteractionBehaviour>();
        _tween = GetComponent<ButtonVisibilityTween>();

        _mat = GetComponent<Renderer>().materials[2];
        _color = _mat.color;
        // myTween = _mat.DOFade(_color.a, 0);
        AngleOffset();

    }

    private bool CheckBtn()
    {
        if (_tween != null)
        {
            return _tween.isUsable;
        }
        else
            return true;
    }


    void AngleOffset()
    {
        offsetAngle = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        //m_Text.text = Mathf.Round((transform.localEulerAngles.z - offsetAngle)).ToString();

        if (_intBtn.isHovered && CheckBtn())
            CheckButtonAngle();
       
    }

    private void CheckButtonAngle()
    {
        if (_joint.limits.max - (transform.localEulerAngles.z - offsetAngle) < 0.5f && !pressed)
        {            
            OnPress.Invoke();
            pressed = true;
            PlaySound();
            ChangeButtonColorOnPress();
        }



        if ((transform.localEulerAngles.z - offsetAngle) - _joint.limits.min < 3)
        {
            pressed = false;
            //ResetColorBack();
        }

        //if (_joint.limits.max - _joint.angle < 1f)

    }

    private void ChangeButtonColorOnPress()
    {
        myTween = _mat.DOColor(CoorToBlendTo, 0.1f).SetLoops(2, LoopType.Yoyo);

    }

    private void ResetColorBack()
    {
        myTween = _mat.DOColor(_color, 0.2f);
    }

    private void PlaySound()
    {
        if (ButtonPressSound.Length > 1)
            RuntimeManager.PlayOneShot(ButtonPressSound, transform.position);
    }
}
