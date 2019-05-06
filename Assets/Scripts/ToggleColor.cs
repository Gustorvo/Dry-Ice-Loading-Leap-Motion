using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleColor : MonoBehaviour
{

    private Material _mat;
    public Material toggRedleMaterial;
    public Color yellow;
    private Color _originalCol;
   

   


    // Use this for initialization
    void Start()
    {
       
        
    }

    private void Awake()
    {
        _mat = GetComponent<Renderer>().material;
        _originalCol = _mat.color;





    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle_Color(bool activeColor)
    {
        if (activeColor)
        {
            GetComponent<Renderer>().material = _mat;
            _mat.color = _originalCol;
            
        }
        else
        {
            GetComponent<Renderer>().material = _mat;
            _mat.color = yellow;
            
        }
    }

    public void ToggleRedColor()
    {
        GetComponent<Renderer>().material = toggRedleMaterial;
      
    }
}
