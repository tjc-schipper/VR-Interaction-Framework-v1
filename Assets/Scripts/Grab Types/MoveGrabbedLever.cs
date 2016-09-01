using UnityEngine;
using System.Collections;

public class MoveGrabbedLever : MoveGrabbed {

    
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

    void CreateDebugLineRenderer()
    {
        GameObject g = new GameObject();
        g.transform.parent = transform;
    }

    void OnDestroy()
    {
        RestoreProperties();
    }

    public override void DoMove()
    {
        if (!inited) return;

        // Desiredposition remains the same
        desiredPosition = grabbable.rb.position;

        // Dragvector = the line from the handle pivot to the grabber actionPoint
        Vector3 dragVector = firstGrabInstance.grabber.actionPoint.position - ((GrabbableLever)grabbable).pivot.transform.position;

        // Project the drag vector onto the plane orthogonal to the lever axis
        Vector3 handleVector = grabbable.transform.forward;
        Vector3 cross = Vector3.zero;

        // Get the angle between that projected vector and the current position of the handle
        float angle = handleVector.GetPlaneProjectedAngleTo(dragVector, ((GrabbableLever)grabbable).pivot.transform.right, out cross);
        Quaternion angleRotation = Quaternion.Euler(angle, 0f, 0f);
        desiredRotation = grabbable.rb.rotation * angleRotation;
    }
}
