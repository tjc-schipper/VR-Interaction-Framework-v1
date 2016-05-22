using UnityEngine;
using System.Collections;

public class GrabZone : MonoBehaviour {

    public Grabbable grabbable;
    
    public bool isToolGrab;
    [SerializeField]
    Transform alignmentPoint;
    public Transform ActionPoint
    {
        get
        {
            if (alignmentPoint != null) return alignmentPoint;
            else return transform;
        }
    }

    void Awake()
    {
        // Search upwards through hierarchy for the closest grabbable, starting with this.gameObject
        GameObject go = gameObject;
        do
        {
            grabbable = go.GetComponent<Grabbable>();
        } while (grabbable == null && (go = go.transform.parent.gameObject));

        if (grabbable == null)
        {
            Debug.LogError("No grabbable found for GrabZone: " + this.gameObject.name);
        }
    }
}
