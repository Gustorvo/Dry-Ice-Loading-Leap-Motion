using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleColor : MonoBehaviour
{

    private Material _mat;
    public Material toggRedleMaterial;
    public Color yellow;
    private Color _originalCol;
    public bool useOutline;
    private QuickOutline outliner;





    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0);
        

    }

    private void Awake()
    {
        if (useOutline)
            outliner = GetComponent<QuickOutline>();

        RetrieveOriginalColor();



    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle_Color(bool activeColor)
    {
        if (activeColor)
        {
           
           ChangeMaterilaTo(_mat, _originalCol);
            if (useOutline)
                outliner.OutlineWidth = 0;

        }
        else
        {
            ChangeMaterilaTo(_mat, yellow);
            if (useOutline)
                outliner.OutlineWidth = 2;

        }
    }

    public void ToggleRedColor()
    {
        GetComponent<Renderer>().material = toggRedleMaterial;

    }

    private void ChangeMaterilaTo(Material _mat, Color _color)
    {
        if (GetComponent<Renderer>() == null) 
        {
            Renderer[] _ren = GetComponentsInChildren<Renderer>();
            foreach (var item in _ren)
            {
                item.material = _mat;
                _mat.color = _color;
            }
        }
        else
        {
            GetComponent<Renderer>().material = _mat;
            _mat.color = _color;
        }
    }

    private void RetrieveOriginalColor()
    {
        if (GetComponent<Renderer>() == null)
        {
            Renderer[] _ren = GetComponentsInChildren<Renderer>();
            foreach (var item in _ren)
            {
                _mat = item.material;
                _originalCol = _mat.color;
                
            }
        }
        else
        {
            _mat = GetComponent<Renderer>().material;
            _originalCol = _mat.color;
        }
    }
}
