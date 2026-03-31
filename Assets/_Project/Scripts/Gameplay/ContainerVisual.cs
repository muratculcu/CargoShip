using UnityEngine;

public class ContainerVisual : MonoBehaviour
{
    private Renderer rend;

    void Awake() => rend = GetComponent<Renderer>();

    public void SetColor(Color color)
    {
        if (rend != null)
            rend.material.color = color;
    }

    public void SetSize(Vector2Int size)
    {
        transform.localScale = new Vector3(size.x, size.y, 1f);
    }
}
