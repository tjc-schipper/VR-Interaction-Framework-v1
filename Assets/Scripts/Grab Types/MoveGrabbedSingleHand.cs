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

        // Ease in setup (for when coming back from two-handed grabs)
        if (firstGrabInstance.stretchDistance >= MoveGrabbed.EASE_IN_THRESHOLD)
        {
            easeInTimer = MoveGrabbed.EASE_IN_DURATION;
        }

        inited = true;
    }

    void OnDestroy()
    {
        // Overriding base.OnDestroy()!
        if (easeInTimer > 0f)
        {
            firstGrabInstance.grabbable.rb.velocity = Vector3.zero;
        }
        RestoreProperties();
    }

    public override void DoMove()
    {
        if (!inited) return;

        desiredPosition = firstGrabInstance.grabber.actionPoint.position - firstGrabInstance.grabOffset;
        desiredRotation = firstGrabInstance.grabber.actionPoint.rotation * firstGrabInstance.grabRotationOffset;
    }
}
