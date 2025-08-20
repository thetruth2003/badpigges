using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    public GameObject connectedCube = null; // Bu attach point ile bağlanan cube

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

            if (otherRigidbody != null)
            {
                Rigidbody parentRigidbody = transform.parent.GetComponent<Rigidbody>();

                if (parentRigidbody != null)
                {
                    Cube parentCubeScript = transform.parent.GetComponentInParent<Cube>();
                    Cube otherCubeScript = other.GetComponentInParent<Cube>();

                    if (parentCubeScript != null && otherCubeScript != null)
                    {
                        if (parentCubeScript.IsRigidbodyConnected(otherRigidbody))
                        {
                            Debug.Log($"{other.name} objesi zaten bağlı!");
                            return;
                        }
                        if (otherCubeScript.IsRigidbodyConnected(otherRigidbody))
                        {
                            Debug.Log($"{other.name} objesi zaten bağlı!");
                            return;
                        }
                        // **Attach işlemi**
                        other.transform.position = transform.position;
                        other.transform.rotation = Quaternion.identity; // Rotasyonu (0, 0, 0) yapar
                        FixedJoint joint = transform.parent.gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = otherRigidbody;

                        // **Bağlantıları ekle**
                        parentCubeScript.AddConnectedRigidbody(otherRigidbody);
                        otherCubeScript.AddConnectedRigidbody(parentRigidbody);

                        // **Bağlı olan tüm küpleri senkronize et**
                        SyncAllConnections(parentCubeScript, otherCubeScript);

                        Debug.Log($"AttachPoint {name} bağlı: {other.name}");
                    }
                    else
                    {
                        Debug.LogError("Bağlanmaya çalışan küplerin Cube scripti eksik!");
                    }
                }
                else
                {
                    Debug.LogError("Parent objesinin Rigidbody'si bulunamadı!");
                }
            }
            else
            {
                Debug.LogError($"{other.gameObject.name} objesi {gameObject.name} ile temasa girdi, ancak Rigidbody bileşeni bulunamadı!");
            }
        }
    }

    // **Tüm bağlı küplerin bağlantı listelerini senkronize eder**
    private void SyncAllConnections(Cube cubeA, Cube cubeB)
    {
        HashSet<Rigidbody> allConnectedRigidbodies = new HashSet<Rigidbody>();

        // Cube A ve B'nin mevcut bağlantılarını topla
        allConnectedRigidbodies.UnionWith(cubeA.GetConnectedRigidbodies());
        allConnectedRigidbodies.UnionWith(cubeB.GetConnectedRigidbodies());

        // Kendilerini de ekle
        allConnectedRigidbodies.Add(cubeA.GetComponent<Rigidbody>());
        allConnectedRigidbodies.Add(cubeB.GetComponent<Rigidbody>());

        // Tüm küpleri tek tek güncelle
        foreach (Rigidbody rb in allConnectedRigidbodies)
        {
            Cube connectedCube = rb.GetComponentInParent<Cube>();
            if (connectedCube != null)
            {
                foreach (Rigidbody otherRb in allConnectedRigidbodies)
                {
                    if (rb != otherRb) // Kendi kendine eklenmesin
                    {
                        connectedCube.AddConnectedRigidbody(otherRb);
                    }
                }
            }
        }

        Debug.Log("Tüm bağlantılar senkronize edildi!");
    }
}
