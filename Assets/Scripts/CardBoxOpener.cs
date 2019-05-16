using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class CardBoxOpener : MonoBehaviour
{

    private DoorsOpenClose[] boxes;
    void Start()
    {
        boxes = GetComponentsInChildren<DoorsOpenClose>();


        //boxes[0].OnOpenCloseBegin += DisableColliders;
        //boxes[0].OnOpenCloseEnd += EnableColliders;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenCloseCardBox()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<InteractionBehaviour>().ignoreHoverMode = IgnoreHoverMode.Both;
        foreach (var side in boxes)
        {
            
            side.OpenCloseDoor();
        }
    }

    
}
