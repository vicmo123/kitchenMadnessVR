using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public enum CuttingState { NotCutting, StartCut, IsCutting, ReachedCenter, StoppedCutting}
public enum ColliderPlane { XY, XZ, YZ}

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

        //Initialize Variables
        cut = new CutInfo();
        cut.state = CuttingState.NotCutting;
        numberOfCuts = 0;

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
                    Debug.Log("has cut");
                    cut.state = CuttingState.StoppedCutting;
                    debuggCheckCollider(cut.currentCollider);
                    ProcessCut();
                }
                break;
            case CuttingState.StoppedCutting:
                //erase information of the cut
                cut = new();
                cut.state = CuttingState.NotCutting;
                break;
        }
    }

    private void ProcessCut()
    {
        
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
    //Function that creates three trigger zones in each axis of the cuttable game object to detect for cut
    void CreateTriggerZones()
    {
        //Create cutting colliders
        horizontalCuttingTrigger = gameObject.AddComponent<BoxCollider>();
        verticalCuttingTriggerZ= gameObject.AddComponent<BoxCollider>();
        verticalCuttingTriggerX= gameObject.AddComponent<BoxCollider>();
        
        //SetCollider to be triggers
        horizontalCuttingTrigger.isTrigger = true;
        verticalCuttingTriggerX.isTrigger = true;
        verticalCuttingTriggerZ.isTrigger = true;

        //Get sum of bounds from children wedges
        Bounds bounds = new Bounds();
        bounds.center = transform.position;
        foreach (MeshRenderer wedgeRenderer in childrenMeshRenderers)
        {
            bounds.Encapsulate(wedgeRenderer.bounds);
        }

        //Define collider dimensions
        horizontalCuttingTrigger.size = new Vector3((bounds.extents.x * 2) / transform.localScale.x, (bounds.extents.y * 2 / transform.localScale.y) / colliderWidthModifier , (bounds.extents.z * 2) / transform.localScale.z);
        verticalCuttingTriggerX.size = new Vector3((bounds.extents.x * 2) / transform.localScale.x, (bounds.extents.y * 2 / transform.localScale.y), (bounds.extents.z * 2 / transform.localScale.z) / colliderWidthModifier);
        verticalCuttingTriggerZ.size = new Vector3((bounds.extents.x * 2 / transform.localScale.x) / colliderWidthModifier, (bounds.extents.y * 2)/ transform.localScale.y, (bounds.extents.z * 2) / transform.localScale.z);
    
    }
    void InitializeWedgeArray()
    { 
        int index = 0;
        for (int y = 0; y <= 1; y++)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int z = 0; z <= 1; z++)
                {
                    wedges[x,y,z] = childrenMeshRenderers[index++].gameObject;
                }
            }
        }
    }
}
