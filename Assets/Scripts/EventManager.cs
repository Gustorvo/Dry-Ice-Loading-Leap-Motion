using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public bool IceTrayOpen;
    public bool GlovesOn;
    public bool AllIceLoaded;

    public GameObject IceTrayArrow;
    public GameObject GlovesArrow;
    public GameObject IcePalletsArrow;




    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        DoorsOpenClose[] doorsScript = FindObjectsOfType<DoorsOpenClose>();
        ToggleHandsModel glovesScript = FindObjectOfType<ToggleHandsModel>();
        DryIcePlaceholder icePlaceholderScript = FindObjectOfType<DryIcePlaceholder>();

        icePlaceholderScript.OnCountChanged += ChechIceCount;
        glovesScript.OnGlovesOn += CheckGloves;
        foreach (var item in doorsScript)
        {
            item.OnDoorOpen += GetDoorStatus;
        }
    }

    private void CheckGloves(bool on)
    {
        GlovesOn = on;
        Debug.Log(GlovesOn);
        MakeUpdate();
    }


    private void GetDoorStatus(GameObject go, bool status)
    {

        if (go.name == "DoorsIce")
        {
            IceTrayOpen = status;
            if (IceTrayOpen)
            {
                IceTrayArrow.SetActive(false);
            }
        }
        MakeUpdate();
    }
    void MakeUpdate()
    {
        if (IceTrayOpen && !GlovesOn)
        {           
            GlovesArrow.SetActive(true);
        }
        else GlovesArrow.SetActive(false);
    }

    void  ChechIceCount(int count)
    {
        if (count == 0)
            AllIceLoaded = true;
        else AllIceLoaded = false;
    }



}
