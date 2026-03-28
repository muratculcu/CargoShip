using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectOfType<GameManager>()  == null) new GameObject("GameManager").AddComponent<GameManager>();
        if (FindObjectOfType<ScoreManager>() == null) new GameObject("ScoreManager").AddComponent<ScoreManager>();
        if (FindObjectOfType<SceneLoader>()  == null) new GameObject("SceneLoader").AddComponent<SceneLoader>();
    }
}
