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
        if (args.interactorObject.transform.CompareTag("Left Hand") && isGrabbedByRat == false)
        {
            attachTransform = leftHandTransform;
            GrabbedByPlayer();
        }
        if (args.interactorObject.transform.CompareTag("Right Hand") && isGrabbedByRat == false)
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

        rb.isKinematic = false;

        if (isCarrier)
        {
            Rat ratComponent = GetComponent<Rat>();
            //ratComponent.rb.isKinematic = false;
            ratComponent.IsGrabbed = true;
        }
        base.OnSelectExited(args);
    }

    //Player Related Methodes
    void GrabbedByPlayer()
    {

        isGrabbedByPlayer = true;

        if (isGrabbedByRat)
        {
            isGrabbedByRat = false;
            DropItem();
        }
        
        if (isCarrier)
        {
            Rat ratComponent = GetComponent<Rat>();
            ratComponent.agent.enabled = false;
            
            isCarrier.DropItem();
        }
    }
    #endregion

    #region Rats
    //Carrier Grab
    private void OnCollisionEnter(Collision collision)
    {
        Carrier carrier = collision.gameObject.GetComponent<Carrier>();

        if (carrier && !carrier.holdingItem && !isGrabbedByRat && !isGrabbedByPlayer && CompareTag("Food"))
        {
            if(carrier.GetComponent<Rat>().agent.enabled == true)
            {
                carrier.holdingItem = true;
                isGrabbedByRat = true;
                rb.isKinematic = true;
                //transform.position = carrier.attachPoint.position; HAVE TO WORK ON ATTACH POINT
                gameObject.transform.SetParent(collision.transform);
                SoundManager.RatLaugh?.Invoke();
            }
        }
    }

    //Carrier Drops Item
    public void DropItem()
    {
        rb.isKinematic = false;
        isGrabbedByRat = false;
        gameObject.transform.parent = null;
    }
    #endregion
}
