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

        // Kamerayı ayarla
        var cam = Camera.main;
        if (cam != null)
        {
            float midX = containerController.shipGrid.width / 2f;
            float midY = containerController.shipGrid.height / 2f;
            cam.transform.position = new Vector3(midX, midY, -15f);
            cam.orthographic = true;
            cam.orthographicSize = 7f;
        }

        // Prefab'i controller'a ver
        containerController.SetPrefab(containerPrefab);

        // Event'i baglat
        containerController.OnContainerPlaced = SpawnNext;

        // Timer baslat
        if (timerController != null)
        {
            timerController.startTime = totalTime;
            timerController.OnTimeUp += OnTimeUp;
            timerController.StartTimer();
        }

        // Ilk konteyneri spawn et
        SpawnNext();
    }

    void SpawnNext()
    {
        containerSpawner.Advance();
        if (containerSpawner.Current == null)
        {
            Debug.LogError("[GameStarter] Current null!");
            return;
        }
        containerController.SpawnContainer(containerSpawner.Current);
        Debug.Log($"[GameStarter] {containerSpawner.Current.containerName} spawn edildi!");
    }

    void OnTimeUp()
    {
        Debug.Log("[GameStarter] Sure doldu!");
        GameManager.Instance?.EndGame();
    }
}