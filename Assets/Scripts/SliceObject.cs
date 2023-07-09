using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public LayerMask sliceableLayer;
    public VelocityEstimator velocityEstimator;

    public Transform planeDebug;
    public GameObject target;
    public Material crossMaterial;
    public float cutForce = 200;

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
        if (hull != null)
        {
            GameObject lowerHull = hull.CreateLowerHull(target, crossMaterial);
            SetupSlicedComponent(lowerHull);
            GameObject upperHull = hull.CreateUpperHull(target, crossMaterial);
            SetupSlicedComponent(upperHull);

            // Destruir los objetos después de 2 segundos
            Destroy(lowerHull, 2f);
            Destroy(upperHull, 2f);

            
            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}
