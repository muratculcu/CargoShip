using UnityEngine;

public class ShipDeck : MonoBehaviour
{
    [Header("Grid Settings")]
    public int   gridWidth  = 10;
    public int   gridHeight = 8;
    public float cellSize   = 1f;

    [Header("Visuals")]
    public Color deckColor     = new Color(0.3f, 0.25f, 0.2f);
    public Color gridLineColor = new Color(0.4f, 0.35f, 0.3f);

    void Start() => BuildDeck();

    void BuildDeck()
    {
        // Zemin
        var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "DeckFloor";
        floor.transform.SetParent(transform);
        floor.transform.localPosition = new Vector3(gridWidth / 2f, -0.5f, 0f);
        floor.transform.localScale    = new Vector3(gridWidth, 1f, 3f);
        floor.GetComponent<Renderer>().material.color = deckColor;
        Destroy(floor.GetComponent<Collider>());

        // Sol duvar
        CreateWall("WallLeft",  new Vector3(-0.5f, gridHeight / 2f, 0f),
                                new Vector3(0.2f, gridHeight, 3f));
        // Sag duvar
        CreateWall("WallRight", new Vector3(gridWidth + 0.5f, gridHeight / 2f, 0f),
                                new Vector3(0.2f, gridHeight, 3f));

        // Grid cizgileri
        for (int x = 0; x <= gridWidth; x++)
            CreateGridLine(new Vector3(x, gridHeight / 2f, -0.5f),
                           new Vector3(0.05f, gridHeight, 0.05f));
    }

    void CreateWall(string wallName, Vector3 pos, Vector3 scale)
    {
        var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = wallName;
        wall.transform.SetParent(transform);
        wall.transform.localPosition = pos;
        wall.transform.localScale    = scale;
        wall.GetComponent<Renderer>().material.color = deckColor;
        Destroy(wall.GetComponent<Collider>());
    }

    void CreateGridLine(Vector3 pos, Vector3 scale)
    {
        var line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        line.name = "GridLine";
        line.transform.SetParent(transform);
        line.transform.localPosition = pos;
        line.transform.localScale    = scale;
        line.GetComponent<Renderer>().material.color = gridLineColor;
        Destroy(line.GetComponent<Collider>());
    }
}
