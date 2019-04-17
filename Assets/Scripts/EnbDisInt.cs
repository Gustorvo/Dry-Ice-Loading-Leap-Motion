using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnbDisInt : MonoBehaviour
{


    DoorsOpenClose _doors;
    Collider _col;

    // Use this for initialization
    void Start()
    {
        _doors = GetComponent<DoorsOpenClose>();
        if (_doors == null)
            gameObject.SetActive(false);
        _col = GetComponent<Collider>();

        CheckRestriction();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckRestriction()
    {
        if (_doors.OlderItemRestrictsClose != null)
            _col.enabled = !_doors.OlderItemRestrictsClose.GetComponent<IDoor>().GetOpened();


        if (_doors.OlderItemRestrictsOpen != null)
            _col.enabled = _doors.OlderItemRestrictsOpen.GetComponent<IDoor>().GetOpened();
    }
}
