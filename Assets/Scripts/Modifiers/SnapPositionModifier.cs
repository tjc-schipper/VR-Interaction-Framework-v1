using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapPositionModifier : MovementModifier {

    public Transform snapLocation;
    public Material previewMaterial;
    float maxSnapDistance = 0.25f;

    public Transform objectPreviewRoot;
    GameObject objectPreview;

    public override void DoModify(List<GrabInstance> grabInstances, MoveGrabbed move)
    {
        float desiredDistance = (move.desiredPosition - snapLocation.position).magnitude;
        if (desiredDistance >= maxSnapDistance)
        {
            grabInstances[0].grabbable.RemoveModifier(this);
            objectPreview.SetActive(true);
        }
        else
        {
            objectPreview.SetActive(false);
            move.desiredPosition = snapLocation.position;
            move.desiredRotation = snapLocation.rotation;
        }
        
    }

    public void SetObjectPreview(GameObject grabbable)
    {
        if (objectPreview != null)
        {
            DestroyImmediate(objectPreview);
        }

        if (grabbable != null)
        {
            objectPreview = new GameObject();
            objectPreview.transform.parent = objectPreviewRoot;
            objectPreview.transform.localPosition = Vector3.zero;
            objectPreview.transform.localRotation = Quaternion.identity;

            MeshRenderer r = objectPreview.AddComponent<MeshRenderer>();
            MeshFilter f = objectPreview.AddComponent<MeshFilter>();
            f.mesh = grabbable.GetComponentInChildren<MeshFilter>().mesh;
            r.material = previewMaterial;
        }
    }

}
