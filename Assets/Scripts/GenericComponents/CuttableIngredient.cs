using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum CuttingState { NotCutting, StartCut, IsCutting, ReachedCenter}
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
public class CuttableIngredient : MonoBehaviour,InterFace_Cutter
{
    //reflections
    public void CopyComponentsToObject(GameObject toCopyTo)
    {
        try
        {
            Component[] allComp = gameObject.GetComponents<Component>();
            List<System.Type> allTypes = new List<System.Type>();
            foreach (Component c in allComp)
            {
                if (c.GetType() != typeof(Transform) && c.GetType() != typeof(BoxCollider))
                {
                    allTypes.Add(c.GetType());
                }
            }

            foreach (System.Type t in allTypes)
            {
                toCopyTo.AddComponent(t);
            }
        }
        catch
        {
            Debug.LogError("CopyComponentsFailed");
        }
    }









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

    Burnable burnableComponent;

    public void Awake()
    {
        Debug.Log("Awake Called");
        //get and cache requiered components
        MeshRenderer[] childrenMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        Transform[] childrentransform = GetComponentsInChildren<Transform>();
        InitializeWedgeArray(childrentransform);
        //Initialize Variables
        cut = new CutInfo();
        numberOfCuts = 0;
        //Set Layer To Correct Layer
        gameObject.layer = LayerMask.NameToLayer("Food");
        CreateTriggerZones(childrenMeshRenderers);
    }

    public void Cut(RaycastHit hit)
    {
        //transform the point of collision from worldspace to localspace
        Vector3 hitPoint = transform.InverseTransformPoint(hit.point);
        switch (cut.state)
        {
            case CuttingState.NotCutting:
                goto case CuttingState.StartCut;
            case CuttingState.StartCut:
                //get collision info from the raycast
                cut.entryPoint = hitPoint;
                cut.exitPoint = hitPoint;
                cut.cutPointNormal = hit.normal;
                cut.currentCollider = hit.collider;
                cut.minimumCutDistance = GetMinimumCutDistance(cut.currentCollider, hit.normal);
                //start the cut
                cut.state = CuttingState.IsCutting;
                break;
            case CuttingState.IsCutting:
                // detect if the face of the object we are cutting change, or if we exited the current collider and hit another
                if (!Approximate(cut.cutPointNormal, hit.normal, .1f))
                {
                    cut.state = CuttingState.StartCut;
                    break;
                }
                
                cut.exitPoint = hitPoint;
                float distance = Vector3.Distance(cut.entryPoint, cut.exitPoint);
                if (distance >= cut.minimumCutDistance)
                {
                    lastCutPlane = GetColliderEnum(cut.currentCollider);
                    ProcessCut(lastCutPlane);
                    StopCut();
                }
                break;
        }
    }
    public void StopCut()
    {
        cut = new CutInfo();
    }
    private void ProcessCut(ColliderPlane colliderPlane)
    {
        ArrayList leftHalf = new ArrayList();
        ArrayList rightHalf = new ArrayList();
        GameObject leftParent = new GameObject();
        GameObject rightParent = new GameObject();
        leftParent.name = "OnionHalf" + numberOfCuts + 1;
        rightParent.name = "OnionHalf" + numberOfCuts + 2;
        leftParent.transform.position = transform.position;
        rightParent.transform.position = transform.position;
        SeparateHalves(colliderPlane, ref leftHalf, ref rightHalf);
        SetNewParent(leftParent.transform, leftHalf);
        SetNewParent(rightParent.transform, rightHalf);

        gameObject.SetActive(false);

        AddComponentsToParents(leftParent, rightParent);
    }

    private void AddComponentsToParents(GameObject leftParent, GameObject rightParent)
    {
        CopyComponentsToObject(leftParent);
        CopyComponentsToObject(rightParent);

        Toppingable leftTopping = leftParent.AddComponent<Toppingable>();
        Toppingable rightTopping = rightParent.AddComponent<Toppingable>();
        leftTopping.ready = true;
        rightTopping.ready = true;
        leftTopping.ingredientType = Taco.Ingredients.Onion;
        rightTopping.ingredientType = Taco.Ingredients.Onion;

        CuttableIngredient leftCuttable = leftParent.GetComponent<CuttableIngredient>();
        CuttableIngredient rightCuttable = rightParent.GetComponent<CuttableIngredient>();
        if (numberOfCuts < 3)
        {
            leftCuttable.numberOfCuts = numberOfCuts + 1;
            leftCuttable.lastCutPlane = lastCutPlane;
            leftCuttable.Awake();
            rightCuttable.numberOfCuts = numberOfCuts + 1;
            rightCuttable.lastCutPlane = lastCutPlane;
            rightCuttable.Awake();
            leftParent.layer = LayerMask.NameToLayer("Food");
            rightParent.layer = LayerMask.NameToLayer("Food");
        }
        else
        {
            leftCuttable.enabled = false;
            leftCuttable.enabled = false;
        }
    }

    //Function that creates three trigger zones in each axis of the cuttable game object to detect for cut
    private void CreateTriggerZones(MeshRenderer[] meshRendererArray)
    {
        if (lastCutPlane != ColliderPlane.None)
        {
            Destroy(verticalCuttingTriggerZ);
            Destroy(verticalCuttingTriggerX);
            Destroy(horizontalCuttingTrigger);
        }
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
        foreach (MeshRenderer wedgeRenderer in meshRendererArray)
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
    //This function takes a collider and checks if it's equal to the plane colliders, if it is it returns the appropriate Enum
    private ColliderPlane GetColliderEnum(Collider collider)
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
    //Searches for wedges in the object and loads it into a 3D array
    private void InitializeWedgeArray(Transform[] childrentransform)
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
    private void SetNewParent(Transform newParent, Transform objectToParent)
    {
        objectToParent.SetParent(null, true);
        objectToParent.SetParent(newParent, true);
    }
    private void SetNewParent(Transform newParent, ArrayList objectsToParent)
    {
        foreach (GameObject gameObject in objectsToParent)
        {
            SetNewParent(newParent, gameObject.transform);
        }
    }
    private void SeparateHalves(ColliderPlane colliderPlane, ref ArrayList leftHalf, ref ArrayList rightHalf)
    {
        switch (colliderPlane)
        {
            //Divides wedges into two arrays according to what plane was cut (to the left of the plane is 1, to the right is 0)
            case ColliderPlane.XY:
                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        leftHalf.Add(wedges[x, y, 1]);
                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        rightHalf.Add(wedges[x, y, 0]);
                break;

            case ColliderPlane.XZ:
                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int z = 0; z < wedges.GetLength(1); z++)
                        leftHalf.Add(wedges[x, 1, z]);
                for (int x = 0; x < wedges.GetLength(0); x++)
                    for (int z = 0; z < wedges.GetLength(1); z++)
                        rightHalf.Add(wedges[x, 0, z]);
                break;

            case ColliderPlane.YZ:
                for (int z = 0; z < wedges.GetLength(0); z++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        leftHalf.Add(wedges[1, y, z]);
                for (int z = 0; z < wedges.GetLength(0); z++)
                    for (int y = 0; y < wedges.GetLength(1); y++)
                        rightHalf.Add(wedges[0, y, z]);
                break;

            case ColliderPlane.None:
                break;
        }
    }
    //Returns the minimum distance a cutter object should travel to register a cut
    private float GetMinimumCutDistance(Collider collider, Vector3 normal)
    {
        normal.Normalize();
        if (Approximate(normal.x, 1, .1f) || Approximate(normal.x, -1, .1f))
            return collider.Equals(verticalCuttingTriggerX) ? collider.bounds.size.y : collider.bounds.size.z ;
        if (Approximate(normal.z, 1, .1f) || Approximate(normal.z, -1, .1f))
            return collider.Equals(verticalCuttingTriggerZ) ? collider.bounds.size.y : collider.bounds.extents.x;
        if (Approximate(normal.y, 1, .1f) || Approximate(normal.y, -1, .1f))
            return collider.Equals(verticalCuttingTriggerZ) ? collider.bounds.size.z : collider.bounds.size.x;
        return 0;
    }
    //Checks if a number is whithin a certain range of a number
    private bool Approximate(float value, float compare, float range)
    {
        return value >= compare - range && value <= compare + range;
    }

    private bool Approximate(Vector3 value, Vector3 compare, float range)
    {
        return Approximate(value.x, compare.x, range) && Approximate(value.y, compare.y, range) && Approximate(value.z, compare.z, range);
    }

}
