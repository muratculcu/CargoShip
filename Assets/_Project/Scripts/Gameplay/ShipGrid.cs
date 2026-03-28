using UnityEngine;

public class ShipGrid : MonoBehaviour
{
    public static ShipGrid Instance { get; private set; }

    [Header("Grid Size")]
    public int width  = 10;
    public int height = 8;

    private bool[,] cells;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        cells = new bool[width, height];
    }

    public bool CanPlace(Vector2Int pos, Vector2Int size)
    {
        for (int x = pos.x; x < pos.x + size.x; x++)
        for (int y = pos.y; y < pos.y + size.y; y++)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            if (cells[x, y]) return false;
        }
        return true;
    }

    public void Place(Vector2Int pos, Vector2Int size)
    {
        for (int x = pos.x; x < pos.x + size.x; x++)
        for (int y = pos.y; y < pos.y + size.y; y++)
            cells[x, y] = true;
    }

    public void Clear() => cells = new bool[width, height];

    public float FillPercentage()
    {
        int filled = 0;
        for (int x = 0; x < width;  x++)
        for (int y = 0; y < height; y++)
            if (cells[x, y]) filled++;
        return (float)filled / (width * height);
    }
}
