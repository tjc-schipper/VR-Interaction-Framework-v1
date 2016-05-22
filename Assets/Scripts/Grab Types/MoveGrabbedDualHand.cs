using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: Allow for rotation along the 'grab-axis' (actionPoint to actionPoint) using the average rotation of the controllers.
/// Need a way to transform controller rotation to rotation along this axis. 
/// Store initial grab rotations (similar to singlehand)?
/// Do this frame-per-frame, angle based?
/// rb.rotation = rb.rotation * Quaternion.AngleAxis(angle, firstG.actionPoint.position - secondG.actionPoint.position)?
/// </summary>
public class MoveGrabbedDualHand : MoveGrabbed
{

    Vector3 DEBUG_crossProduct;

    Vector3 actionPointAlignment
    {
        get
        {
            return firstGrabInstance.grabOffset - secondGrabInstance.grabOffset;
        }
    }
    Vector3 handsAxis
    {
        get
        {
            return firstGrabInstance.grabber.actionPoint.position - secondGrabInstance.grabber.actionPoint.position;
        }
    }

    Vector3 previousControllerOrientation;
    Vector3 controllerOrientation
    {
        get
        {
            Vector3 c1o = Vector3.ProjectOnPlane(firstGrabInstance.grabber.actionPoint.forward, handsAxis).normalized;
            Vector3 c2o = Vector3.ProjectOnPlane(secondGrabInstance.grabber.actionPoint.forward, handsAxis).normalized;
            return Vector3.Lerp(c1o, c2o, 0.5f);
        }
    }
    public override void Init(GrabInstance _grabInstance)
    {
        Debug.LogError("Cannot init dual hand MoveGrabbed with only 1 grabInstance!");
        inited = false;
    }

    public override void Init(GrabInstance _first, GrabInstance _second)
    {
        base.Init(_first, _second);
        grabbable.rb.useGravity = false;
        grabbable.rb.isKinematic = true;

        // Initialize controller orientation so the update loop doesn't screw up ([0,0,0] previousOrientation).
        previousControllerOrientation = controllerOrientation;
        inited = true;
    }

    public override void DoMove()
    {
        if (!inited) return;

        Vector3 currentControllerOrientation = controllerOrientation;

        // Rotate object so that the 'grab axis' aligns with the 'handAxis'.
        desiredRotation = Quaternion.FromToRotation(actionPointAlignment, handsAxis) * grabbable.rb.rotation;

        float angle = previousControllerOrientation.GetPlaneProjectedAngleTo(currentControllerOrientation, handsAxis, out DEBUG_crossProduct);

        // Rotate the grabbed object around the grabaxis by that many degrees to match what the hands were doing
        desiredRotation = Quaternion.AngleAxis(angle, handsAxis) * desiredRotation;

        // Position rb in between hands, taking grabOffsets into account. Lerp 0.5 between two *SingleHands, basically
        desiredPosition = Vector3.Lerp(
            firstGrabInstance.grabber.actionPoint.position - firstGrabInstance.grabOffset,
            secondGrabInstance.grabber.actionPoint.position - secondGrabInstance.grabOffset,
            0.5f);

        previousControllerOrientation = currentControllerOrientation;
    }

    void OnDrawGizmos()
    {
        if (Debug.isDebugBuild)
        {
            Gizmos.DrawLine(
            firstGrabInstance.grabber.actionPoint.position,
            firstGrabInstance.grabber.actionPoint.position + controllerOrientation
            );
            Gizmos.DrawLine(
                firstGrabInstance.grabber.actionPoint.position,
                firstGrabInstance.grabber.actionPoint.position + DEBUG_crossProduct
                );
            Gizmos.DrawWireSphere(
                firstGrabInstance.grabber.actionPoint.position + DEBUG_crossProduct,
                0.05f
                );
        }
    }
}

public static partial class RotationHelpers
{
    public static float GetAngleAroundAxis(this Quaternion rotation, Vector3 axis)
    {
        Debug.LogWarning("Has not been tested to be correct! http://stackoverflow.com/questions/3684269/component-of-a-quaternion-rotation-around-an-axis");

        /* Swing-twist decomposition. Pure magic, don't touch.
         * See here: http://stackoverflow.com/questions/3684269/component-of-a-quaternion-rotation-around-an-axis
         */

        Vector3 components = rotation.eulerAngles;
        Vector3 projected = Vector3.Project(components, axis);
        Quaternion twist = new Quaternion(projected.x, projected.y, projected.z, rotation.w);

        Vector3 newAxis = Vector3.zero;
        float angle = 0f;
        twist.ToAngleAxis(out angle, out newAxis);

        if (!axis.Equals(newAxis))
        {
            //Debug.LogError("Invalid quaternion stuff!");
        }
        return angle;
    }

    public static float GetPlaneProjectedAngleTo(this Vector3 oldDir, Vector3 newDir, Vector3 planeNormal, out Vector3 cross)
    {
        // Project original two vectors onto a plane defined by planeNormal
        Vector3 projectedOld = Vector3.ProjectOnPlane(oldDir, planeNormal).normalized;
        Vector3 projectedNew = Vector3.ProjectOnPlane(newDir, planeNormal).normalized;
        float angle = Vector3.Angle(projectedOld, projectedNew);
        
        // Get orthogonal to pOld and pNew, indicating angle direction using right-hand-rule.
        cross = Vector3.Cross(projectedOld, projectedNew).normalized;
        
        // Reverse angle if crossproduct faces opposite planeNormal
        float crossDirection = Vector3.Dot(cross, planeNormal);
        if (crossDirection < 0) angle *= -1;
        return angle;
    }
}
