using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LightController : MonoBehaviour {

    Light _light;
    
    Tween tween;
    
    
	// Use this for initialization
	void Start () {
        _light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		
            
    }
    private void OnEnable()
    {
        
    }
   

    public void IncreaseIntensity(float value)
    {        
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        tween = _light.DOIntensity(value, 1f);
        
    }

    public void DecreaseIntensity(float value)
    {
        if (tween != null && tween.IsPlaying()) tween.Kill(); 
        tween = _light.DOIntensity(value, 0.15f);
    }

    
}
