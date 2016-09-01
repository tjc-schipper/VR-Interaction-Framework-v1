using UnityEngine;
using System.Collections;

public class MoveGrabbedSliderDual : MoveGrabbed {

    
    
    public override void Init(GrabInstance _grabInstance)
    {
        base.Init(_grabInstance);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;

        inited = true;
    }

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        Debug.LogError("Cannot init a MoveGrabbedLever with two GrabInstances!");
        inited = false;
    }

    void OnDestroy()
    {
        RestoreProperties();
    }

    public override void DoMove()
    {
        if (!inited) return;

        // DesiredRotation remains the same
        desiredRotation = grabbable.rb.rotation;

        // Dragvector = the line from the handle pivot to the grabber actionPoint
        Vector3 dragVector = firstGrabInstance.grabber.actionPoint.position - grabbable.rb.position;

        // Project the drag vector onto the plane along slider axis
        Vector3 alongSliderAxis = Vector3.Project(dragVector, ((GrabbableSlider)grabbable).axis);
        desiredPosition = grabbable.rb.position + alongSliderAxis;
    }
}
