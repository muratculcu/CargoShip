using UnityEngine;

public enum GameState { Idle, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()  => SetState(GameState.Playing);
    public void PauseGame()  => SetState(GameState.Paused);
    public void ResumeGame() => SetState(GameState.Playing);
    public void EndGame()    => SetState(GameState.GameOver);

    void SetState(GameState s) { CurrentState = s; Debug.Log($"[GameManager] {s}"); }
}
