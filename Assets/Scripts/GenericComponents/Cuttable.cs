using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public enum CuttingState { NotCutting, StartCut, IsCutting, ReachedCenter, StoppedCutting}

class CutInfo
{
    public Vector3 entryPoint;
    public Vector3 exitPoint;
    public CuttingState state;
    public Vector3 cutPointNormal;

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
    public float colliderWidthModifier = 8;
    
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
        switch (cut.state)
        {
            case CuttingState.NotCutting:
                cut.state = CuttingState.StartCut;
                break;
            case CuttingState.StartCut:
                cut.entryPoint = hit.point;
                cut.exitPoint = hit.point;
                break;
            case CuttingState.IsCutting:

                break;
            case CuttingState.ReachedCenter:
                break;
            case CuttingState.StoppedCutting:
                break;
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
}
