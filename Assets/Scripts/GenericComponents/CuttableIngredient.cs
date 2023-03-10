using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum CuttingState { NotCutting, StartCut, IsCutting, ReachedCenter }
public enum CuttingPlane { None = 0, XY, XZ, YZ }


class CutInfo
{
    //This class stores the information of a cut that is currently being made a cuttable

    public Vector3 entryPoint; //the point where the cut started
    public Vector3 exitPoint; //the last detected point
    public CuttingState state; //the current state of the cut
    public Vector3 cutPointNormal; //the normal of the current collider
    public float minimumCutDistance; //the distance that we should travel in the object to register a cut
    public Collider currentCollider; //the collider that first detected the cut
    public CutInfo()
    {
        entryPoint = Vector3.zero; exitPoint = Vector3.zero; state = CuttingState.NotCutting;
    }

}

[RequireComponent(typeof(Rigidbody))]
public class CuttableIngredient : MonoBehaviour, InterFace_Cutter
{
    Burnable burnableComponent;
    private Timer timer;

    public string ingredientName = "Ingredient";
    public Taco.Ingredients ingredientType;
    public float percentageToCut = 40;
    private float timeBetweenCuts = .5f;
    //used to divide the width of the collider
    public float colliderWidthModifier = 4;

    private Wedge[] wedges; //an array containing the wedges of the cuttable
    Dictionary<CuttingPlane, BoxCollider> triggers; //dictionary that contains the cutting triggers
    private ArrayList cutPlanes;
    [HideInInspector]
    public ArrayList CutPlanes
    {
        get
        {
            return cutPlanes;
        }
        set
        {
            cutPlanes = new ArrayList(value);
        }
    } //an array that stores which planes have been cut


    private int numberOfCuts;
    CutInfo cut;

    private void Awake()
    {
        //get and cache requiered components
        wedges = GetComponentsInChildren<Wedge>();

        //Initialize Variables
        CutPlanes = new ArrayList();
        triggers = new Dictionary<CuttingPlane, BoxCollider>();
        cut = new CutInfo();
        numberOfCuts = 0;
        timer = new Timer();

        //Set Layer To Correct Layer
        gameObject.layer = LayerMask.NameToLayer("Food");
        gameObject.tag = "Food";
    }
    public void Start()
    {
        MeshRenderer[] childrenMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        CreateTriggerZones(childrenMeshRenderers);
    }
    private void Update()
    {
        timer.UpdateTimer();
    }
    public void Cut(RaycastHit hit)
    {
        if (timer < timeBetweenCuts)
            return;
        //transform the point of collision from worldspace to localspace
        Vector3 hitPoint = transform.InverseTransformPoint(hit.point);
        Vector3 normal = transform.InverseTransformVector(hit.normal);
        normal.Normalize();
        switch (cut.state)
        {
            case CuttingState.NotCutting:
                goto case CuttingState.StartCut;

            case CuttingState.StartCut:
                //Check if cutting in correct plane
                CuttingPlane plane = GetColliderEnum(hit.collider);
                if (CheckIncorrectCuttingNormal(normal, plane))
                    return;

                //get collision info from the raycast
                cut.entryPoint = hitPoint;
                cut.exitPoint = hitPoint;
                cut.cutPointNormal = normal;
                cut.currentCollider = hit.collider;
                cut.minimumCutDistance = GetMinimumCutDistance(plane, normal);

                //start the cut
                cut.state = CuttingState.IsCutting;
                break;

            case CuttingState.IsCutting:
                // detect if the face of the object we are cutting change, or if we exited the current collider and hit another
                if (CheckIncorrectCuttingNormal(normal, GetColliderEnum(hit.collider)))
                {
                    cut.state = CuttingState.StartCut;
                    break;
                }

                cut.exitPoint = hitPoint;
                float distance = Vector3.Distance(cut.entryPoint, cut.exitPoint);
                if (distance >= cut.minimumCutDistance)
                {
                    CuttingPlane cutPlane = GetColliderEnum(cut.currentCollider);
                    CutPlanes.Add(cutPlane);
                    ProcessCut(cutPlane);
                    SoundManager.Chopping?.Invoke();
                    Destroy(gameObject);
                }
                break;
        }
    }
    public void StopCut()
    {
        cut = new CutInfo();
    }

    private bool CheckIncorrectCuttingNormal(Vector3 normal, CuttingPlane currentCollider)
    {
        //This function returns true when the collider is being hit in an invalid direction to prevent an incorrect cut
        switch (currentCollider)
        {
            case CuttingPlane.None:
                return false;
            case CuttingPlane.XY:
                return (Approximate(normal.z, 1, .1f) || Approximate(normal.z, -1, .1f));
            case CuttingPlane.XZ:
                return (Approximate(normal.y, 1, .1f) || Approximate(normal.y, -1, .1f));
            case CuttingPlane.YZ:
                return (Approximate(normal.x, 1, .1f) || Approximate(normal.x, -1, .1f));
            default:
                return false;
        }

    }
    private void ProcessCut(CuttingPlane colliderPlane)
    {
        gameObject.SetActive(false);
        ArrayList leftHalf = new();
        ArrayList rightHalf = new();
        GameObject leftParent = new GameObject();
        GameObject rightParent = new GameObject();
        leftParent.name = ingredientName + "left" + numberOfCuts + 1;
        rightParent.name = ingredientName + "right" + numberOfCuts + 1;
        SeparateHalves(colliderPlane, ref leftHalf, ref rightHalf);
        leftParent.transform.position = GetGameObjectMiddlePoint(leftHalf);
        rightParent.transform.position = GetGameObjectMiddlePoint(rightHalf);
        leftParent.transform.rotation = transform.rotation;
        rightParent.transform.rotation = transform.rotation;
        SetNewParent(leftParent.transform, leftHalf);
        SetNewParent(rightParent.transform, rightHalf);
        numberOfCuts++;
        AddComponentsToParents(leftParent, rightParent);
    }
    private void AddComponentsToParents(GameObject leftParent, GameObject rightParent)
    {
        CopyComponentsToObject(leftParent);
        CopyComponentsToObject(rightParent);

        CuttableIngredient leftCuttable = leftParent.GetComponent<CuttableIngredient>();
        CuttableIngredient rightCuttable = rightParent.GetComponent<CuttableIngredient>();

        SetHandPrefabForCutIngredients(ingredientType, numberOfCuts, leftParent, rightParent);

        InheritCuttableVariables(leftCuttable);
        InheritCuttableVariables(rightCuttable);
        leftParent.layer = LayerMask.NameToLayer("Food");
        rightParent.layer = LayerMask.NameToLayer("Food");

        if (numberOfCuts >= 3)
        {
            leftCuttable.enabled = false;
            leftCuttable.enabled = false;
            Toppingable leftTopping = leftParent.AddComponent<Toppingable>();
            Toppingable rightTopping = rightParent.AddComponent<Toppingable>();
            leftTopping.ready = true;
            rightTopping.ready = true;
            leftTopping.ingredientType = ingredientType;
            rightTopping.ingredientType = ingredientType;
        }
    }

    #region Grab Hand Pose
    private void SetHandPrefabForCutIngredients(Taco.Ingredients ingredient, int nbOfCut, GameObject leftParent, GameObject rightParent)
    {
        switch (ingredient)
        {
            //case Taco.Ingredients.Meat:
            //    break;
            case Taco.Ingredients.Onion:
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/RightHandPineapple1Cut"), leftParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/LeftHandPineapple1Cut"), leftParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/RightHandPineapple1Cut"), rightParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/LeftHandPineapple1Cut"), rightParent.transform);
                break;
            case Taco.Ingredients.Pineapple:
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/RightHandPineapple1Cut"), leftParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/LeftHandPineapple1Cut"), leftParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/RightHandPineapple1Cut"), rightParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/LeftHandPineapple1Cut"), rightParent.transform);
                break;
            case Taco.Ingredients.Cheese:
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/RightHandPineapple1Cut"), leftParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/LeftHandPineapple1Cut"), leftParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/RightHandPineapple1Cut"), rightParent.transform);
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHandPose/Pineapple/1 Cut/LeftHandPineapple1Cut"), rightParent.transform);
                break;
        }
    }
    #endregion

    private void CreateTriggerZones(MeshRenderer[] meshRendererArray)
    {
        Quaternion rotation = transform.rotation;
        transform.rotation = Quaternion.identity;
        //Check if planes have been cut
        var planes = System.Enum.GetValues(typeof(CuttingPlane));
        foreach (CuttingPlane colliderPlane in planes)
            if (colliderPlane != CuttingPlane.None)
            {
                BoxCollider boxCollider = null;
                if (!CutPlanes.Contains(colliderPlane))
                    boxCollider = gameObject.AddComponent<BoxCollider>();
                triggers.Add(colliderPlane, boxCollider);
            }

        //creates a box around the item to have the exact length width and depth
        Bounds bounds = new Bounds();
        bounds.center = transform.position;
        foreach (MeshRenderer wedgeRenderer in meshRendererArray)
            bounds.Encapsulate(wedgeRenderer.bounds);

        //Defines each collider size according to the modifier
        if (triggers[CuttingPlane.XZ])
        {
            triggers[CuttingPlane.XZ].size = new Vector3((bounds.size.x) / transform.localScale.x, bounds.size.y / transform.localScale.y / colliderWidthModifier, (bounds.size.z) / transform.localScale.z);
            triggers[CuttingPlane.XZ].isTrigger = true;
        }
        if (triggers[CuttingPlane.XY])
        {
            triggers[CuttingPlane.XY].size = new Vector3((bounds.size.x) / transform.localScale.x, (bounds.size.y / transform.localScale.y), (bounds.size.z / transform.localScale.z) / colliderWidthModifier);
            triggers[CuttingPlane.XY].isTrigger = true;
        }
        if (triggers[CuttingPlane.YZ])
        {
            triggers[CuttingPlane.YZ].size = new Vector3((bounds.extents.x * 2 / transform.localScale.x) / colliderWidthModifier, (bounds.extents.y * 2) / transform.localScale.y, (bounds.extents.z * 2) / transform.localScale.z);
            triggers[CuttingPlane.YZ].isTrigger = true;

        }
        transform.rotation = rotation;
    }
    private CuttingPlane GetColliderEnum(Collider collider)
    {
        //This function takes a collider and checks if it's equal one of cutting triggers, if it is it returns the appropriate Enum
        foreach (var key in triggers.Keys)
            if (triggers[key])
                if (triggers[key].Equals(collider))
                    return key;

        return CuttingPlane.None;
    }
    private void SeparateHalves(CuttingPlane colliderPlane, ref ArrayList leftHalf, ref ArrayList rightHalf)
    {
        //Divides wedges into two arrays, one containing the wedges to the left of the cut axis, the other to the right.
        switch (colliderPlane)
        {
            case CuttingPlane.XY:

                foreach (Wedge wedge in wedges)
                    if (wedge.front)
                        leftHalf.Add(wedge.gameObject);
                    else
                        rightHalf.Add(wedge.gameObject);
                break;

            case CuttingPlane.XZ:
                foreach (Wedge wedge in wedges)
                    if (wedge.top)
                        leftHalf.Add(wedge.gameObject);
                    else
                        rightHalf.Add(wedge.gameObject);
                break;

            case CuttingPlane.YZ:
                foreach (Wedge wedge in wedges)
                    if (wedge.right)
                        rightHalf.Add(wedge.gameObject);
                    else
                        leftHalf.Add(wedge.gameObject);
                break;

            case CuttingPlane.None:
                break;
        }
    }
    private float GetMinimumCutDistance(CuttingPlane plane, Vector3 normal)
    {
        //Returns the minimum distance a cutter object should travel to register a cut


        switch (plane)
        {
            case CuttingPlane.None:
                //if no plane is detected error must be returned
                Debug.Log("Error in (" + nameof(CuttableIngredient) + "." + nameof(GetMinimumCutDistance) + "): No Plane");
                return -1;
            case CuttingPlane.XY:
                if (Approximate(normal.x, Vector3.right.x, .2f) || Approximate(normal.x, Vector3.left.x, .2f))
                    return triggers[plane].size.y * (percentageToCut / 100);
                else if (Approximate(normal.y, Vector3.up.y, .2f) || Approximate(normal.y, Vector3.down.y, .2f))
                    return triggers[plane].size.x * (percentageToCut / 100);
                return 100;
            case CuttingPlane.XZ:
                if (Approximate(normal.x, Vector3.right.x, .2f) || Approximate(normal.x, Vector3.left.x, .2f))
                    return triggers[plane].size.z * (percentageToCut / 100);
                else if (Approximate(normal.z, Vector3.forward.z, .2f) || Approximate(normal.z, Vector3.back.z, .2f))
                    return triggers[plane].size.x * (percentageToCut / 100);
                return 100;
            case CuttingPlane.YZ:
                if (Approximate(normal.z, Vector3.forward.z, .2f) || Approximate(normal.z, Vector3.back.z, .2f))
                    return triggers[plane].size.y * (percentageToCut / 100);
                else if (Approximate(normal.y, Vector3.up.y, .2f) || Approximate(normal.y, Vector3.down.y, .2f))
                    return triggers[plane].size.z * (percentageToCut / 100);
                return 100;
            default:
                break;
        }

        Debug.Log("Error in (" + nameof(CuttableIngredient) + "." + nameof(GetMinimumCutDistance) + "): Invalid RayCast Normal");
        return -1;
    }
    public void CopyComponentsToObject(GameObject toCopyTo)
    {
        //uses reflection to find all components in the current object and copies them to the target object
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
    private Vector3 GetGameObjectMiddlePoint(ArrayList array)
    {
        //returns the worldspace middle point of an array of objects
        Vector3 centerPosition = new();
        int numberOfWedges = 0;
        foreach (GameObject gameObject in array)
            if (gameObject)
            {
                centerPosition += gameObject.transform.position;
                numberOfWedges++;
            }
        return centerPosition / numberOfWedges;
    }
    private bool Approximate(float value, float compare, float range)
    {
        //Checks if a number is whithin a certain range of a number
        return value >= compare - range && value <= compare + range;
    }
    private bool Approximate(Vector3 value, Vector3 compare, float range)
    {
        return Approximate(value.x, compare.x, range) && Approximate(value.y, compare.y, range) && Approximate(value.z, compare.z, range);
    }
    private void AddNonNullToArray(ArrayList leftHalf, GameObject wedge)
    {
        if (wedge)
            leftHalf.Add(wedge);
    }
    private void SetNewParent(Transform newParent, Transform objectToParent)
    {
        objectToParent.SetParent(null, true);
        objectToParent.SetParent(newParent, true);
    }
    private void SetNewParent(Transform newParent, ArrayList objectsToParent)
    {
        foreach (GameObject gameObject in objectsToParent)
            if (gameObject)
                SetNewParent(newParent, gameObject.transform);
    }
    private void InheritCuttableVariables(CuttableIngredient cuttable)
    {
        cuttable.numberOfCuts = numberOfCuts;
        cuttable.CutPlanes = CutPlanes;
        cuttable.ingredientType = ingredientType;
        cuttable.colliderWidthModifier = colliderWidthModifier;
        cuttable.ingredientName = ingredientName;
        if (numberOfCuts == 2)
            cuttable.percentageToCut = percentageToCut / 2;
    }
}
