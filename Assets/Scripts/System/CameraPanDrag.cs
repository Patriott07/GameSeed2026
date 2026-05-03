using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraPanDrag : MonoBehaviour
{
    [Header("Pengaturan Geser Kamera")]
    public float panSpeed = 15f;
    public float smoothSpeed = 10f; // Kecepatan menyusul (Makin kecil = makin melayang/licin)
    private Vector3 dragOrigin;
    private Vector3 targetPosition; // Menyimpan titik tujuan akhir kamera

    [Header("Pengaturan Zoom Kamera")]
    public float zoomSpeed = 5f; // Aku besarkan sedikit agar lebih responsif untuk sistem target
    public float minZoom = 2f;
    public float maxZoom = 12f;
    public float zoomSmoothSpeed = 10f; // Efek empuk saat nge-zoom
    private float targetZoom; // Menyimpan ukuran tujuan akhir zoom

    public CinemachineCamera cam;

    void Awake()
    {
        // cam = GetComponent<CinemachineCamera>();

        // Saat mulai, target posisi dan zoom disamakan dengan posisi asli agar tidak loncat
        targetPosition = transform.position;
        targetZoom = cam.Lens.OrthographicSize;
    }

    void Update()
    {
        Zoom();
        RotateCam();
    }

    void RotateCam()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 newRotate = cam.transform.rotation.eulerAngles;
            newRotate.y += 45;
            cam.transform.DOLocalRotate(newRotate, 1.4f).SetEase(Ease.OutBack);
        }
    }

    void LateUpdate()
    {
        DragCam();

        // --- RAHASIA "GAME FEEL" PREMIUM ADA DI DUA BARIS INI ---

        // 1. Secara perlahan (smooth) menggerakkan posisi kamera menuju targetPosition
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 2. Secara perlahan mengubah ukuran layar menuju targetZoom
        cam.Lens.OrthographicSize = Mathf.Lerp(cam.Lens.OrthographicSize, targetZoom, zoomSmoothSpeed * Time.deltaTime);
    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            // ALIH-ALIH MENGUBAH KAMERA LANGSUNG, KITA UBAH TARGETNYA
            targetZoom -= (scrollInput * zoomSpeed);
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    void DragCam()
    {
       if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        // Gunakan Camera.main untuk menerjemahkan posisi layar, karena Cinemachine tidak punya layar
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(-pos.x * panSpeed, 0, -pos.y * panSpeed);
        
        // Sesuaikan arah gerak dengan rotasi Y kamera agar gesernya gak kebalik pas diputar
        move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move;

        targetPosition += move;

        dragOrigin = Input.mousePosition;
    }
}
