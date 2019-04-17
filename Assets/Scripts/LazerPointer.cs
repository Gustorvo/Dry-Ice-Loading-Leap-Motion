using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerPointer : MonoBehaviour
{
    public LayerMask excludeLayers; // excluding 
    public LayerMask includeLayers; // including

    public GameObject LaserRedPrefab;
    public GameObject LaserGreenPrefab;

    public RaycastHit hit;
    public bool Hidden { get; private set; }

    private Transform LaserRed;
    private Transform LaserGreen;


    public Transform laserEndPoint;
    public Transform laserstartPoint;

    public bool _enableLaser;


    private void Awake()
    {
        LaserRed = Instantiate(LaserRedPrefab).transform;
        LaserGreen = Instantiate(LaserGreenPrefab).transform;

        LaserRed.gameObject.SetActive(false);
        LaserGreen.gameObject.SetActive(false);

        //gameObject.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        if (Physics.Raycast(laserstartPoint.transform.position, transform.forward, out hit, 5, includeLayers))
        {
            ShowLaser(hit);
        }

        else
        {
            HideLaser();

        }
    }

    private void ShowLaser(RaycastHit hit)
    {
        //if (hit.transform.gameObject.layer == 18)
        // {
        Hidden = false;
        LaserGreen.gameObject.SetActive(true);
        LaserRed.gameObject.SetActive(false);

        LaserGreen.position = Vector3.Lerp(laserstartPoint.transform.position, hit.point, .5f);
        LaserGreen.LookAt(hit.point);
        LaserGreen.localScale = new Vector3(LaserGreen.localScale.x, LaserGreen.localScale.y, hit.distance);

        //iArrow arrow = hit.transform.GetComponentInChildren<iArrow>();
        //if (arrow != null)
        //{
        //    arrow.ShowHideArrow();
        //}
        // }
        //else
        //{
        //    LaserGreen.gameObject.SetActive(false);
        //    LaserRed.gameObject.SetActive(true);

        //    LaserRed.position = Vector3.Lerp(laserstartPoint.transform.position, hit.point, .5f);
        //    LaserRed.LookAt(hit.point);
        //    LaserRed.localScale = new Vector3(LaserRed.localScale.x, LaserRed.localScale.y, hit.distance);
        //}

    }

    private void HideLaser()
    {
        Hidden = true;
        LaserRed.gameObject.SetActive(false);
        LaserGreen.gameObject.SetActive(false);

    }

}
