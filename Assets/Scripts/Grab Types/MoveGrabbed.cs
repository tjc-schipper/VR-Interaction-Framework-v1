using UnityEngine;
using System.Collections;

public abstract class MoveGrabbed : MonoBehaviour {

    protected Grabbable grabbable;
    protected GrabInstance firstGrabInstance;
    protected GrabInstance secondGrabInstance;

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

    void OnDestroy()
    {
        RestoreProperties();
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

    public abstract void DoMove();
}
