using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Leap.Unity.Interaction;

public class EventManager : MonoBehaviour
{
    //public Animator animator;

    public UnityEvent OnIceGraspedWithNoGloves;
    public UnityEvent OnGlovesGetOn;
    public UnityEvent OnAlmostDone;
    public UnityEvent OnDone;
    public bool IceTrayClosed;
    public bool GlovesOn;
    public bool AllIceLoaded;
    public VRTeleporter teleport;

    public GameObject IceTrayArrow;
    public GameObject GlovesArrow;
    public GameObject IcePalletsArrow;

    public GameObject Stage1;
    public GameObject Stage2;

    public GameObject TutorialButton;
    public GameObject RestartButton;

    private bool cardboardGetOpend;
    private int teleportCount;




    // Use this for initialization
    IEnumerator Start()
    {

        yield return new WaitForEndOfFrame();
        FadeManager.instance.FadeIn();
        teleport.OnTeleportBegin += CountTeleport;
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
        DryIce[] iceBlocks = FindObjectsOfType<DryIce>();

        icePlaceholderScript.OnCountChanged += ChechIceCount;
        glovesScript.OnGlovesOn += CheckGloves;
        foreach (var item in doorsScript)
        {
            item.OnDoorOpen += GetDoorStatus;
        }

        foreach (var item in iceBlocks)
        {
            item.GetComponent<InteractionBehaviour>().OnGraspBegin += OnIceBeingGrasped;
        }
    }

    private void OnIceBeingGrasped()
    {
        if (!GlovesOn)
        {
            OnIceGraspedWithNoGloves.Invoke(); // show warning
            GlovesArrow.SetActive(true);
        }
    }

    private void CountTeleport(float nothing)
    {
        teleportCount++;
        if (teleportCount >= 2) GoToNextStage();
    }

    void GoToNextStage()
    {
        TutorialButton.SetActive(false);
        RestartButton.SetActive(true);
        Stage1.SetActive(false);
        Stage2.SetActive(true);
    }

    private void CheckGloves(bool on)
    {
        GlovesOn = on;
        if (GlovesOn)
        {
            OnGlovesGetOn.Invoke(); // hide information board
            GlovesArrow.SetActive(false);
        }

        MakeUpdate();
    }


    private void GetDoorStatus(GameObject go, bool status)
    {
        if (go.name == "DoorsIce")
        {
            IceTrayClosed = status; // true = close, false = open
            //Debug.Log(status);
            if (IceTrayClosed)
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
        GoToNextStage();

        if (!cardboardGetOpend)
            IcePalletsArrow.SetActive(true);
        if (AllIceLoaded && IceTrayClosed)
            OnDone.Invoke();

    }

    void ChechIceCount(int count)
    {
        if (count == 0)
        {
            AllIceLoaded = true;
            OnAlmostDone.Invoke();
        }
        else AllIceLoaded = false;
        MakeUpdate();
    }



    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }



}
