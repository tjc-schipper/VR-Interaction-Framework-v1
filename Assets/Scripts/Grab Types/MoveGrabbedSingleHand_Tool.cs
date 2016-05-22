using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: Make this type of grab disconnect when the grab button is pressed a second time.
/// </summary>

public class MoveGrabbedSingleHand_Tool : MoveGrabbedSingleHand {

    bool rotationInitialized = false;
    Quaternion _toolRotation;
    Quaternion toolRotation
    {
        get
        {
            if (!rotationInitialized)
            {
                _toolRotation = Quaternion.Inverse(firstGrabInstance.grabbable.rb.rotation) * firstGrabInstance.grabZone.ActionPoint.rotation;
                rotationInitialized = true;
            }
            return _toolRotation;
        }
    }

    public override void DoMove()
    {
        if (!inited) return;

        desiredPosition = firstGrabInstance.grabber.actionPoint.position - (firstGrabInstance.grabZone.ActionPoint.position - firstGrabInstance.grabbable.rb.position);
        desiredRotation = firstGrabInstance.grabber.actionPoint.rotation * toolRotation;
    }

    
}
