using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

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

    private void Awake()
    {
        //get and cache requiered components
        rb = GetComponent<Rigidbody>();
        childrenMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        Cuttable[] childrenWedges = GetComponentsInChildren<Cuttable>();

        //Check if components have CuttableComponent
        
        
        CreateTriggerZones();
    }

    public void tryCut(RaycastHit hit)
    {
        Bounds bound;
        Bounds opositeBound;
        
        GameObject testObject = Instantiate(gameObject);
        testObject.transform.position = hit.point;
    }

    private void createValidBoundsInTrigger(Collider collider, out Bounds bound, out Bounds opositeBound)
    {
        if (collider.Equals(verticalCuttingTriggerX))
        {
            Vector3 center = verticalCuttingTriggerX.bounds.center + new Vector3(verticalCuttingTriggerX.bounds.extents.x - triggerValidZoneSize, 0, 0);
            Vector3 size = new Vector3(triggerValidZoneSize * 2, verticalCuttingTriggerX.bounds.extents.y * 2, verticalCuttingTriggerX.bounds.extents.z);
            bound = new Bounds(center, size);
            center = verticalCuttingTriggerX.bounds.center - new Vector3(verticalCuttingTriggerX.bounds.extents.x + triggerValidZoneSize, 0, 0);
            opositeBound= new Bounds(center, size);
        }
        else if (collider.Equals(verticalCuttingTriggerZ))
        {
            Vector3 center = verticalCuttingTriggerZ.bounds.center + new Vector3(0, 0, verticalCuttingTriggerX.bounds.extents.z - triggerValidZoneSize);
            Vector3 size = new Vector3(triggerValidZoneSize * 2, verticalCuttingTriggerX.bounds.extents.y * 2, verticalCuttingTriggerX.bounds.extents.z * 2);
            bound = new Bounds(center, size);
            center = verticalCuttingTriggerX.bounds.center - new Vector3(0, 0, verticalCuttingTriggerX.bounds.extents.z + triggerValidZoneSize);
            opositeBound = new Bounds(center, size);
        }
        else
        {
            bound = new Bounds();
            opositeBound = new Bounds(); 
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
