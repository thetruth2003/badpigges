using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public Camera playerCamera;
    public GameObject gridManager;  // GridManager referansı
    private CharacterController characterController;
    private float verticalLookRotation = 0f; // Kameranın yukarı-aşağı dönüş açısını saklar
    public float maxRaycastDistance = 10f; // Raycast'in menzili

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Fareyi ekran ortasında kilitle
    }

    void TurnObject(float rotation)
    {
        // Fare pozisyonundan bir ray oluşturuyoruz
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRaycastDistance))
        {
            // Objeyi döndür
            hit.collider.gameObject.transform.Rotate(0f, rotation, 0f);
            Debug.Log("Temas edilen obje: " + hit.collider.gameObject.name);
        }
    }

    void Update()
    {
        // 'E' tuşuna basıldığında sağa döndür
        if (Input.GetKeyDown(KeyCode.E)) TurnObject(90f);
        // 'Q' tuşuna basıldığında sola döndür
        if (Input.GetKeyDown(KeyCode.Q)) TurnObject(-90f);
        // 'Tab' tuşuna basıldığında GridManager'ı kapat
        if (Input.GetKeyDown(KeyCode.Tab) && gridManager != null)
        {
            // GridManager'ı kapat
            gridManager.SetActive(false);
            Debug.Log("Grid kapatıldı.");
            // Sahnedeki tüm Rigidbody bileşenlerini al ve kinematik durumlarını kaldır
            Rigidbody[] allRigidbodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            foreach (Rigidbody rb in allRigidbodies)
            {
                rb.isKinematic = false; // Kinematik durumu kaldır
            }
            // Hem "Cube" hem de "Parts" tag'ine sahip objelerin kinematik durumlarını aç
            GameObject[] cubesAndParts = GameObject.FindGameObjectsWithTag("Cube");
            GameObject[] parts = GameObject.FindGameObjectsWithTag("Parts");
            GameObject[] wheel = GameObject.FindGameObjectsWithTag("wheel");
            // "Cube" tag'ine sahip objeler üzerinde işlem yap
            foreach (GameObject cube in cubesAndParts)
            {
                // Çocuk nesneleri kontrol et
                foreach (Transform child in cube.transform)
                {
                    child.gameObject.SetActive(true); // Nesneyi aktif hale getir
                }
            }

            // "Parts" tag'ine sahip objeler üzerinde işlem yap
            foreach (GameObject wheels in wheel)
            {
                // FrontWheele script'ini kontrol et ve aktif hale getir
                FrontWheele frontWheele = wheels.GetComponent<FrontWheele>();
                if (frontWheele != null)
                {
                    MonoBehaviour mono = frontWheele as MonoBehaviour; // Script'i MonoBehaviour olarak al
                    if (mono != null && !mono.enabled) // Eğer script devre dışıysa
                    {
                        mono.enabled = true; // Script'i aktif hale getir
                    }
                }

                // BackWheele script'ini kontrol et ve aktif hale getir
                BackWheele backWheele = wheels.GetComponent<BackWheele>();
                if (backWheele != null)
                {
                    MonoBehaviour mono2 = backWheele as MonoBehaviour; // Script'i MonoBehaviour olarak al
                    if (mono2 != null && !mono2.enabled) // Eğer script devre dışıysa
                    {
                        mono2.enabled = true; // Script'i aktif hale getir
                    }
                }
            }
        }
            // WASD ile hareket
            float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        characterController.Move((transform.right * moveX + transform.forward * moveZ) * moveSpeed * Time.deltaTime);

        // Mouse ile sağa sola ve yukarı-aşağı bakma
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Yatay dönüş (sağa-sola)
        transform.Rotate(Vector3.up * mouseX);

        // Dikey dönüş (yukarı-aşağı)
        verticalLookRotation = Mathf.Clamp(verticalLookRotation - mouseY, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }
}
