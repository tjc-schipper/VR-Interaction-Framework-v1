using UnityEngine;
using System.Collections;

public class MoveGrabbedSlider : MoveGrabbed {

    GrabbableSlider gr;

    
    public override void Init(GrabInstance _grabInstance)
    {
        base.Init(_grabInstance);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;

        gr = (GrabbableSlider)grabbable;
        
        inited = true;
    }

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        Debug.LogError("Cannot init a MoveGrabbedSlider with two GrabInstances!");
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

        // Project the drag vector onto the slider axis
        //float dragDistanceAlongAxis = Vector3.Dot(dragVector, gr.axis);
        float dragDistanceAlongAxis = Vector3.Dot(gr.axis, dragVector);
        Vector3 tempDesiredPosition = grabbable.rb.position + dragDistanceAlongAxis * gr.axis;

        float sliderDisplacement = gr.GetDisplacement(tempDesiredPosition);
        
        
        // Correct tempDesiredPosition if out of bounds
        if (Mathf.Abs(sliderDisplacement) <= gr.maxDisplacement)
        {
            desiredPosition = tempDesiredPosition;
        }
        else
        {
            desiredPosition = gr.rb.position;
        }
        

    }
}
