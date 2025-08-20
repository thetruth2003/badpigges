using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public Camera playerCamera;
    public float maxGrabDistance = 100f;
    public float grabMoveSpeed = 10f;
    private GameObject grabbedObject = null; // Tutulan obje
    private float objectDistance; // Objeye olan mesafe

    // Taşıma durumu
    public bool IsCarrying = false;

    void Update()
    {
        // Sol fare tuşuna basıldığında
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
                TryGrabObject();
        }

        // Sol fare tuşunu bıraktığında
        if (Input.GetMouseButtonUp(0) && grabbedObject != null)
        {
            ReleaseObject();
        }

        // Objeyi hareket ettir
        if (grabbedObject != null)
        {
            MoveGrabbedObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrabDistance))
        {
            if (hit.collider.CompareTag("Cube") || hit.collider.CompareTag("Parts") || hit.collider.CompareTag("wheel"))
            {
                grabbedObject = hit.collider.gameObject;

                // Objeye olan uzaklığı kaydet
                objectDistance = Vector3.Distance(playerCamera.transform.position, grabbedObject.transform.position);

                // Taşıma durumu aktif
                IsCarrying = true;

                // Rigidbody'yi kinematik yaparak fiziksel hareketi durdur
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Obje, en yakın hücreye yerleştiriliyor ve hücre yok ediliyor
            PlaceObjectInCell(grabbedObject);

            grabbedObject = null;
            IsCarrying = false;
        }
    }

    void MoveGrabbedObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPosition = ray.origin + ray.direction * objectDistance; // Sabit mesafede kalmasını sağla

        if (grabbedObject != null)
        {
            grabbedObject.transform.position = Vector3.Lerp(
                grabbedObject.transform.position,
                targetPosition,
                Time.deltaTime * grabMoveSpeed
            );
        }
    }
    void PlaceObjectInCell(GameObject obj)
    {
        Collider[] hits = Physics.OverlapSphere(obj.transform.position, 0.25f); // Yakın hücreleri bul
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Cell")) // Hücre kontrolü
            {
                Vector3 cellPosition = hit.transform.position; // Hücrenin pozisyonunu al

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Rigidbody'yi kinematik yaparak fiziksel etkileşimi durdur
                }

                obj.transform.position = cellPosition; // Objeyi hücrenin merkezine taşı
                obj.transform.rotation = Quaternion.identity; // Rotasyonu sıfırla (0,0,0)

                Debug.Log($"{obj.name} {cellPosition} pozisyonuna yerleştirildi.");

                return;
            }
        }
    }

}
