using UnityEngine;

public enum ContainerType
{
    Standard20ft,
    Standard40ft,
    HighCube20ft,
    Reefer,
    FlatRack,
    SmallCargo,
    Tank
}

[CreateAssetMenu(fileName = "NewContainer", menuName = "CargoShip/Container Data")]
public class ContainerData : ScriptableObject
{
    public string containerName;
    public ContainerType type;
    public Vector2Int size;
    public float weight;
    public Color color = Color.white;
    public Sprite icon;
}
