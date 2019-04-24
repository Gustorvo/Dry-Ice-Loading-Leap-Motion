﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTeleporter : MonoBehaviour
{
    public float speed;
    public Vector3 mvelocity = Vector3.one;
    public float smoothTime = 0.3F;
    public GameObject positionMarker; // marker for display ground position

    public Transform bodyTransforn; // target transferred by teleport

    public LayerMask excludeLayers; // excluding 
    public LayerMask includeLayers; // including 

    public float angle = 45f; // Arc take off angle

    public float strength = 10f; // Increasing this value will increase overall arc length


    int maxVertexcount = 50; // limitation of vertices for performance. 

    private float vertexDelta = 0.08f; // Delta between each Vertex on arc. Decresing this value may cause performance problem.

    private LineRenderer arcRenderer;

    private Vector3 velocity; // Velocity of latest vertex

    private Vector3 groundPos; // detected ground position

    private Vector3 lastNormal; // detected surface normal

    private bool groundDetected = false;

    private List<Vector3> vertexList = new List<Vector3>(); // vertex on arc

    private bool displayActive = false; // don't update path when it's false.


    // Teleport target transform to ground position
    public void Teleport()
    {
        if (groundDetected)
        {
            // bodyTransforn.position = groundPos + lastNormal * 0.01f;
            StartCoroutine(MoveOverSpeed(bodyTransforn, (groundPos + lastNormal * 0.01f), 10));
            ToggleDisplay(false);

        }
        else
        {
            Debug.Log("Ground wasn't detected");
        }
    }

    // Active Teleporter Arc Path
    public void ToggleDisplay(bool active)
    {
        displayActive = active;
        //arcRenderer.enabled = active; //use Coroutine instead, to avoid jettering (next line of code)
        StartCoroutine(EnableLineRendererOnNextFrame(active));
        positionMarker.SetActive(active);
        


    }


    private void Awake()
    {
        arcRenderer = GetComponent<LineRenderer>();
        arcRenderer.enabled = false;
        positionMarker.SetActive(false);
    }

    private void Start()
    {
        // arcRenderer.enabled = true; experimental
    }

    private void Update()
    {
        if (displayActive)
        {
            UpdatePath();
        }
    }




    private void UpdatePath()
    {
        groundDetected = false;

        vertexList.Clear(); // delete all previouse vertices


        velocity = Quaternion.AngleAxis(-angle, transform.right) * transform.forward * strength;

        RaycastHit hit;


        Vector3 pos = transform.position; // take off position

        vertexList.Add(pos);

        while (!groundDetected && vertexList.Count < maxVertexcount)
        {
            Vector3 newPos = pos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;

            velocity += Physics.gravity * vertexDelta;

            vertexList.Add(newPos); // add new calculated vertex

            // linecast between last vertex and current vertex
            if (Physics.Linecast(pos, newPos, out hit, includeLayers))//~excludeLayers))
            {
                groundDetected = true;
                groundPos = hit.point;
                lastNormal = hit.normal;
            }

            pos = newPos; // update current vertex as last vertex
        }


        positionMarker.SetActive(groundDetected);

        if (groundDetected)
        {
            //positionMarker.transform.position = Vector3.SmoothDamp(positionMarker.transform.position, groundPos + lastNormal * 0.01f, ref mvelocity, smoothTime);


            positionMarker.transform.position = groundPos + lastNormal * 0.01f; //original implementation
            positionMarker.transform.LookAt(groundPos);
        }

        // Update Line Renderer

        arcRenderer.positionCount = vertexList.Count;
        arcRenderer.SetPositions(vertexList.ToArray());
    }

    public IEnumerator MoveOverSpeed(Transform objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator EnableLineRendererOnNextFrame(bool active)
    {
        if (active)
            yield return new WaitForEndOfFrame(); // fixes jittering on enabling LineRenderer before moving its position;
        arcRenderer.enabled = active;
        
    }


}