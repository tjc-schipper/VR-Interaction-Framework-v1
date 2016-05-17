using UnityEngine;
using System.Collections;

public class MoveGrabbedDualHand : MoveGrabbed {

    public override void Init(GrabInstance _grabInstance)
    {
        Debug.LogError("Cannot init dual hand MoveGrabbed with only 1 grabInstance!");
        inited = false;
    }
    
    public override void DoMove()
    {
        if (!inited) return;        
        // Do the |a1-a2| === |h1-h2| rotation and center positioning
    }
}
