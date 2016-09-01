using UnityEngine;
using System.Collections;

public class MoveGrabbedLever_Dual : MoveGrabbed
{

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        base.Init(_first, _second);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;

        Debug.Log("Two handed lever created!");
        inited = true;
    }

    public override void Init(GrabInstance _grabInstance)
    {
        Debug.LogError("Cannot init dual hand grab with one grabInstance");
        inited = false;
    }

    void OnDestroy()
    {
        RestoreProperties();
    }

    public override void DoMove()
    {
        if (!inited) return;
        
        // DesiredPosition remains the same
        desiredPosition = grabbable.rb.position;
        
        // Average the two controller inputs to get the drag vector
        Vector3 dragVector = Vector3.Lerp(
            firstGrabInstance.grabber.actionPoint.position,
            secondGrabInstance.grabber.actionPoint.position,
            0.5f
            ) - ((GrabbableLever)grabbable).pivot.transform.position;

        // Project the drag vector onto the plane orthogonal to the lever axis
        Vector3 handleVector = grabbable.transform.forward;
        Vector3 cross = Vector3.zero;

        // Get the angle between that projected vector and the current position of the handle
        float angle = handleVector.GetPlaneProjectedAngleTo(dragVector, ((GrabbableLever)grabbable).pivot.transform.right, out cross);
        Quaternion angleRotation = Quaternion.Euler(angle, 0f, 0f);
        desiredRotation = grabbable.rb.rotation * angleRotation;
    }

}
