using UnityEngine;
using System.Collections;

public class GrabbableLever : Grabbable {

    public GrabbablePivot pivot;
    //public float angleTravel = 30f;

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

            moveGrabbed = gameObject.AddComponent<MoveGrabbedLever>();
            moveGrabbed.Init(grabInstances[0]);
        }
        else
        {
            if (allowMultipleGrabs)
            {
                // Do dual hand grab
                DestroyImmediate(moveGrabbed);
                moveGrabbed = gameObject.AddComponent<MoveGrabbedLever_Dual>();
                moveGrabbed.Init(grabInstances[0], grabInstances[1]);
            }
        }
    }
}
