using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pickupable : XRGrabInteractable
{
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    Rigidbody rb;
    Carrier isCarrier;
    [HideInInspector] public bool isGrabbedByPlayer;
    [HideInInspector] public bool isGrabbedByRat;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (GetComponent<Carrier>())
            isCarrier = GetComponent<Carrier>();
    }

    #region Player
    //Player Grabs item
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftHandTransform;
            GrabbedByPlayer();
        }
        if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightHandTransform;
            GrabbedByPlayer();
        }

        base.OnSelectEntered(args);
    }
    // Player drops item
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        isGrabbedByPlayer = false;
        base.OnSelectExited(args);
    }
    //Player Related Methodes
    void GrabbedByPlayer()
    {
        isGrabbedByPlayer = true;
        if (isCarrier)
        {
            isCarrier.DropItem();
        }
    }
    #endregion

    #region Rats
    //Carrier Grab
    private void OnCollisionEnter(Collision collision)
    {
        Carrier carrier = collision.gameObject.GetComponent<Carrier>();
        if (carrier && !carrier.holdingItem && !isGrabbedByRat && !isGrabbedByPlayer)
        {
            carrier.holdingItem = true;
            isGrabbedByRat = true;
            rb.isKinematic = true;
            //transform.position = carrier.attachPoint.position; HAVE TO WORK ON ATTACH POINT
            gameObject.transform.SetParent(collision.transform);
        }

    }
    //Carrier Drops Item
    public void DropItem()
    {
        gameObject.transform.SetParent(null);
        rb.isKinematic = false;
        isGrabbedByRat = false;
    }
    #endregion
}