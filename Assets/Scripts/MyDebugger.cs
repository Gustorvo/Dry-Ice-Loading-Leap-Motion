using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyDebugger : MonoBehaviour
{

    public UnityEvent RunAtStart;

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        if (RunAtStart != null)
        { RunAtStart.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PrintDebug()
    {
        Debug.Log("DEtected");
    }
}
