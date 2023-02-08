using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cuttable : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;
    private Dictionary<string, Cuttable> childrenWedge;

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

        //Define collider dimensions
        horizontalCuttingTrigger.size = new Vector3(transform.localScale.x, transform.localScale.y / colliderWidthModifier, transform.localScale.z);
        verticalCuttingTriggerZ.size = new Vector3(transform.localScale.x / colliderWidthModifier, transform.localScale.y, transform.localScale.z);
        verticalCuttingTriggerX.size = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / colliderWidthModifier);
    }
}
