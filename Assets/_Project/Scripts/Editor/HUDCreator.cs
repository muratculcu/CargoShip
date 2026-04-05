using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class HUDCreator
{
    [MenuItem("CargoShip/Create HUD")]
    public static void CreateHUD()
    {
        // Canvas
        var canvasGO = new GameObject("GameCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // HUD GameObject
        var hudGO = new GameObject("HUD");
        hudGO.transform.SetParent(canvasGO.transform);
        var hud = hudGO.AddComponent<GameHUD>();

        // Timer
        hud.timerText = CreateText(canvasGO.transform, "TimerText", "01:30",
            new Vector2(0, -30), new Vector2(200, 50), new Vector2(0.5f, 1f));

        // Skor
        hud.scoreText = CreateText(canvasGO.transform, "ScoreText", "SKOR: 0",
            new Vector2(-10, -30), new Vector2(200, 50), new Vector2(1f, 1f));

        // Durum
        hud.statusText = CreateText(canvasGO.transform, "StatusText", "DENGELI",
            new Vector2(0, -90), new Vector2(250, 50), new Vector2(0.5f, 1f));
        hud.statusText.color = Color.green;
        hud.statusText.fontSize = 20;

        // Sol/Sag denge bar arkaplan
        var lrBG = CreateImage(canvasGO.transform, "BalanceBG_LR",
            new Vector2(0, -150), new Vector2(300, 20), new Vector2(0.5f, 1f),
            new Color(0.2f, 0.2f, 0.2f));

        // Sol/Sag denge bar
        var lrBar = CreateImage(lrBG.transform, "BalanceBarLR",
            Vector2.zero, new Vector2(300, 20), new Vector2(0.5f, 0.5f),
            Color.green);
        hud.balanceBarLR = lrBar.GetComponent<RectTransform>();

        // Sol/Sag denge text
        hud.balanceTextLR = CreateText(canvasGO.transform, "BalanceTextLR", "Sol/Sag: %0",
            new Vector2(0, -175), new Vector2(300, 30), new Vector2(0.5f, 1f));
        hud.balanceTextLR.fontSize = 12;

        // On/Arka denge bar arkaplan
        var fbBG = CreateImage(canvasGO.transform, "BalanceBG_FB",
            new Vector2(0, -210), new Vector2(300, 20), new Vector2(0.5f, 1f),
            new Color(0.2f, 0.2f, 0.2f));

        // On/Arka denge bar
        var fbBar = CreateImage(fbBG.transform, "BalanceBarFB",
            Vector2.zero, new Vector2(300, 20), new Vector2(0.5f, 0.5f),
            Color.blue);
        hud.balanceBarFB = fbBar.GetComponent<RectTransform>();

        // On/Arka denge text
        hud.balanceTextFB = CreateText(canvasGO.transform, "BalanceTextFB", "On/Arka: %0",
            new Vector2(0, -235), new Vector2(300, 30), new Vector2(0.5f, 1f));
        hud.balanceTextFB.fontSize = 12;

        // Doluluk bar arkaplan
        var fillBG = CreateImage(canvasGO.transform, "FillBG",
            new Vector2(0, -270), new Vector2(300, 20), new Vector2(0.5f, 1f),
            new Color(0.2f, 0.2f, 0.2f));

        // Doluluk bar
        var fillBar = CreateImage(fillBG.transform, "FillBar",
            Vector2.zero, new Vector2(0, 20), new Vector2(0f, 0.5f),
            Color.yellow);
        var fillRT = fillBar.GetComponent<RectTransform>();
        fillRT.anchorMin = new Vector2(0, 0);
        fillRT.anchorMax = new Vector2(0, 1);
        fillRT.pivot     = new Vector2(0, 0.5f);
        hud.fillBar = fillRT;

        // Doluluk text
        hud.fillText = CreateText(canvasGO.transform, "FillText", "Doluluk: %0",
            new Vector2(0, -295), new Vector2(300, 30), new Vector2(0.5f, 1f));
        hud.fillText.fontSize = 12;

        // Eski Canvas varsa sil
        var oldCanvas = GameObject.Find("GameCanvas");
        if (oldCanvas != null && oldCanvas != canvasGO)
            Object.DestroyImmediate(oldCanvas);

        Selection.activeGameObject = hudGO;
        EditorUtility.DisplayDialog("Tamamlandi!", "HUD basariyla olusturuldu! Referanslari Inspector'dan baglayin.", "OK");
        Debug.Log("[HUDCreator] HUD olusturuldu!");
    }

    static Text CreateText(Transform parent, string name, string content,
        Vector2 anchoredPos, Vector2 size, Vector2 anchor)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot     = anchor;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        var txt = go.AddComponent<Text>();
        txt.text      = content;
        txt.color     = Color.white;
        txt.fontSize  = 16;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return txt;
    }

    static GameObject CreateImage(Transform parent, string name,
        Vector2 anchoredPos, Vector2 size, Vector2 anchor, Color color)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot     = anchor;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        var img = go.AddComponent<Image>();
        img.color = color;
        return go;
    }
}
