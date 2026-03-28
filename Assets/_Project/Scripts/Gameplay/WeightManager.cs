using UnityEngine;
using System.Collections.Generic;

public enum BalanceState { Safe, Warning, Danger, Capsizing }

public class WeightManager : MonoBehaviour
{
    public static WeightManager Instance { get; private set; }

    [Header("Ship Grid")]
    public int gridWidth  = 10;
    public int gridHeight = 8;

    private readonly List<(Vector2 center, float weight)> placed = new();

    public float BalanceRatioLR { get; private set; } = 0.5f;
    public float BalanceRatioFB { get; private set; } = 0.5f;
    public BalanceState State   { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void RegisterContainer(Vector2Int gridPos, Vector2Int size, float weight)
    {
        Vector2 center = new Vector2(gridPos.x + size.x / 2f, gridPos.y + size.y / 2f);
        placed.Add((center, weight));
        Recalculate();
    }

    public void RemoveLastContainer()
    {
        if (placed.Count > 0) { placed.RemoveAt(placed.Count - 1); Recalculate(); }
    }

    public void Clear() { placed.Clear(); BalanceRatioLR = BalanceRatioFB = 0.5f; State = BalanceState.Safe; }

    void Recalculate()
    {
        float midX = gridWidth / 2f;
        float midY = gridHeight / 2f;
        float leftT = 0, rightT = 0, frontT = 0, backT = 0;

        foreach (var (center, w) in placed)
        {
            float dx = center.x - midX;
            float dy = center.y - midY;
            if (dx < 0) leftT  += w * Mathf.Abs(dx); else rightT += w * dx;
            if (dy < 0) frontT += w * Mathf.Abs(dy); else backT  += w * dy;
        }

        float totalLR = leftT + rightT;
        float totalFB = frontT + backT;
        BalanceRatioLR = totalLR > 0 ? rightT / totalLR : 0.5f;
        BalanceRatioFB = totalFB > 0 ? backT  / totalFB : 0.5f;

        float dev = Mathf.Max(
            Mathf.Abs(BalanceRatioLR - 0.5f),
            Mathf.Abs(BalanceRatioFB - 0.5f)) * 2f;

        State = dev >= 0.70f ? BalanceState.Capsizing :
                dev >= 0.40f ? BalanceState.Danger    :
                dev >= 0.20f ? BalanceState.Warning   : BalanceState.Safe;

        Debug.Log($"[WeightManager] LR:{BalanceRatioLR:F2} FB:{BalanceRatioFB:F2} => {State}");
    }
}
