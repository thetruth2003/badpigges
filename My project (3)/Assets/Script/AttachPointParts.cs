using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPointParts : MonoBehaviour
{
    public bool isConnected = false; // Bu attach point'in bağlantı durumu

    private void OnTriggerEnter(Collider other)
    {
        // Eğer temas eden obje "Cube" tag'ine sahipse ve bu attach point daha önce bağlanmamışsa
        if (other.CompareTag("Parts") && !isConnected)
        {
            // Temas eden küpün Rigidbody'sini al
            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

            // Eğer Rigidbody varsa ve parent küpe FixedJoint eklenmemişse
            if (otherRigidbody != null)
            {
                // AttachPoint'e, Cube'un pozisyonunu ve rotasını eşitle
                transform.position = other.transform.position; // AttachPoint'in pozisyonunu, Cube'un pozisyonuna eşitliyoruz
                // Parent objenin Rigidbody'sini al
                Rigidbody parentRigidbody = transform.parent.GetComponent<Rigidbody>();

                // Eğer parent objesinin Rigidbody'si varsa
                if (parentRigidbody != null)
                {
                    // Parent'a FixedJoint ekleyip diğer objeyi bağlıyoruz
                    FixedJoint joint = transform.parent.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = otherRigidbody;

                    // Bağlantı tamamlandı, isConnected'ı true yap
                    isConnected = true;

                    Debug.Log($"AttachPoint {name} bağlı: {other.name} - isConnected: {isConnected}");
                }
            }
        }
    }
}
