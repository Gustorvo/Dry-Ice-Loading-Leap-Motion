using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleColor : MonoBehaviour {

    private Material _mat;
    public Color yellow;
    private Color _originalCol;
    public Color red;
   

    // Use this for initialization
    void Start () {
        //_mat = GetComponent<Renderer>().material;
        //_originalCol = _mat.color;
	}

    private void Awake()
    {
        _mat = GetComponent<Renderer>().material;
        _originalCol = _mat.color;
       

    }
    // Update is called once per frame
    void Update () {
		
	}

    public void Toggle_Color(bool activeColor)
    {
        if (activeColor) _mat.color = _originalCol;
        else _mat.color = yellow;
    }

    public void ToggleRedColor()
    {
        _mat.color = red;
    }
}
