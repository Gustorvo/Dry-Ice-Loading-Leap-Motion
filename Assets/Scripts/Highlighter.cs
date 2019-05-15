using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;

[RequireComponent(typeof(InteractionBehaviour))]


public class Highlighter : MonoBehaviour
{

    private Material _material;
    public InteractionBehaviour _intObj;
    public bool useHover = true;
    public bool usePrimaryHover = false;
    public bool useOutliner;
    public bool useGrasp;
    public bool useContact;


    private Color defaultColor;
    public Color suspendedColor = Color.red;
    public Color hoverColor = Color.Lerp(Color.black, Color.white, 0.7F);
    public Color graspColor = Color.Lerp(Color.green, Color.green, 0.7F);
    public Color graspOutlineColor = Color.Lerp(Color.green, Color.yellow, 0.7F);
    public Color hoverOutlineColor = Color.Lerp(Color.white, Color.yellow, 0.7F);
    public Color primaryHoverColor = Color.Lerp(Color.black, Color.white, 0.8F);
    public Color pressedColor = Color.white;
    public Renderer myRenderer;
    private QuickOutline _outliner;




    // Use this for initialization
    void Start()
    {
        _intObj = GetComponent<InteractionBehaviour>();

        myRenderer = GetComponent<Renderer>();
        if (myRenderer == null)
        {
            myRenderer = GetComponentInChildren<Renderer>();
        }
        if (myRenderer != null)
        {
            _material = myRenderer.material;
        }

        defaultColor = _material.color;

        if (useGrasp)
        {
            useOutliner = true;
            _intObj.OnGraspBegin += ControllOutline;
            _intObj.OnGraspEnd += ControllOutline;
        }
        if (useContact)
        {
            useOutliner = true;
            _intObj.OnContactBegin += ContactBeginOutline;
            _intObj.OnContactEnd += ControllOutline;
        }

        if (useOutliner)
        {
            _outliner = GetComponent<QuickOutline>();
            if (_outliner == null)
            {
                _outliner = gameObject.AddComponent<QuickOutline>();
            }
            
            _outliner.OutlineMode = QuickOutline.Mode.OutlineAll;
            
            _outliner.OutlineWidth = 0f;

            _intObj.OnHoverBegin += ControllOutline;
            _intObj.OnHoverEnd += ControllOutline;            
        }

    }

    void ContactBeginOutline()
    {
        _outliner.OutlineWidth = 6;
        _outliner.OutlineColor = graspOutlineColor;
    }

    
    void ControllOutline()
    {
        if (_intObj.isGrasped)
        {
            _outliner.OutlineWidth = 6;
            _outliner.OutlineColor = graspOutlineColor;
        }
        
        else if (_intObj.isHovered || _intObj.isPrimaryHovered)
        {
            _outliner.OutlineWidth = 2;
            _outliner.OutlineColor = hoverOutlineColor;
        }
        else _outliner.OutlineWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_material != null)
        {

            // The target color for the Interaction object will be determined by various simple state checks.
            Color targetColor = defaultColor;

            // "Primary hover" is a special kind of hover state that an InteractionBehaviour can
            // only have if an InteractionHand's thumb, index, or middle finger is closer to it
            // than any other interaction object.
            if (_intObj.isPrimaryHovered && usePrimaryHover)
            {
                targetColor = primaryHoverColor;
            }
            else if (_intObj.isGrasped)
            {
                targetColor = graspColor;
            }
            else
            {
                // Of course, any number of objects can be hovered by any number of InteractionHands.
                // InteractionBehaviour provides an API for accessing various interaction-related
                // state information such as the closest hand that is hovering nearby, if the object
                // is hovered at all.
                if (_intObj.isHovered && useHover)
                {
                    float glow = _intObj.closestHoveringControllerDistance.Map(0F, 0.2F, 1F, 0.0F);
                    targetColor = Color.Lerp(defaultColor, hoverColor, glow);
                }

            }

            if (_intObj.isSuspended)
            {
                // If the object is held by only one hand and that holding hand stops tracking, the
                // object is "suspended." InteractionBehaviour provides suspension callbacks if you'd
                // like the object to, for example, disappear, when the object is suspended.
                // Alternatively you can check "isSuspended" at any time.
                targetColor = suspendedColor;
            }

            // We can also check the depressed-or-not-depressed state of InteractionButton objects
            // and assign them a unique color in that case.
            if (_intObj is InteractionButton && (_intObj as InteractionButton).isPressed)
            {
                targetColor = pressedColor;
            }


            // Lerp actual material color to the target color.
            _material.color = Color.Lerp(_material.color, targetColor, 30F * Time.deltaTime);
        }
    }
}
