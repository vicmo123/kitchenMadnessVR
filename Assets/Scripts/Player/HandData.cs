using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData : MonoBehaviour
{
    public enum HandLeftRright { LEFT, RIGHT };
    public HandLeftRright handLR;
    public Transform hand;
    public Transform[] fingersJoints;
    public Animator handAnimator;
}
