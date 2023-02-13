using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public enum CuttingState { NotCutting, StartCut, IsCutting, ReachedCenter, StoppedCutting}
public enum ColliderPlane { None = 0, XY, XZ, YZ}

class CutInfo
{
    //This class stores the information of a cut that is currently being made to the object

    public Vector3 entryPoint; //the point where the cut started
    public Vector3 exitPoint; //the last detected point
    public CuttingState state; //the current state of the cut
    public Vector3 cutPointNormal; //the normal of the current collider
    public float minimumCutDistance; //the distance that we should travel in the object to register a cut
    public Collider currentCollider; //the collider that initiated the cut
    public CutInfo()
    {
        entryPoint = Vector3.zero; exitPoint = Vector3.zero; state = CuttingState.NotCutting;
    }

}

[RequireComponent(typeof(Rigidbody))]
public class Cuttable : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer[] childrenMeshRenderers;
    private GameObject[,,] wedges;
    public ColliderPlane lastCutPlane;

    //triggers that span a plane intersects the object for each axis
    private BoxCollider horizontalCuttingTrigger;
    private BoxCollider verticalCuttingTriggerX;
    private BoxCollider verticalCuttingTriggerZ;

    
    //used to divide the width of the collider
    public float colliderWidthModifier = 4;
    private int numberOfCuts;
   

    CutInfo cut;


    private void Awake()
    {
        //get and cache requiered components
        rb = GetComponent<Rigidbody>();
        childrenMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        Transform[] childrentransform = GetComponentsInChildren<Transform>();
        InitializeWedgeArray(childrentransform);

        //Initialize Variables
        cut = new CutInfo();
        cut.state = CuttingState.NotCutting;
        numberOfCuts = 0;

        //Set Layer To Correct Layer

        gameObject.layer = LayerMask.NameToLayer("Food");

        //Check if components have CuttableComponent    

        CreateTriggerZones();
    }

    public void tryCut(RaycastHit hit)
    {
        //transform the normal and point from worldspace to localspace
        Vector3 hitPoint = transform.InverseTransformPoint(hit.point);
        
        switch (cut.state)
        {
            case CuttingState.NotCutting:
                goto case CuttingState.StartCut;
            case CuttingState.StartCut:
                //get cut info from the raycast
                cut.entryPoint = hitPoint;
                cut.exitPoint = hitPoint;
                cut.cutPointNormal = hit.normal;
                cut.currentCollider = hit.collider;
                cut.minimumCutDistance = getMinimumCutDistance(cut.currentCollider, hit.normal);
                //start the cut
                cut.state = CuttingState.IsCutting;
                break;
            case CuttingState.IsCutting:
                Debug.Log(cut.state);
                // detect if the face of the object we are cutting change, or if we exited the current collider and hit another
                if (cut.cutPointNormal != hit.normal)
                {
                    cut.state = CuttingState.StartCut;
                    break;
                }
                
                cut.exitPoint = hitPoint;
                float distance = Vector3.Distance(cut.entryPoint, cut.exitPoint);
                if (distance >= cut.minimumCutDistance)
                {
                    cut.state = CuttingState.StoppedCutting;
                    lastCutPlane = getColliderEnum(cut.currentCollider);
                    ProcessCut(lastCutPlane);
                }
                break;
            case CuttingState.StoppedCutting:
                //erase information of the cut
                cut = new();
                cut.state = CuttingState.NotCutting;
                break;
        }
    }

    private void ProcessCut(ColliderPlane colliderPlane)
    {
        gameObject.SetActive(false);
        ArrayList leftHalf= new ArrayList();
        ArrayList rightHalf= new ArrayList();
        GameObject leftParent;
        GameObject rightParent;
        reParentCut(colliderPlane, ref leftHalf, ref rightHalf, out leftParent, out rightParent);
        setNewParent(leftParent.transform, leftHalf);
        setNewParent(rightParent.transform, rightHalf);
        Cuttable leftCuttable = null;
        Cuttable right = null;
        Rigidbody leftRb = null;
        Rigidbody rightRb = null;

        BoxCollider leftCollider = leftParent.AddComponent<BoxCollider>();
        BoxCollider rightCollider = rightParent.AddComponent<BoxCollider>();
        leftCollider.size = gameObject.transform.localScale;
        rightCollider.size = gameObject.transform.localScale;
        leftCollider.center = leftParent.transform.localPosition;
        rightCollider.center = rightParent.transform.localPosition;


        leftRb = leftParent.AddComponent<Rigidbody>();
        rightRb = rightParent.AddComponent<Rigidbody>();

        //if (numberOfCuts < 3)
        //{
        //    left = leftParent.AddComponent<Cuttable>();
        //    right = rightParent.AddComponent<Cuttable>(); 
        //}

        Toppingable leftTopping = leftParent.AddComponent<Toppingable>();
        Toppingable rightTopping = rightParent.AddComponent<Toppingable>();
        leftParent.AddComponent<Pickupable>();
        rightParent.AddComponent<Pickupable>();


        if (leftCuttable != null)
        {
            leftCuttable.numberOfCuts = numberOfCuts + 1;
            leftCuttable.lastCutPlane = lastCutPlane;
        }
        if (right != null)
        {
            right.numberOfCuts = numberOfCuts + 1;
            right.lastCutPlane = lastCutPlane;
        }

        leftTopping.ready = true;
        rightTopping.ready = true;
        //leftRb.useGravity = false;
        //rightRb.useGravity = false;

    }

    private void OnTriggerExit(Collider other)
    {
        cut = new();
        cut.state = CuttingState.NotCutting;
    }

    private float getMinimumCutDistance(Collider collider, Vector3 normal)
    {
        if (normal.x == 1 || normal.x == -1)
            return collider.Equals(verticalCuttingTriggerX) ? collider.bounds.size.y : collider.bounds.size.z ;
        if (normal.z == 1 || normal.z == -1)                                  
            return collider.Equals(verticalCuttingTriggerZ) ? collider.bounds.size.y : collider.bounds.extents.x ;
        if (normal.y == 1 || normal.y == -1)                                  
            return collider.Equals(verticalCuttingTriggerZ) ? collider.bounds.size.z : collider.bounds.size.x ;
        return 0;
    }
    private void debuggCheckCollider(Collider collider)
    {
        if (collider.Equals(horizontalCuttingTrigger))
        {
            foreach (MeshRenderer renderer in childrenMeshRenderers)
                renderer.material.color = Color.green;
            Debug.Log("Horizontal Collider");
        }
        else if (collider.Equals(verticalCuttingTriggerX))
        {
            foreach (MeshRenderer renderer in childrenMeshRenderers)
                renderer.material.color = Color.red;
            Debug.Log("Vertical collider X");
        }
        else if (collider.Equals(verticalCuttingTriggerZ))
        {
            foreach (MeshRenderer renderer in childrenMeshRenderers)
                renderer.material.color = Color.blue;
            Debug.Log("Vertical Collider Z");
        }
        
    }
    private ColliderPlane getColliderEnum(Collider collider)
    {
        if (collider.Equals(horizontalCuttingTrigger))
        {
            return ColliderPlane.XZ;
        }
        else if (collider.Equals(verticalCuttingTriggerX))
        {
            return ColliderPlane.XY;
        }
        else if (collider.Equals(verticalCuttingTriggerZ))
        {
            return ColliderPlane.YZ;
        }
        return ColliderPlane.None;

    }
    //Function that creates three trigger zones in each axis of the cuttable game object to detect for cut
    void CreateTriggerZones()
    {
        //Create cutting colliders
        switch (lastCutPlane)
        {
            case ColliderPlane.None:
                horizontalCuttingTrigger = gameObject.AddComponent<BoxCollider>();
                verticalCuttingTriggerZ = gameObject.AddComponent<BoxCollider>();
                verticalCuttingTriggerX = gameObject.AddComponent<BoxCollider>();
                break;
            case ColliderPlane.XY:
                horizontalCuttingTrigger = gameObject.AddComponent<BoxCollider>();
                verticalCuttingTriggerZ = gameObject.AddComponent<BoxCollider>();
                verticalCuttingTriggerX = null;
                break;
            case ColliderPlane.XZ:
                horizontalCuttingTrigger = null;
                verticalCuttingTriggerZ = gameObject.AddComponent<BoxCollider>();
                verticalCuttingTriggerX = gameObject.AddComponent<BoxCollider>();
                break;
            case ColliderPlane.YZ:
                horizontalCuttingTrigger = gameObject.AddComponent<BoxCollider>();
                verticalCuttingTriggerZ = null;
                verticalCuttingTriggerX = gameObject.AddComponent<BoxCollider>();
                break;
            default:
                break;
        }

        //Get sum of bounds from children wedges
        Bounds bounds = new Bounds();
        bounds.center = transform.position;
        foreach (MeshRenderer wedgeRenderer in childrenMeshRenderers)
        {
            bounds.Encapsulate(wedgeRenderer.bounds);
        }

        //Define collider dimensions and set as triggers
        if (horizontalCuttingTrigger)
        {
            horizontalCuttingTrigger.size = new Vector3((bounds.extents.x * 2) / transform.localScale.x, (bounds.extents.y * 2 / transform.localScale.y) / colliderWidthModifier, (bounds.extents.z * 2) / transform.localScale.z);
            horizontalCuttingTrigger.isTrigger= true;
        }
        if (verticalCuttingTriggerX)
        {
            verticalCuttingTriggerX.size = new Vector3((bounds.extents.x * 2) / transform.localScale.x, (bounds.extents.y * 2 / transform.localScale.y), (bounds.extents.z * 2 / transform.localScale.z) / colliderWidthModifier);
            verticalCuttingTriggerX.isTrigger= true;    
        }
        if (verticalCuttingTriggerZ)
        {
            verticalCuttingTriggerZ.size = new Vector3((bounds.extents.x * 2 / transform.localScale.x) / colliderWidthModifier, (bounds.extents.y * 2)/ transform.localScale.y, (bounds.extents.z * 2) / transform.localScale.z);
            verticalCuttingTriggerZ.isTrigger= true;
        }
    
    }
    void InitializeWedgeArray(Transform[] childrentransform)
    {
        childrentransform = GetComponentsInChildren<Transform>();
        wedges = new GameObject[2, 2, 2];
        int index = 1;
        for (int y = 0; y <= 1; y++)
            for (int x = 0; x <= 1; x++)
                for (int z = 0; z <= 1; z++)
                    if (index < childrentransform.Length)
                    {
                        wedges[x, y, z] = childrentransform[index].gameObject;
                        index += 2;
                    }
    }
    private void setNewParent(Transform newParent, Transform objectToParent)
    {
        objectToParent.SetParent(null, true);
        objectToParent.SetParent(newParent, false);
    }
    private void setNewParent(Transform newParent, ArrayList objectsToParent)
    {
        foreach (GameObject gameObject in objectsToParent)
        {
            setNewParent(newParent, gameObject.transform);
        }
    }
    private void reParentCut(ColliderPlane colliderPlane, ref ArrayList leftHalf, ref ArrayList rightHalf, out GameObject leftParent, out GameObject rightParent)
    {
        leftParent = new GameObject();
        leftParent.name = "OnionHalf" + numberOfCuts + 1;
        rightParent = new GameObject();
        rightParent.name = "OnionHalf" + numberOfCuts + 2;
        leftParent.transform.position = transform.position;
        rightParent.transform.position = transform.position;
        switch (colliderPlane)
        {
            case ColliderPlane.XY:
                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        leftHalf.Add(wedges[x, y, 1]);
                leftParent.transform.position += new Vector3(0, .5f, -transform.position.z / 2);
                

                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        rightHalf.Add(wedges[x, y, 0]);
                rightParent.transform.position += new Vector3(0, .5f, transform.position.z / 2);

                break;
            case ColliderPlane.XZ:
                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int z = 0; z < wedges.GetLength(1); z++)
                        leftHalf.Add(wedges[x, 1, z]);
                leftParent.transform.position += new Vector3(0, transform.position.y / 2, 0);

                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int z = 0; z < wedges.GetLength(1); z++)
                        rightHalf.Add(wedges[x, 0, z]);
                rightParent.transform.position += new Vector3(0, -transform.position.y / 2, 0);

                break;
            case ColliderPlane.YZ:
                for (int z = 0; z < wedges.GetLength(0); z++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        leftHalf.Add(wedges[1, y, z]);
                leftParent.transform.position += new Vector3(-transform.position.x / 2, .5f, 0);

                for (int z = 0; z < wedges.GetLength(0); z++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        rightHalf.Add(wedges[0, y, z]);
                rightParent.transform.position += new Vector3(transform.position.x / 2, .5f, 0);

                break;
            case ColliderPlane.None:
                break;
            default:
                break;
        }
    }


}
