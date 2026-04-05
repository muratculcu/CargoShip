using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("Sure")]
    public Text timerText;

    [Header("Skor")]
    public Text scoreText;

    [Header("Denge Sol/Sag")]
    public RectTransform balanceBarLR;
    public Text balanceTextLR;

    [Header("Denge On/Arka")]
    public RectTransform balanceBarFB;
    public Text balanceTextFB;

    [Header("Doluluk")]
    public RectTransform fillBar;
    public Text fillText;

    [Header("Durum")]
    public Text statusText;

    [Header("References")]
    public TimerController timerController;
    public WeightManager weightManager;
    public ShipGrid shipGrid;

    private float barMaxWidth = 200f;

    void Start()
    {
        if (timerController != null)
            timerController.OnTick += UpdateTimer;

        // Bar genisligini ayarla
        if (balanceBarLR != null) barMaxWidth = balanceBarLR.parent.GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        UpdateBalance();
        UpdateScore();
        UpdateFill();
    }

    void UpdateTimer(float time)
    {
        if (timerText == null) return;
        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{min:00}:{sec:00}";
        timerText.color = time < 15f ? Color.red : Color.white;
    }

    void UpdateBalance()
    {
        if (weightManager == null) return;

        SetBarFill(balanceBarLR, weightManager.BalanceRatioLR);
        SetBarFill(balanceBarFB, weightManager.BalanceRatioFB);

        if (balanceTextLR != null)
        {
            float dev = Mathf.Abs(weightManager.BalanceRatioLR - 0.5f) * 200f;
            balanceTextLR.text = $"Sol/Sag: %{Mathf.RoundToInt(dev)}";
        }

        if (balanceTextFB != null)
        {
            float dev = Mathf.Abs(weightManager.BalanceRatioFB - 0.5f) * 200f;
            balanceTextFB.text = $"On/Arka: %{Mathf.RoundToInt(dev)}";
        }

        if (statusText == null) return;
        switch (weightManager.State)
        {
            case BalanceState.Safe:
                statusText.text = "DENGELI";
                statusText.color = Color.green;
                break;
            case BalanceState.Warning:
                statusText.text = "DIKKAT!";
                statusText.color = Color.yellow;
                break;
            case BalanceState.Danger:
                statusText.text = "TEHLIKE!";
                statusText.color = Color.red;
                break;
            case BalanceState.Capsizing:
                statusText.text = "ALABORA!";
                statusText.color = Color.red;
                break;
        }
    }

    void SetBarFill(RectTransform bar, float ratio)
    {
        if (bar == null) return;
        var img = bar.GetComponent<Image>();
        if (img != null)
        {
            img.color = ratio < 0.35f || ratio > 0.65f ? Color.red :
                        ratio < 0.4f || ratio > 0.6f ? Color.yellow : Color.green;
        }
        float parentWidth = 200f;
        var parent = bar.parent?.GetComponent<RectTransform>();
        if (parent != null) parentWidth = parent.rect.width;
        bar.sizeDelta = new Vector2(parentWidth * ratio, bar.sizeDelta.y);
    }

    void UpdateScore()
    {
        if (scoreText == null || ScoreManager.Instance == null) return;
        scoreText.text = $"SKOR: {ScoreManager.Instance.CurrentScore}";
    }

    void UpdateFill()
    {
        if (shipGrid == null) return;
        float pct = shipGrid.FillPercentage();
        if (fillBar != null)
        {
            float parentWidth = 200f;
            var parent = fillBar.parent?.GetComponent<RectTransform>();
            if (parent != null) parentWidth = parent.rect.width;
            fillBar.sizeDelta = new Vector2(parentWidth * pct, fillBar.sizeDelta.y);
        }
        if (fillText != null) fillText.text = $"Doluluk: %{Mathf.RoundToInt(pct * 100f)}";
    }
}