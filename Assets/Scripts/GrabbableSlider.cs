using UnityEngine;
using System.Collections;

public class GrabbableSlider : Grabbable {

    public float maxDisplacement = 0.19f;
    public GameObject sliderBase;

    public float GetDisplacement(Vector3 position)
    {
        Vector3 offset = position - sliderBase.transform.position;
        return Vector3.Dot(offset, axis);
    }

    public Vector3 axis
    {
        get
        {
            return transform.right;
        }
    }
    
    protected override void CreateGrabHandler(GrabInstance grabInstance)
    {
        if (grabInstances.Count == 0)
        {
            DestroyImmediate(moveGrabbed);
            moveGrabbed = null;
        }
        else if (grabInstances.Count == 1)
        {
            // Do lever grab
            DestroyImmediate(moveGrabbed);

            moveGrabbed = gameObject.AddComponent<MoveGrabbedSlider>();
            moveGrabbed.Init(grabInstances[0]);
        }
        else
        {
            if (allowMultipleGrabs)
            {
                // Do dual hand grab
                DestroyImmediate(moveGrabbed);
                moveGrabbed = gameObject.AddComponent<MoveGrabbedSliderDual>();
                moveGrabbed.Init(grabInstances[0], grabInstances[1]);
            }
        }
    }
}
