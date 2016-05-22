using UnityEngine;
using System.Collections;

public class MoveGrabbedDual_Forces : MoveGrabbed {

    readonly float forceFactor = 10f;
    Vector3 firstForce;
    Vector3 secondForce;

    public override void Init(GrabInstance _grabInstance)
    {
        Debug.LogError("Cannot init two-hand force grab with only one grab instance!");
        inited = false;
    }

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        base.Init(_first, _second);
        grabbable.rb.useGravity = true;
        grabbable.rb.isKinematic = false;
        inited = true;
    }
    
    public override void DoMove()
    {
        // Calculate forces from attachPoints to hands.
        firstForce = firstGrabInstance.stretch * forceFactor * grabbable.rb.mass;
        secondForce = secondGrabInstance.stretch * forceFactor * grabbable.rb.mass;

        grabbable.rb.AddForceAtPosition(firstForce, grabbable.rb.position - firstGrabInstance.grabOffset);
        grabbable.rb.AddForceAtPosition(secondForce, grabbable.rb.position - secondGrabInstance.grabOffset);

        // TODO: Rotation similar to *DualHand, but using AddForce around an axis instead.
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            (grabbable.rb.position - firstGrabInstance.grabOffset),
            (grabbable.rb.position - firstGrabInstance.grabOffset) + firstForce
            );
        Gizmos.DrawLine(
            (grabbable.rb.position - secondGrabInstance.grabOffset),
            (grabbable.rb.position - secondGrabInstance.grabOffset) + secondForce
            );
        Gizmos.DrawLine(grabbable.rb.position, grabbable.rb.position + (grabbable.rb.mass * Physics.gravity));
    }
}
