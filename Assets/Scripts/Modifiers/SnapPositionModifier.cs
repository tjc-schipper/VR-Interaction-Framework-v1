using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapPositionModifier : MovementModifier {

    public Transform snapLocation;
    float maxSnapDistance = 0.25f;

    public override void DoModify(List<GrabInstance> grabInstances, MoveGrabbed move)
    {
        float desiredDistance = (move.desiredPosition - snapLocation.position).magnitude;
        if (desiredDistance >= maxSnapDistance)
        {
            grabInstances[0].grabbable.RemoveModifier(this);
            return;
        }
        
        move.desiredPosition = snapLocation.position;
        move.desiredRotation = snapLocation.rotation;
    }

}
