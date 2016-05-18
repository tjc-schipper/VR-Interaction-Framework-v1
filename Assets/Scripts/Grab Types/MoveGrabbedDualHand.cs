using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: Allow for rotation along the 'grab-axis' (actionPoint to actionPoint) using the average rotation of the controllers.
/// Need a way to transform controller rotation to rotation along this axis. 
/// Store initial grab rotations (similar to singlehand)?
/// Do this frame-per-frame, angle based?
/// rb.rotation = rb.rotation * Quaternion.AngleAxis(angle, firstG.actionPoint.position - secondG.actionPoint.position)?
/// </summary>
public class MoveGrabbedDualHand : MoveGrabbed {

    Vector3 actionPointAlignment
    {
        get
        {
            return firstGrabInstance.grabOffset - secondGrabInstance.grabOffset;
        }
    }
    Vector3 grabberAlignment
    {
        get
        {
            return firstGrabInstance.grabber.actionPoint.position - secondGrabInstance.grabber.actionPoint.position;
        }
    }
    
    public override void Init(GrabInstance _grabInstance)
    {
        Debug.LogError("Cannot init dual hand MoveGrabbed with only 1 grabInstance!");
        inited = false;
    }

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        base.Init(_first, _second);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;
        Debug.Log("DualHand inited!");
    }
    
    public override void DoMove()
    {
        if (!inited) return;        
        // Do the |a1-a2| === |h1-h2| rotation and center positioning

        desiredRotation = Quaternion.FromToRotation(actionPointAlignment, grabberAlignment) * grabbable.rb.rotation;    // Align grab axis with hand axis
        // TODO: Do rotation along grab-axis here!

        desiredPosition = Vector3.Lerp(
            firstGrabInstance.grabber.actionPoint.position - firstGrabInstance.grabOffset, 
            secondGrabInstance.grabber.actionPoint.position - secondGrabInstance.grabOffset, 
            0.5f);  // Position rb in between hands, taking grabOffsets into account. Lerp 0.5 between two *SingleHands, basically
    }
}
