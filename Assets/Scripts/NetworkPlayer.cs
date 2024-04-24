using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Xml.Serialization;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Renderer[] meshToDisable;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                item.enabled = false;
            }
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            root.position = VRRigReferenses.instance.root.position;
            root.rotation = VRRigReferenses.instance.root.rotation;

            head.position = VRRigReferenses.instance.head.position;
            head.rotation = VRRigReferenses.instance.head.rotation;

            rightHand.position = VRRigReferenses.instance.rightHand.position;
            rightHand.rotation = VRRigReferenses.instance.rightHand.rotation;

            leftHand.position = VRRigReferenses.instance.leftHand.position;
            leftHand.rotation = VRRigReferenses.instance.leftHand.rotation;
        }
    }
}
