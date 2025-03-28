using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SetTilemapLayerEditor : EditorWindow
{
    [MenuItem("Tools/Set Tilemap Layer")]
    public static void SetTilemapLayer()
    {
        foreach (var tilemap in Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
        {
            if (tilemap.gameObject.name.Contains("Walls"))
            {
                tilemap.gameObject.layer = LayerMask.NameToLayer("Ground");
                tilemap.gameObject.tag = "Ground";
            }
        }
        Debug.Log("Tilemap layer & tag set!");
    }
}
