using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCenterOfMass : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(UpdateCenterOfMass), 0.5f, 0.5f); // Düzenli olarak güncelle
    }

    void UpdateCenterOfMass()
    {
        if (rb == null) return;

        List<Rigidbody> connectedBodies = GetConnectedRigidbodies(); // Bağlı tüm nesneleri al

        if (connectedBodies.Count == 0) return;

        Vector3 newCenterOfMass = Vector3.zero;
        float totalMass = 0f;

        // Bağlı nesnelerin ağırlık merkezlerini hesapla
        foreach (Rigidbody body in connectedBodies)
        {
            newCenterOfMass += body.worldCenterOfMass * body.mass;
            totalMass += body.mass;
        }

        if (totalMass > 0)
        {
            newCenterOfMass /= totalMass; // Ağırlık merkezini hesapla
            rb.centerOfMass = transform.InverseTransformPoint(newCenterOfMass); // Rigidbody'ye uygula
        }
    }

    List<Rigidbody> GetConnectedRigidbodies()
    {
        List<Rigidbody> connectedBodies = new List<Rigidbody>();
        Queue<Rigidbody> toProcess = new Queue<Rigidbody>();
        HashSet<Rigidbody> processed = new HashSet<Rigidbody>();

        toProcess.Enqueue(rb);

        while (toProcess.Count > 0)
        {
            Rigidbody current = toProcess.Dequeue();
            if (processed.Contains(current)) continue;
            processed.Add(current);
            connectedBodies.Add(current);

            // FixedJoint ile bağlı nesneleri bul
            FixedJoint[] joints = current.GetComponents<FixedJoint>();
            foreach (FixedJoint joint in joints)
            {
                if (joint.connectedBody != null && !processed.Contains(joint.connectedBody))
                {
                    toProcess.Enqueue(joint.connectedBody);
                }
            }
        }

        return connectedBodies;
    }
}
