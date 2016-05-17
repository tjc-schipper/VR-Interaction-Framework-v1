using UnityEngine;
using System.Collections;

public class GrabZone : MonoBehaviour {

    public Grabbable grabbable;

    void Awake()
    {
        // Search upwards through hierarchy for the closest grabbable, starting with this.gameObject
        GameObject go = gameObject;
        do
        {
            grabbable = gameObject.GetComponent<Grabbable>();
        } while (grabbable == null && (go = go.transform.parent.gameObject));

        if (grabbable == null)
        {
            Debug.LogError("No grabbable found for GrabZone: " + this.gameObject.name);
        }
    }
}
