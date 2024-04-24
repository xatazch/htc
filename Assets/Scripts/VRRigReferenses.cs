using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigReferenses : MonoBehaviour
{
    public static VRRigReferenses instance;

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
