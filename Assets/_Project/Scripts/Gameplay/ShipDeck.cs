using UnityEngine;

public class ShipDeck : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 10;
    public int gridHeight = 8;
    public float cellSize = 1f;
    public float deckDepth = 3f;

    [Header("Colors")]
    public Color deckColor = new Color(0.25f, 0.22f, 0.18f);
    public Color wallColor = new Color(0.20f, 0.18f, 0.15f);
    public Color gridLineColor = new Color(0.35f, 0.30f, 0.25f);

    void Start() => BuildDeck();

    void BuildDeck()
    {
        // Ana zemin
        CreateBox("DeckFloor",
            new Vector3(gridWidth / 2f, -0.5f, deckDepth / 2f),
            new Vector3(gridWidth, 1f, deckDepth),
            deckColor);

        // Sol duvar
        CreateBox("WallLeft",
            new Vector3(-0.5f, gridHeight / 2f, deckDepth / 2f),
            new Vector3(0.3f, gridHeight, deckDepth),
            wallColor);

        // Sag duvar
        CreateBox("WallRight",
            new Vector3(gridWidth + 0.5f, gridHeight / 2f, deckDepth / 2f),
            new Vector3(0.3f, gridHeight, deckDepth),
            wallColor);

        // Arka duvar
        CreateBox("WallBack",
            new Vector3(gridWidth / 2f, gridHeight / 2f, deckDepth + 0.15f),
            new Vector3(gridWidth, gridHeight, 0.3f),
            wallColor);

        // Grid cizgileri (yatay)
        for (int x = 0; x <= gridWidth; x++)
            CreateBox($"GridV_{x}",
                new Vector3(x, gridHeight / 2f, deckDepth / 2f),
                new Vector3(0.05f, gridHeight, deckDepth),
                gridLineColor);

        // Grid cizgileri (dikey)
        for (int y = 0; y <= gridHeight; y++)
            CreateBox($"GridH_{y}",
                new Vector3(gridWidth / 2f, y, deckDepth / 2f),
                new Vector3(gridWidth, 0.05f, deckDepth),
                gridLineColor);
    }

    void CreateBox(string boxName, Vector3 pos, Vector3 scale, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = boxName;
        go.transform.SetParent(transform);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().material.color = color;
        Destroy(go.GetComponent<Collider>());
    }
}
