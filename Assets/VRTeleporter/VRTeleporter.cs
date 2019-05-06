using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VRTeleporter : MonoBehaviour
{
    public Vector3 Offset;
    private bool runNormal;

    private Task AnimatedArcRoutine;
    public float speed;
    public Vector3 mvelocity = Vector3.one;
    public float smoothTime = 0.3F;
    public GameObject positionMarker; // marker for display ground position

    public Transform bodyTransforn; // target transferred by teleport

    public LayerMask excludeLayers; // excluding 
    public LayerMask includeLayers; // including 

    public float angle = 45f; // Arc take off angle

    public float strength = 10f; // Increasing this value will increase overall arc length


    int maxVertexcount = 35; // limitation of vertices for performance. 
    [SerializeField]
    private float vertexDelta = 0.08f; // Delta between each Vertex on arc. Decresing this value may cause performance problem. 0.08f is a def value

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
        if (groundDetected && runNormal)
        {
            // bodyTransforn.position = groundPos + lastNormal * 0.01f;
            StartCoroutine(TeleportOverSpeed(bodyTransforn, (groundPos + lastNormal * 0.01f), 10));
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

        StartCoroutine(EnableLineRendererOnNextFrame(active));
        if (active && AnimatedArcRoutine == null)
        {
            runNormal = false;
            AnimatedArcRoutine = new Task(BeginAnimateArc());
        }
        if (!active)
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

    }

    private void Update()
    {
        if (displayActive)
        {
            UpdatePath();
        }

    }

    IEnumerator BeginAnimateArc()
    {
        //yield return new WaitForEndOfFrame();
        
        arcRenderer.positionCount = 0;
        arcRenderer.enabled = true;
        while (arcRenderer.positionCount != vertexList.ToArray().Length -1)
        {
            for (int i = 1; i < vertexList.ToArray().Length; i++)
            {
                Vector3[] tempPos = vertexList.ToArray();
                
                Array.Resize(ref tempPos, i ); // increments arch path every frame
                
                arcRenderer.positionCount = i;
                arcRenderer.SetPositions(tempPos);
                yield return new WaitForEndOfFrame();
            }

        }
        
        runNormal = true;        // start render the arch nornally, every frame.
        AnimatedArcRoutine = null;
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
            if (Physics.Linecast(pos, newPos, out hit, ~excludeLayers))// includeLayers))
            {
                groundDetected = true;
                groundPos = hit.point;
                lastNormal = hit.normal;
            }

            pos = newPos; // update current vertex as last vertex
        }


        // positionMarker.SetActive(groundDetected);

        if (groundDetected)
        {
            //positionMarker.transform.position = Vector3.SmoothDamp(positionMarker.transform.position, groundPos + lastNormal * 0.01f, ref mvelocity, smoothTime);


            positionMarker.transform.position = groundPos + lastNormal * 0.01f; //original implementation
            positionMarker.transform.LookAt(groundPos);
        }

        // Update Line Renderer every frame (full path)

        if (runNormal)
        {
            positionMarker.SetActive(groundDetected);
            arcRenderer.enabled = groundDetected;
            arcRenderer.positionCount = vertexList.Count -1; // -1 to skipp rendering the end of the arch
            Vector3[] tempPos = vertexList.ToArray();
            Array.Resize(ref tempPos, vertexList.Count -1); // -1 to skipp rendering the end of the arch
            arcRenderer.SetPositions(tempPos);
        }

    }



    public IEnumerator TeleportOverSpeed(Transform objectToMove, Vector3 end, float speed) //teleporter over time
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end + Offset)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end + Offset, speed * Time.deltaTime);
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