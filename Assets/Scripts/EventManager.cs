using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public Animator animator;


    public bool IceTrayOpen;
    public bool GlovesOn;
    public bool AllIceLoaded;

    public GameObject IceTrayArrow;
    public GameObject GlovesArrow;
    public GameObject IcePalletsArrow;

    private bool cardboardGetOpend;


   

    // Use this for initialization
    IEnumerator Start()
    {
        
        yield return new WaitForEndOfFrame();
        FadeManager.instance.FadeIn();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FadeManager.instance.FadeOutToScene(0);           
        }
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
        else if (go.CompareTag("cardboard"))
        {
            IcePalletsArrow.SetActive(false);
            cardboardGetOpend = true;
        }

        MakeUpdate();
    }
    void MakeUpdate()
    {
        if (IceTrayOpen && !GlovesOn)
        {
            GlovesArrow.SetActive(true);
        }

        else
        {
            GlovesArrow.SetActive(false);
            if (!cardboardGetOpend)
                IcePalletsArrow.SetActive(true);
        }
    }

    void  ChechIceCount(int count)
    {
        if (count == 0)
            AllIceLoaded = true;
        else AllIceLoaded = false;
    }

    public void FadeToLevel()
    {
        animator.SetTrigger("FadeOut");
       
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }



}
