using UnityEngine;
using System.Collections;

public class MoveGrabbedSlider : MoveGrabbed {

    GrabbableSlider gr;

    Material knobMat;
    bool _oob = false;
    bool oob
    {
        get
        {
            return _oob;
        }
        set
        {
            if (value != _oob)
            {
                knobMat.color = (value) ? Color.red : Color.green;
                _oob = value;
            }
        }
    }

    public override void Init(GrabInstance _grabInstance)
    {
        base.Init(_grabInstance);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;

        gr = (GrabbableSlider)grabbable;
        knobMat = grabbable.GetComponent<MeshRenderer>().material;

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

        // Calc how far the desiredPosition is removed from the center of the slider
        Vector3 a = tempDesiredPosition - gr.sliderBase.transform.position;
        //float sliderDisplacement = Vector3.Dot(a, gr.axis);
        float sliderDisplacement = Vector3.Dot(gr.axis, a);

        
        // Correct tempDesiredPosition if out of bounds
        if (Mathf.Abs(sliderDisplacement) > gr.maxDisplacement)
        {
            // Out of bounds!
            oob = true;
            //tempDesiredPosition = grabbable.rb.position + (gr.axis * gr.maxDisplacement * Mathf.Sign(sliderDisplacement));
        }
        else
        {
            oob = false;
        }

        desiredPosition = tempDesiredPosition;
    }
}
