using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEventListener : MonoBehaviour {

    DryIcePlaceholder IcePalletsList;

    void Start()
    {
        IcePalletsList = GetComponentInParent<DryIcePlaceholder>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ListenToEvent(GameObject go)
    {
        tag = "occupied";
        go.GetComponent<DryIce>().OnIceGrasped += UpdateStatus;
    }

    private void UpdateStatus(GameObject go)
    {
        go.GetComponent<DryIce>().OnIceGrasped -= UpdateStatus;
        tag = "empty";
        if (IcePalletsList.IceUsed.Count != 0)
        {
            foreach (var item in IcePalletsList.IceUsed)
            {
                if (item.CompareTag("empty"))
                {
                    IcePalletsList.Ice.Insert(0, item);
                    IcePalletsList.IceUsed.Remove(item);
                    break;
                }
            }
        }
    }
}
