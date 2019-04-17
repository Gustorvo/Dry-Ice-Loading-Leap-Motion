using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsOpenClose : MonoBehaviour, IDoor
{
    public bool XRotate;
    public bool YRotate;
    public bool ZRotate;
    public float rotationAngle;
    public float speed;
    public float openDelay;
    [SerializeField]
    protected bool ON;
    protected bool rotate;
    protected Vector3 currentRotateVector;
    protected Quaternion ClosedRotation, OpenRotation, TargetRotation;
    protected Transform _transform;

    public GameObject OlderItemRestrictsOpen;
    public GameObject OlderItemRestrictsClose;
    public GameObject PairDoor;
    public MeshRenderer Mesh;
    public int MaterialNum;
    public Material TransparentMaterial;
    private Material OpaqueMaterial;

    public Collider[] cols;
    private Collider[] othersCols;

    private bool _openCommandComesOutside;

    private void Awake()
    {
        InitStartData();
        if (Mesh != null)
        {
            Material[] mat = Mesh.materials;
            OpaqueMaterial = mat[MaterialNum];
        }

        cols = GetComponentsInChildren<Collider>();

        GetCollidersInOthers();
        CheckRestriction();
    }
    void OnEnable()
    {
        // DoorsLogo_Reset.ResetAction += Reset;
    }
    void OnDisable()
    {
        //DoorsLogo_Reset.ResetAction -= Reset;
    }

    private void InitStartData()
    {


        Vector3 rotateVector = Vector3.zero;
        _transform = transform;
        if (GetComponent<Collider>() == null) gameObject.AddComponent<BoxCollider>();
        if (_transform.CompareTag("Untagged")) _transform.tag = Constant.TAGDOORS;
        speed = speed != 0 ? speed : 200f;
        if (!XRotate && !YRotate && !ZRotate) print("!!!!!!! set rotation axis for " + _transform.name + " !!!!!!");

        ClosedRotation = _transform.localRotation;
        OpenRotation = _transform.localRotation;

        // openDelay = openDelay == 0 ? 0.2f : openDelay;

        if (XRotate) rotateVector = new Vector3(1, 0, 0);
        else if (YRotate) rotateVector = new Vector3(0, 1, 0);
        else if (ZRotate) rotateVector = new Vector3(0, 0, 1);

        Vector3 vector = OpenRotation.eulerAngles;
        vector += (rotateVector * rotationAngle);
        OpenRotation.eulerAngles = vector;
        rotateVector *= speed;
    }
    public void OpenCloseDoorsFromPaired()
    {
        _openCommandComesOutside = true;
        OpenCloseDoor();
    }

    public void OpenCloseDoor()
    {
        if (ON && OlderItemRestrictsClose != null && OlderItemRestrictsClose.GetComponent<IDoor>().GetOpened())
        {
            print(name + " !!!!!!!!! restrict open");
            Debug.Break();
            return;
        }
        if (!ON && OlderItemRestrictsOpen != null && !OlderItemRestrictsOpen.GetComponent<IDoor>().GetOpened())
        {
            print(name + " !!!!!!!! restrict close");
            Debug.Break();
            return;
        }

        TargetRotation = ON ? ClosedRotation : OpenRotation;
        
        ON = !ON;
        CheckRestriction();

        Hide();
        if (PairDoor != null && !_openCommandComesOutside)
        {
            DoorsOpenClose dr = PairDoor.GetComponent<DoorsOpenClose>();
            if (dr != null) dr.OpenCloseDoorsFromPaired();
        }
        _openCommandComesOutside = false;

        Invoke("StartMove", openDelay);


    }

    void Hide()
    {
        if (Mesh == null) return;
        Material[] mat = Mesh.materials;
        mat[MaterialNum] = ON ? TransparentMaterial : OpaqueMaterial;
        Mesh.materials = mat;
    }

    void StartMove() { rotate = true; }

    //----------------------------------------------------------
    void Update()
    {
        if (rotate)
        {
            _transform.localRotation = Quaternion.RotateTowards(transform.localRotation, TargetRotation, Time.deltaTime * speed); //Time.deltaTime*speed);
            if (Quaternion.Angle(_transform.localRotation, TargetRotation) < .1f)
            {
                _transform.localRotation = TargetRotation;
                rotate = false;
            }
        }
    }
    public bool GetOpened()
    {
        return ON;
    }

    private void Reset()
    {
        if (!ON) return;

        ON = false;
        _openCommandComesOutside = false;
        Hide();
        _transform.localRotation = ClosedRotation;
        rotate = false;
    }


    private IEnumerator CollidersOnOff (bool ON)
    {
       yield return new WaitForSecondsRealtime(0.5f);
        foreach (var col in othersCols)
        {
            col.enabled = ON;
        }

    }
    public void CheckRestriction()
    {

        if (OlderItemRestrictsClose != null && ON)
        {
            StartCoroutine("CollidersOnOff", ON);
                      
        }

        if (OlderItemRestrictsClose != null && !ON)
        {
            StartCoroutine("CollidersOnOff", ON);
            
        }


        if (OlderItemRestrictsOpen != null && ON)
        {
            StartCoroutine("CollidersOnOff", !ON);
                       
        }

        if (OlderItemRestrictsOpen != null && !ON)
        {
            StartCoroutine("CollidersOnOff", !ON);
           
        }
        
        
    }

    void GetCollidersInOthers()
    {
        if (OlderItemRestrictsClose != null)
            othersCols = OlderItemRestrictsClose.GetComponentsInChildren<Collider>();
        if (OlderItemRestrictsOpen != null)
            othersCols = OlderItemRestrictsOpen.GetComponentsInChildren<Collider>();
    }

}
