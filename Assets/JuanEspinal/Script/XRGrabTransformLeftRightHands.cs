using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabTransformLeftRightHands : XRGrabInteractable
{
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    [HideInInspector] public bool isGrabbed;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftHandTransform;
            isGrabbed = true;
        }
        if (args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightHandTransform;
            isGrabbed = true;
        }

        base.OnSelectEntered(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;
        base.OnSelectExited(args);
    }
}
