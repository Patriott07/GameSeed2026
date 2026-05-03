using UnityEngine;
using UnityEngine.AI;

public class SetTargetPlayer2 : MonoBehaviour
{
    private NavMeshAgent agent;
    public int index = 1;
    // Keyword 'static' bikin variabel ini dishare ke SEMUA player yang pakai script ini
    public static int globalTurnIndex = 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetDest();
        }
       

        Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin,
              Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.red);
    }

    void SetTargetDest()
    {
        // Membuat laser dari posisi mouse di layar ke dunia 3D
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Jika laser mengenai sesuatu (lantai/objek)
        if (Physics.Raycast(ray, out hit))
        {
            // Cek apakah sekarang giliran index saya?
            if (index == globalTurnIndex)
            {
                Debug.Log($"Player {index} bergerak ke: {hit.point}");
                agent.SetDestination(hit.point);

                // Ganti giliran secara global
                // Kalau sekarang 1 jadi 2, kalau 2 jadi 1
                globalTurnIndex = (globalTurnIndex == 1) ? 2 : 1;

                Debug.Log($"Sekarang giliran Player {globalTurnIndex}");
            }
        }

    }
}
