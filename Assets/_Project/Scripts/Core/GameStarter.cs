using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [Header("References")]
    public ContainerController containerController;
    public ContainerSpawner containerSpawner;

    [Header("Test Prefab")]
    public GameObject containerPrefab;

    void Start()
    {
        if (containerController == null || containerSpawner == null)
        {
            Debug.LogError("[GameStarter] Referanslar eksik!");
            return;
        }

        var cam = Camera.main;
        if (cam != null)
        {
            float midX = containerController.shipGrid.width / 2f;
            float midY = containerController.shipGrid.height / 2f;
            cam.transform.position = new Vector3(midX, midY, -15f);
            cam.orthographic = true;
            cam.orthographicSize = 7f;
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

        var data = containerSpawner.Current;
        var go = Instantiate(containerPrefab);

        // Boyut ve renk ayarla
        go.transform.localScale = new Vector3(data.size.x, data.size.y, 1f);

        var rend = go.GetComponent<Renderer>();
        if (rend != null) rend.material.color = data.color;

        containerController.SpawnContainer(data, go);
        Debug.Log($"[GameStarter] {data.containerName} spawn edildi! Renk:{data.color}");
    }
}