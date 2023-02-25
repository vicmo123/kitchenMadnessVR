using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabHandPose : MonoBehaviour
{
    public HandData rightHand;
    public HandData leftHand;
    public float poseTransitionDuration;

    Vector3 startingHandPosition;
    Vector3 finalHandPosition;
    Quaternion startingHandRotation;
    Quaternion finalHandRotation;

    Quaternion[] startingFingerRotation;
    Quaternion[] finalFingerRotation;

    // Start is called before the first frame update
    void Start()
    {
        if (!rightHand || !leftHand)
        {
            foreach (Transform g in gameObject.transform)
            {
                if (g.CompareTag("Right Hand"))
                    rightHand = g.GetComponentInChildren<HandData>();

                if (g.CompareTag("Left Hand"))
                    leftHand = g.GetComponentInChildren<HandData>();
            }
        }

        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        poseTransitionDuration = 0.2f;
        grabInteractable.selectEntered.AddListener(SetPose);
        grabInteractable.selectExited.AddListener(UnsetPose);

        rightHand.gameObject.SetActive(false);
        leftHand.gameObject.SetActive(false);
    }

    public void SetPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.handAnimator.enabled = false;

            if (handData.handLR == HandData.HandLeftRright.RIGHT)
            {
                SetDataValue(handData, rightHand);
            }
            else
            {
                SetDataValue(handData, leftHand);
            }

            StartCoroutine(SetHandDataRoutine(handData, finalHandPosition, finalHandRotation, finalFingerRotation, startingHandPosition, startingHandRotation, startingFingerRotation));
        }
    }

    public void UnsetPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.handAnimator.enabled = true;

            if (handData.handLR == HandData.HandLeftRright.RIGHT)
            {
                SetDataValue(handData, rightHand);
            }
            else
            {
                SetDataValue(handData, leftHand);
            }
            //SetDataValue(handData, rightHand);
            StartCoroutine(SetHandDataRoutine(handData, startingHandPosition, startingHandRotation, startingFingerRotation, finalHandPosition, finalHandRotation, finalFingerRotation));
        }
    }

    public void SetDataValue(HandData startData, HandData endData)
    {
        startingHandPosition = startData.hand.localPosition;
        finalHandPosition = endData.hand.localPosition;

        startingHandRotation = startData.hand.localRotation;
        finalHandRotation = endData.hand.localRotation;

        startingFingerRotation = new Quaternion[startData.fingersJoints.Length];
        finalFingerRotation = new Quaternion[endData.fingersJoints.Length];

        for (int i = 0; i < startData.fingersJoints.Length; i++)
        {
            startingFingerRotation[i] = startData.fingersJoints[i].localRotation;
            finalFingerRotation[i] = endData.fingersJoints[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPos, Quaternion newRotation, Quaternion[] newJointsRotation)
    {
        h.hand.localPosition = newPos;
        h.hand.localRotation = newRotation;

        for (int i = 0; i < newJointsRotation.Length; i++)
        {
            h.fingersJoints[i].localRotation = newJointsRotation[i];
        }
    }

    public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPos, Quaternion newRotation, Quaternion[] newJointsRotation, Vector3 startingPos, Quaternion startingRotation, Quaternion[] startingJointsRotaion)
    {
        float timer = 0;

        while (timer < poseTransitionDuration)
        {
            Vector3 pos = Vector3.Lerp(startingPos, newPos, timer / poseTransitionDuration);
            Quaternion rotation = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            h.hand.localPosition = pos;
            h.hand.localRotation = rotation;

            for (int i = 0; i < newJointsRotation.Length; i++)
            {
                h.fingersJoints[i].localRotation = Quaternion.Lerp(startingJointsRotaion[i], newJointsRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Mirror Selected Right Grab Pose")]
    public static void MirrorRightPose()
    {
        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        handPose.MirrorPose(handPose.leftHand, handPose.rightHand);
    }
#endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        Vector3 mirroredPos = poseUsedToMirror.hand.localPosition;
        mirroredPos.x *= -1;

        Quaternion mirroredRotation = poseUsedToMirror.hand.localRotation;
        mirroredRotation.y *= -1;
        mirroredRotation.z *= -1;

        poseToMirror.hand.localPosition = mirroredPos;
        poseToMirror.hand.localRotation = mirroredRotation;

        for (int i = 0; i < poseUsedToMirror.fingersJoints.Length; i++)
        {
            poseToMirror.fingersJoints[i].localRotation = poseUsedToMirror.fingersJoints[i].localRotation;
        }
    }
}
