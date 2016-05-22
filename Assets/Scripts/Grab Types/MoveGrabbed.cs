using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: Make a system to smoothly animate back to a single-hand grab if coming from a two-hand grab?
/// </summary>

public abstract class MoveGrabbed : MonoBehaviour {

    protected const float EASE_IN_DURATION = 0.25f;   // How long ease-in lasts
    protected const float EASE_IN_THRESHOLD = 0.1f;   // How stretched the grabInstance needs to be for easeIn to kick in
    protected float easeInTimer = 0f;
    
    protected Grabbable grabbable;
    protected GrabInstance firstGrabInstance;
    protected GrabInstance secondGrabInstance;

    #region Pos/Rot interpolation based on weight
    //TODO: This does not work with grabAxis rotation in dualhand grabs! The easing smooths out the small rotations each frame.
    
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
    }
    public virtual void Init(GrabInstance _first, GrabInstance _second)
    {
        firstGrabInstance = _first;
        secondGrabInstance = _second;
        grabbable = firstGrabInstance.grabbable;
        StoreProperties();
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
            if (grabbable.rb.isKinematic)
            {
                if (easeInTimer >= 0f)
                {
                    // Do ease in
                    easeInTimer -= Time.fixedDeltaTime;
                    grabbable.rb.MovePosition(Vector3.Lerp(grabbable.rb.position, desiredPosition, 1f - (easeInTimer / EASE_IN_DURATION)));
                    grabbable.rb.MoveRotation(Quaternion.Lerp(grabbable.rb.rotation, desiredRotation, 1f - (easeInTimer / EASE_IN_DURATION)));
                }
                else
                {
                    grabbable.rb.MovePosition(Vector3.Lerp(grabbable.rb.position, desiredPosition, lerpFactor));
                    grabbable.rb.MoveRotation(Quaternion.Lerp(grabbable.rb.rotation, desiredRotation, lerpFactor));
                }
            }
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
