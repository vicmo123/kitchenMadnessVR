using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cuttable : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;
    private Dictionary<string, Cuttable> childrenWedge;
    private MeshRenderer[] childrenMeshRenderers;

    private BoxCollider horizontalCuttingTrigger;
    private BoxCollider verticalCuttingTriggerZ;
    private BoxCollider verticalCuttingTriggerX;

    //used to divide the width of the collider
    public float colliderWidthModifier = 8; 

    private void Awake()
    {
        //get and cache requiered components
        rb = GetComponent<Rigidbody>();
        Cuttable[] childrenWedges = GetComponentsInChildren<Cuttable>();
        childrenMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        //CreateT
        CreateTriggerZones();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateTriggerZones()
    {
        //Create cutting colliders
        horizontalCuttingTrigger = gameObject.AddComponent<BoxCollider>();
        verticalCuttingTriggerX= gameObject.AddComponent<BoxCollider>();
        verticalCuttingTriggerZ= gameObject.AddComponent<BoxCollider>();
        
        //SetCollider to be triggers
        horizontalCuttingTrigger.isTrigger = true;
        verticalCuttingTriggerZ.isTrigger = true;
        verticalCuttingTriggerX.isTrigger = true;

        //Get sum of bounds from children wedges
        Bounds bounds = new Bounds();
        foreach (MeshRenderer wedgeRenderer in childrenMeshRenderers)
        {
            bounds.Encapsulate(wedgeRenderer.bounds);
        }

        //Define collider dimensions
        
        horizontalCuttingTrigger.size = new Vector3((bounds.extents.x * 2) / transform.localScale.x, (bounds.extents.y * 2 / transform.localScale.y) / colliderWidthModifier , (bounds.extents.z * 2) / transform.localScale.z);
        verticalCuttingTriggerZ.size = new Vector3((bounds.extents.x * 2) / transform.localScale.x, (bounds.extents.y * 2 / transform.localScale.y), (bounds.extents.z * 2 / transform.localScale.z) / colliderWidthModifier);
        verticalCuttingTriggerX.size = new Vector3((bounds.extents.x * 2 / transform.localScale.x) / colliderWidthModifier, (bounds.extents.y * 2)/ transform.localScale.y, (bounds.extents.z * 2) / transform.localScale.z);
    }
}
