using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    public List<Transform> attachPoints; // Tüm attach point'lerin listesi
    private bool isConnected = false;   // Bağlantı durumu
    private FixedJoint joint;           // FixedJoint referansı

    private void OnTriggerEnter(Collider other)
    {
        // Eğer diğer nesne bir attach point ise
        if (other.CompareTag("AttachPoint0") && !isConnected)
        {
            Transform otherAttachPoint = other.transform;

            // İki attach point arasındaki bağlantıyı oluştur
            ConnectCubes0(otherAttachPoint);
        }
        // Eğer diğer nesne bir attach point ise
        if (other.CompareTag("AttachPoint90") && !isConnected)
        {
            Transform otherAttachPoint = other.transform;

            // İki attach point arasındaki bağlantıyı oluştur
            ConnectCubes90(otherAttachPoint);
        }
        // Eğer diğer nesne bir attach point ise
        if (other.CompareTag("AttachPoint180") && !isConnected)
        {
            Transform otherAttachPoint = other.transform;

            // İki attach point arasındaki bağlantıyı oluştur
            ConnectCubes180(otherAttachPoint);
        }
        // Eğer diğer nesne bir attach point ise
        if (other.CompareTag("AttachPoint-90") && !isConnected)
        {
            Transform otherAttachPoint = other.transform;

            // İki attach point arasındaki bağlantıyı oluştur
            ConnectCubes_90(otherAttachPoint);
        }
    }

    // İki attach point'i bağla
    private void ConnectCubes0(Transform otherAttachPoint)
    {
        transform.rotation = Quaternion.identity;
        transform.position = otherAttachPoint.position;
        // Bu nesneye FixedJoint ekle
        joint = gameObject.AddComponent<FixedJoint>();
        Rigidbody otherRigidbody = otherAttachPoint.GetComponentInParent<Rigidbody>();

        // Eğer diğer attach point'in bağlı olduğu bir Rigidbody varsa
        if (otherRigidbody != null)
        {
            joint.connectedBody = otherRigidbody;

            // Bağlantıyı yaptıktan sonra isConnected flag'ini true yap
            isConnected = true;

            Debug.Log($"Connected to: {otherAttachPoint.name}");
        }
    }
    // İki attach point'i bağla
    private void ConnectCubes90(Transform otherAttachPoint)
    {
        transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        transform.position = otherAttachPoint.position;
        // Bu nesneye FixedJoint ekle
        joint = gameObject.AddComponent<FixedJoint>();
        Rigidbody otherRigidbody = otherAttachPoint.GetComponentInParent<Rigidbody>();

        // Eğer diğer attach point'in bağlı olduğu bir Rigidbody varsa
        if (otherRigidbody != null)
        {
            joint.connectedBody = otherRigidbody;

            // Bağlantıyı yaptıktan sonra isConnected flag'ini true yap
            isConnected = true;

            Debug.Log($"Connected to: {otherAttachPoint.name}");
        }
    }
    // İki attach point'i bağla
    private void ConnectCubes180(Transform otherAttachPoint)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = otherAttachPoint.position;
        // Bu nesneye FixedJoint ekle
        joint = gameObject.AddComponent<FixedJoint>();
        Rigidbody otherRigidbody = otherAttachPoint.GetComponentInParent<Rigidbody>();

        // Eğer diğer attach point'in bağlı olduğu bir Rigidbody varsa
        if (otherRigidbody != null)
        {
            joint.connectedBody = otherRigidbody;

            // Bağlantıyı yaptıktan sonra isConnected flag'ini true yap
            isConnected = true;

            Debug.Log($"Connected to: {otherAttachPoint.name}");
        }
    }
    // İki attach point'i bağla
    private void ConnectCubes_90(Transform otherAttachPoint)
    {
        transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        transform.position = otherAttachPoint.position;
        // Bu nesneye FixedJoint ekle
        joint = gameObject.AddComponent<FixedJoint>();
        Rigidbody otherRigidbody = otherAttachPoint.GetComponentInParent<Rigidbody>();

        // Eğer diğer attach point'in bağlı olduğu bir Rigidbody varsa
        if (otherRigidbody != null)
        {
            joint.connectedBody = otherRigidbody;

            // Bağlantıyı yaptıktan sonra isConnected flag'ini true yap
            isConnected = true;

            Debug.Log($"Connected to: {otherAttachPoint.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Bağlantıyı kes
        //if (other.CompareTag("AttachPoint") && isConnected)
        {
            //Destroy(joint); // FixedJoint'i kaldır
            //isConnected = false;
            //Debug.Log($"Disconnected from: {other.name}");
        }
    }
}