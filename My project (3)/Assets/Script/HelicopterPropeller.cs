using UnityEngine;

public class pervane : MonoBehaviour
{
    private bool isSpinning = false;  // Pervanenin dönüp dönmediğini kontrol ederiz
    public float spinSpeed = 200f;    // Dönme hızı
    public GameObject Pervane;       // Pervane objesi (Bu, artık dönecek olan nesne)
    private Rigidbody rb;
    private bool isConnected = false; // Bağlanma durumunu kontrol eden değişken

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody referansını al
    }

    private void Update()
    {
        // Eğer pervane dönüyorsa, dönüşü uygula
        if (isSpinning)
        {
            Pervane.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);  // Sadece Pervane döner
        }

        // G tuşuna basıldığında ileriye kuvvet uygula
        if (Input.GetKey(KeyCode.G))
        {
            if (rb != null)
            {
                isSpinning = !isSpinning; // isSpinning değerini tersine çevir
                // Pervanenin döndüğü yöne göre kuvvet uygula
                Vector3 direction = Pervane.transform.up;  // Pervanenin bakış yönü (dönme yönüne göre)
                rb.AddForce(direction * 1000f, ForceMode.Force);  // Dönme yönünde kuvvet uygula
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isConnected) return;

        // Eğer temas eden obje attachpoint taglerinden birine sahipse
        if (other.CompareTag("AttachPoint180") || other.CompareTag("AttachPoint90") ||
            other.CompareTag("AttachPoint-90") || other.CompareTag("AttachPoint0") ||
            other.CompareTag("AttachPointUp") || other.CompareTag("AttachPointDown"))
        {
            Vector3 rotationAxis = Vector3.zero;
            float rotationAngle = 0f;

            // Tag'e göre dönüş ekseni ve açıyı belirle
            if (other.CompareTag("AttachPoint180"))
            {
                rotationAxis = Vector3.right;
                rotationAngle = -90f;
            }
            else if (other.CompareTag("AttachPoint90"))
            {
                rotationAxis = Vector3.forward;
                rotationAngle = -90f;
            }
            else if (other.CompareTag("AttachPoint-90"))
            {
                rotationAxis = Vector3.forward;
                rotationAngle = 90f;
            }
            else if (other.CompareTag("AttachPoint0"))
            {
                rotationAxis = Vector3.right;
                rotationAngle = 90f;
            }
            else if (other.CompareTag("AttachPointUp"))
            {
                rotationAxis = Vector3.right;
                rotationAngle = 0f;
            }
            else if (other.CompareTag("AttachPointDown"))
            {
                rotationAxis = Vector3.right;
                rotationAngle = 180f;
            }

            if (rotationAxis != Vector3.zero)
            {
                Pervane.transform.Rotate(rotationAxis, rotationAngle, Space.World); // **Sadece Pervane döner**
                isConnected = true; // Bağlandıktan sonra tekrar bağlanmayı engelle
                Debug.Log($"{Pervane.name} objesi {other.gameObject.name} ile temas etti ve {rotationAngle} derece döndü! Artık tekrar bağlanamaz.");
            }
        }
    }
}
