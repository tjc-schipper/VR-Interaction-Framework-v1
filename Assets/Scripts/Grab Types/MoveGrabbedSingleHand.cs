using UnityEngine;
using System.Collections;

public class MoveGrabbedSingleHand : MoveGrabbed {

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        Debug.LogError("Cannot init a single hand MoveGrabbed with two GrabInstances!");
        inited = false;
    }

    public override void Init(GrabInstance _grabInstance)
    {
        base.Init(_grabInstance);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;
    }

    public override void DoMove()
    {
        if (!inited) return;
        //TODO: Simply align position and rotation so that the actionPoint of the grabber aligns with the actionPoint on the grabInstance;
        grabbable.rb.MovePosition(firstGrabInstance.grabber.actionPoint.position);
    }

    void OnDestroy()
    {
        Debug.Log("Destroy called on single");        
        RestoreProperties();
    }
}
