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
    public Vector3 entryPoint;
    public Vector3 exitPoint;
    public CuttingState state;
    public Vector3 cutPointNormal;
    public float minimumCutDistance;
    public Collider currentCollider;
    public CutInfo()
    {
        entryPoint = Vector3.zero; exitPoint = Vector3.zero; state = CuttingState.NotCutting;
    }

}

[RequireComponent(typeof(Rigidbody))]
public class Cuttable : MonoBehaviour
{
    private Rigidbody rb;
    private Dictionary<string, Cuttable> childrenWedge;
    private MeshRenderer[] childrenMeshRenderers;
    
    //triggers that span a plane intersects the object for each axis
    private BoxCollider horizontalCuttingTrigger;
    private BoxCollider verticalCuttingTriggerX;
    private BoxCollider verticalCuttingTriggerZ;
    
    //used to divide the width of the collider
    public float colliderWidthModifier = 6;
    
    public float triggerValidZoneSize = .5f;

    CutInfo cut;


    private void Awake()
    {
        //get and cache requiered components
        rb = GetComponent<Rigidbody>();
        childrenMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        Cuttable[] childrenWedges = GetComponentsInChildren<Cuttable>();
        cut = new CutInfo();
        //Check if components have CuttableComponent



        CreateTriggerZones();
    }

    public void tryCut(RaycastHit hit)
    {
        Vector3 normal = transform.InverseTransformVector(hit.normal);
        Vector3 hitPoint = transform.InverseTransformPoint(hit.point);
        switch (cut.state)
        {
            case CuttingState.NotCutting:
                cut.state = CuttingState.StartCut;
                goto case CuttingState.StartCut;
            case CuttingState.StartCut:
                Debug.Log(cut.state);
                cut.entryPoint = hitPoint;
                cut.exitPoint = hitPoint;
                cut.cutPointNormal = normal;
                cut.currentCollider = hit.collider;
                cut.minimumCutDistance = getMinimumCutDistance(cut.currentCollider, normal);
                cut.state = CuttingState.IsCutting;
                break;
            case CuttingState.IsCutting:
                if (cut.cutPointNormal != normal || !hit.collider.Equals(cut.currentCollider))
                {
                    cut.state = CuttingState.StartCut;
                    break;
                }
                cut.exitPoint = hitPoint;

                float distance = Vector3.Distance(cut.entryPoint, cut.exitPoint);
                if (distance >= cut.minimumCutDistance)
                {
                    Debug.Log("hasCut");
                    cut.state = CuttingState.StoppedCutting;
                }

                break;
            case CuttingState.StoppedCutting:
                Debug.Log(cut.state);
                cut = new();
                cut.state = CuttingState.NotCutting;
                break;
        }
    }

    private float getMinimumCutDistance(Collider collider, Vector3 normal)
    {
        if (normal.x == 1 || normal.x == -1)
            return collider.Equals(verticalCuttingTriggerX) ? collider.bounds.size.y / 2: collider.bounds.size.z / 2;
        if (normal.z == 1 || normal.z == -1)                                  
            return collider.Equals(verticalCuttingTriggerZ) ? collider.bounds.size.y / 2: collider.bounds.size.x / 2;
        if (normal.y == 1 || normal.y == -1)                                  
            return collider.Equals(verticalCuttingTriggerZ) ? collider.bounds.size.z / 2 : collider.bounds.size.x / 2;
        return 0;
    }
    private void debuggCheckCollider(Collider collider)
    {
        if (collider.Equals(horizontalCuttingTrigger))
            Debug.Log("HorizontalTrigger");
        else if (collider.Equals(verticalCuttingTriggerX))
            Debug.Log("VerticalTriggerX");
        else if (collider.Equals(verticalCuttingTriggerZ))
            Debug.Log("VerticalTriggerZ");
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
}
