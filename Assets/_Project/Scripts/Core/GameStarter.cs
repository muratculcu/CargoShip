using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [Header("References")]
    public ContainerController containerController;
    public ContainerSpawner    containerSpawner;

    [Header("Test Prefab")]
    public GameObject containerPrefab;

    void Start()
    {
        if (containerController == null || containerSpawner == null)
        {
            Debug.LogError("[GameStarter] Referanslar eksik!");
            return;
        }

        // Kamerayı grid merkezine odakla
        var cam = Camera.main;
        if (cam != null)
        {
            float midX = containerController.shipGrid.width  / 2f;
            float midY = containerController.shipGrid.height / 2f;
            cam.transform.position = new Vector3(midX, midY, -15f);
            cam.orthographic = true;
            cam.orthographicSize = 8f;
        }

        SpawnNext();
    }

    public void SpawnNext()
    {
        if (containerSpawner.Current == null)
        {
            Debug.LogError("[GameStarter] ContainerSpawner.Current null!");
            return;
        }
        containerController.SpawnContainer(containerSpawner.Current, containerPrefab);
        Debug.Log("[GameStarter] Konteyner spawn edildi!");
    }
}
