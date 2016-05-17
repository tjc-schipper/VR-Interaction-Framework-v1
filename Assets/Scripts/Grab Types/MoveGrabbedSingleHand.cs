using UnityEngine;
using System.Collections;

public class MoveGrabbedSingleHand : MoveGrabbed {

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        Debug.LogError("Cannot init a single hand MoveGrabbed with two GrabInstances!");
        inited = false;
    }

    public override void DoMove()
    {
        if (!inited) return;
        //TODO: Simply align position and rotation so that the actionPoint of the grabber aligns with the actionPoint on the grabInstance;
        grabbable.rb.MovePosition(firstGrabInstance.grabber.actionPoint.position);
    }
}
