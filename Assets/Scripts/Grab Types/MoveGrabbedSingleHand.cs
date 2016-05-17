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

        grabbable.rb.MoveRotation(firstGrabInstance.grabber.actionPoint.rotation * firstGrabInstance.grabRotationOffset);
        grabbable.rb.MovePosition(firstGrabInstance.grabber.actionPoint.position - firstGrabInstance.grabOffset);

        //TODO: What happens when two controllers grab it?
        //TODO: Set desired position/rotation as above, lerp the rb there (or use addvelocity) for feeling of weight
    }

    void OnDestroy()
    {
        RestoreProperties();
    }
}
