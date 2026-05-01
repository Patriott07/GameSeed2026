using UnityEngine;

// Memastikan komponen CharacterController otomatis terpasang
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6.0f;

    private CharacterController controller;

    void Start()
    {
        // Mengambil referensi komponen
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Menangkap input WASD atau Arrow Keys
        // x untuk kiri/kanan (A/D), z untuk maju/mundur (W/S)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Menentukan arah gerak relatif terhadap hadapan karakter
        Vector3 move = transform.right * x + transform.forward * z;

        // Mengeksekusi pergerakan
        controller.Move(move * speed * Time.deltaTime);
    }
}