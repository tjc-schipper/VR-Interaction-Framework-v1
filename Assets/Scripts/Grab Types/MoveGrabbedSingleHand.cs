using UnityEngine;
using System.Collections;

public class MoveGrabbedSingleHand : MoveGrabbed
{

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        Debug.LogError("Cannot init a single hand MoveGrabbed with two GrabInstances!");
        inited = false;
    }

    public override void Init(GrabInstance _grabInstance)
    {
        base.Init(_grabInstance);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;
    }

    public override void DoMove()
    {
        if (!inited) return;

        desiredPosition = firstGrabInstance.grabber.actionPoint.position - firstGrabInstance.grabOffset;
        desiredRotation = firstGrabInstance.grabber.actionPoint.rotation * firstGrabInstance.grabRotationOffset;
    }
}
