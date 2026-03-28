using UnityEngine;
using System;

public class TimerController : MonoBehaviour
{
    public static TimerController Instance { get; private set; }

    [Header("Settings")]
    public float startTime = 90f;

    public float TimeRemaining { get; private set; }
    public bool  IsRunning     { get; private set; }

    public event Action        OnTimeUp;
    public event Action<float> OnTick;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartTimer()  { TimeRemaining = startTime; IsRunning = true; }
    public void StopTimer()   => IsRunning = false;
    public void PauseTimer()  => IsRunning = false;
    public void ResumeTimer() => IsRunning = true;

    void Update()
    {
        if (!IsRunning) return;
        TimeRemaining -= Time.deltaTime;
        OnTick?.Invoke(TimeRemaining);
        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            IsRunning = false;
            OnTimeUp?.Invoke();
        }
    }
}
