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
        inited = true;
    }
    public virtual void Init(GrabInstance _first, GrabInstance _second)
    {
        firstGrabInstance = _first;
        secondGrabInstance = _second;
        grabbable = firstGrabInstance.grabbable;
        inited = true;
    }

    public abstract void DoMove();
}
