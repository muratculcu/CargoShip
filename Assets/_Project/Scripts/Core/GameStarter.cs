using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [Header("References")]
    public ContainerController containerController;
    public ContainerSpawner containerSpawner;
    public TimerController timerController;

    [Header("Prefab")]
    public GameObject containerPrefab;

    [Header("Settings")]
    public float totalTime = 90f;

    void Start()
    {
        if (containerController == null || containerSpawner == null)
        {
            Debug.LogError("[GameStarter] Referanslar eksik!");
            return;
        }

        SetupIsometricCamera();
        containerController.SetPrefab(containerPrefab);
        containerController.OnContainerPlaced = SpawnNext;

        if (timerController != null)
        {
            timerController.startTime = totalTime;
            timerController.OnTimeUp += OnTimeUp;
            timerController.StartTimer();
        }

        SpawnNext();
    }

    void SetupIsometricCamera()
    {
        var cam = Camera.main;
        if (cam == null) return;

        float midX = containerController.shipGrid.width / 2f;
        float midZ = containerController.shipGrid.height / 2f;

        // Izometrik pozisyon: yukari ve arkadan bak
        cam.transform.position = new Vector3(midX, 18f, -10f);
        cam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        cam.orthographic = false;
        cam.fieldOfView = 50f;
    }

    void SpawnNext()
    {
        containerSpawner.Advance();
        if (containerSpawner.Current == null) return;
        containerController.SpawnContainer(containerSpawner.Current);
        Debug.Log($"[GameStarter] {containerSpawner.Current.containerName} spawn edildi!");
    }

    void OnTimeUp()
    {
        Debug.Log("[GameStarter] Sure doldu!");
        GameManager.Instance?.EndGame();
    }
}
