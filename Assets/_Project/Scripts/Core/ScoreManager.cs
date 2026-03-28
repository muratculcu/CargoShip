using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int CurrentScore { get; private set; }
    public int HighScore    { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        if (CurrentScore > HighScore) { HighScore = CurrentScore; PlayerPrefs.SetInt("HighScore", HighScore); }
        Debug.Log($"[ScoreManager] Score:{CurrentScore} High:{HighScore}");
    }

    public void ResetScore() => CurrentScore = 0;
}
