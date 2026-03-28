using UnityEngine;
using UnityEditor;

public class ContainerDataCreator
{
    [MenuItem("CargoShip/Create All Container Assets")]
    public static void CreateAll()
    {
        var containers = new[]
        {
            new { name="20ft_Standard", w=2, h=2, weight=2f, color=new Color(0.22f,0.60f,0.86f) },
            new { name="40ft_Standard", w=4, h=2, weight=4f, color=new Color(0.11f,0.62f,0.46f) },
            new { name="20ft_HighCube", w=2, h=3, weight=6f, color=new Color(0.85f,0.33f,0.18f) },
            new { name="Reefer",        w=2, h=2, weight=3f, color=new Color(0.50f,0.47f,0.87f) },
            new { name="FlatRack",      w=3, h=1, weight=2f, color=new Color(0.73f,0.46f,0.09f) },
            new { name="SmallCargo",    w=1, h=2, weight=1f, color=new Color(0.83f,0.32f,0.49f) },
            new { name="Tank",          w=3, h=2, weight=5f, color=new Color(0.39f,0.60f,0.14f) },
        };

        string path = "Assets/_Project/Data/ContainerTypes";
        System.IO.Directory.CreateDirectory(Application.dataPath + "/_Project/Data/ContainerTypes");

        foreach (var c in containers)
        {
            var data = ScriptableObject.CreateInstance<ContainerData>();
            data.containerName = c.name;
            data.size          = new Vector2Int(c.w, c.h);
            data.weight        = c.weight;
            data.color         = c.color;
            AssetDatabase.CreateAsset(data, $"{path}/{c.name}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[CargoShip] 7 ContainerData asset olusturuldu!");
        EditorUtility.DisplayDialog("Tamamlandi!", "7 ContainerData asset basariyla olusturuldu.", "OK");
    }
}
