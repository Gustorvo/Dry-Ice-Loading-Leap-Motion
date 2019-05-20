using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class FacingFloorDetector : Detector
{
    private List<Vector3> vertexList = new List<Vector3>(); // vertex on arc 
   

    public LayerMask excludeLayers; // excluding 
    public LayerMask includeLayers; // including 

    public float angle = 45f; // Arc take off angle

    public float strength = 10f; // Increasing this value will increase overall arc length


    int maxVertexcount = 35; // limitation of vertices for performance. 
    [SerializeField]
    private float vertexDelta = 0.08f; // Delta between each Vertex on arc. Decresing this value may cause performance problem. 0.08f is a def value

   

    private Vector3 velocity; // Velocity of latest vertex

    private Vector3 groundPos; // detected ground position  
    public Transform StartingPoint;
    

    // Use this for initialization
    private void Awake()
    {


    }
    void Start()
    {


    }

    bool groundDetected;
    void CheckFloorDetection()
    {
        groundDetected = false;      
        vertexList.Clear(); // delete all previouse vertices
        velocity = Quaternion.AngleAxis(-angle, StartingPoint.right) * StartingPoint.forward * strength;
        Vector3 pos = StartingPoint.position; // take off position
        vertexList.Add(pos);

        while (!groundDetected && vertexList.Count < maxVertexcount)
        {
            Vector3 newPos = pos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;

            velocity += Physics.gravity * vertexDelta;

            vertexList.Add(newPos); // add new calculated vertex
            RaycastHit hit;
            // linecast between last vertex and current vertex
            if (Physics.Linecast(pos, newPos, out hit, ~excludeLayers))// includeLayers))
            {
                //GroundDetected = true;
                groundDetected = true;
                Activate();
                groundPos = hit.point;                
            }
            else Deactivate();

            pos = newPos; // update current vertex as last vertex
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckFloorDetection();

    }
}