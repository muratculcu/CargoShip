using UnityEngine;
using System;

public class ContainerController : MonoBehaviour
{
    [Header("References")]
    public ShipGrid shipGrid;
    public WeightManager weightManager;

    [Header("Settings")]
    public float moveInterval = 0.15f;
    public float fastDropSpeed = 0.05f;
    public float normalDropSpeed = 0.8f;
    public float containerDepth = 2.5f;

    public Action OnContainerPlaced;

    private ContainerData currentData;
    private GameObject currentVisual;
    private GameObject containerPrefab;
    private Vector2Int gridPos;
    private Vector2Int currentSize;
    private float dropTimer;
    private float dropSpeed;
    private float moveTimer;
    private bool isActive;
    private int undoCount = 3;

    public int UndoCount => undoCount;

    public void SetPrefab(GameObject prefab) => containerPrefab = prefab;

    public void SpawnContainer(ContainerData data)
    {
        currentData = data;
        currentSize = data.size;
        gridPos = new Vector2Int((shipGrid.width - currentSize.x) / 2, shipGrid.height - 1);
        dropSpeed = normalDropSpeed;
        isActive = true;
        dropTimer = 0f;

        currentVisual = Instantiate(containerPrefab);

        // 3D boyut: x=genislik, y=yukseklik, z=derinlik
        currentVisual.transform.localScale =
            new Vector3(currentSize.x * 0.95f, currentSize.y * 0.95f, containerDepth);

        var rend = currentVisual.GetComponent<Renderer>();
        if (rend != null) rend.material.color = data.color;

        currentVisual.transform.position = GridToWorld(gridPos);
    }

    void Update()
    {
        if (!isActive) return;
        HandleInput();
        HandleDrop();
    }

    void HandleInput()
    {
        moveTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow) && moveTimer <= 0) MoveLeft();
        if (Input.GetKey(KeyCode.RightArrow) && moveTimer <= 0) MoveRight();
        if (Input.GetKeyDown(KeyCode.UpArrow)) Rotate();
        if (Input.GetKey(KeyCode.DownArrow)) dropSpeed = fastDropSpeed;
        else dropSpeed = normalDropSpeed;
        if (Input.GetKeyDown(KeyCode.Space)) HardDrop();
    }

    void MoveLeft()
    {
        var next = gridPos + Vector2Int.left;
        if (shipGrid.CanPlace(next, currentSize))
        { gridPos = next; UpdateVisual(); moveTimer = moveInterval; }
    }

    void MoveRight()
    {
        var next = gridPos + Vector2Int.right;
        if (shipGrid.CanPlace(next, currentSize))
        { gridPos = next; UpdateVisual(); moveTimer = moveInterval; }
    }

    void Rotate()
    {
        var rotated = new Vector2Int(currentSize.y, currentSize.x);
        var adjusted = new Vector2Int(
            Mathf.Clamp(gridPos.x, 0, shipGrid.width - rotated.x),
            Mathf.Clamp(gridPos.y, 0, shipGrid.height - rotated.y));
        if (shipGrid.CanPlace(adjusted, rotated))
        { currentSize = rotated; gridPos = adjusted; UpdateVisual(); }
    }

    void HardDrop()
    {
        while (CanMoveDown()) gridPos += Vector2Int.down;
        PlaceContainer();
    }

    void HandleDrop()
    {
        dropTimer -= Time.deltaTime;
        if (dropTimer > 0) return;
        dropTimer = dropSpeed;
        if (CanMoveDown()) { gridPos += Vector2Int.down; UpdateVisual(); }
        else PlaceContainer();
    }

    bool CanMoveDown()
    {
        var next = gridPos + Vector2Int.down;
        return next.y >= 0 && shipGrid.CanPlace(next, currentSize);
    }

    void PlaceContainer()
    {
        isActive = false;
        if (currentVisual)
            currentVisual.transform.position = GridToWorld(gridPos);
        currentVisual = null;

        shipGrid.Place(gridPos, currentSize);
        weightManager.RegisterContainer(gridPos, currentSize, currentData.weight);
        ScoreManager.Instance?.AddScore(Mathf.RoundToInt(currentData.weight * 10));
        Debug.Log($"[ContainerController] Placed at {gridPos} size {currentSize}");
        OnContainerPlaced?.Invoke();
    }

    public bool TryUndo()
    {
        if (undoCount <= 0 || isActive) return false;
        weightManager.RemoveLastContainer();
        undoCount--;
        return true;
    }

    void UpdateVisual()
    {
        if (currentVisual)
            currentVisual.transform.position = GridToWorld(gridPos);
    }

    // Z ekseninde ortala (deckDepth / 2)
    Vector3 GridToWorld(Vector2Int pos) =>
        new Vector3(
            pos.x + currentSize.x * 0.5f,
            pos.y + currentSize.y * 0.5f,
            containerDepth / 2f);
}
