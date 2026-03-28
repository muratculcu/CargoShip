using UnityEngine;

public class ContainerSpawner : MonoBehaviour
{
    public static ContainerSpawner Instance { get; private set; }

    [Header("Container Types")]
    public ContainerData[] containerTypes;

    public ContainerData Current { get; private set; }
    public ContainerData Next    { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start() { Current = GetRandom(); Next = GetRandom(); }

    public ContainerData Advance() { Current = Next; Next = GetRandom(); return Current; }

    ContainerData GetRandom() => containerTypes[Random.Range(0, containerTypes.Length)];
}
