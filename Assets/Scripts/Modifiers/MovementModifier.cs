using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MovementModifier : MonoBehaviour {

    List<GameObject> subscribedGameObjects;

    //bool oldWasKinematic;
    //bool oldUseGravity;

    void Start()
    {
        subscribedGameObjects = new List<GameObject>();
    }

    public abstract void DoModify(List<GrabInstance> grabInstances, MoveGrabbed move);

    void OnTriggerEnter(Collider collider)
    {
        Grabbable grabbable = collider.gameObject.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            GameObject go = grabbable.AddModifier(this);
            subscribedGameObjects.Add(go);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (subscribedGameObjects.Contains(collider.gameObject))
        {
            Grabbable grabbable = collider.gameObject.GetComponent<Grabbable>();
            if (grabbable != null) {
                grabbable.RemoveModifier(this);
                subscribedGameObjects.Remove(collider.gameObject);
            }
        }
    }
}
