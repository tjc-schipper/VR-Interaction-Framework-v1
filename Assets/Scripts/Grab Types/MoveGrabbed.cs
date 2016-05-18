using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: Make a system to smoothly animate back to a single-hand grab if coming from a two-hand grab?
/// </summary>

public abstract class MoveGrabbed : MonoBehaviour {

    protected Grabbable grabbable;
    protected GrabInstance firstGrabInstance;
    protected GrabInstance secondGrabInstance;

    #region Pos/Rot interpolation based on weight
    protected Quaternion desiredRotation;
    protected Vector3 desiredPosition;
    protected float handPower = 1f;
    protected float lerpFactor 
    {
        get {
            if (!grabbable.useWeight) return 1f;
            else return Mathf.Clamp01(
                handPower * ((secondGrabInstance == null) ? 1f : 2f)
                / grabbable.rb.mass
                );
        }
    }
    #endregion

    #region Initialization
    protected bool inited = false;

    public virtual void Init(GrabInstance _grabInstance)
    {
        firstGrabInstance = _grabInstance;
        grabbable = firstGrabInstance.grabbable;
        StoreProperties();
        inited = true;
    }
    public virtual void Init(GrabInstance _first, GrabInstance _second)
    {
        firstGrabInstance = _first;
        secondGrabInstance = _second;
        grabbable = firstGrabInstance.grabbable;
        StoreProperties();
        inited = true;
    }
    #endregion

    void OnDestroy()
    {
        RestoreProperties();
    }

    void FixedUpdate()
    {
        if (inited)
        {
            grabbable.rb.MovePosition(Vector3.Lerp(grabbable.rb.position, desiredPosition, lerpFactor));
            grabbable.rb.MoveRotation(Quaternion.Lerp(grabbable.rb.rotation, desiredRotation, lerpFactor));
        }
    }

    #region Store previous properties
    protected bool usedGravity;
    protected bool wasKinematic;

    protected void StoreProperties()
    {
        wasKinematic = grabbable.rb.isKinematic;
        usedGravity = grabbable.rb.useGravity;
    }

    protected void RestoreProperties()
    {
        grabbable.rb.isKinematic = wasKinematic;
        grabbable.rb.useGravity = usedGravity;
    }

    #endregion

    public abstract void DoMove();  // Subclasses MUST implement this! This is what determines the RB behavior based on hand positions
}
