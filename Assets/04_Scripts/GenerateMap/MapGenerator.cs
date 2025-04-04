using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int width = 100; // Map width in tiles
    public int height = 50; // Map height in tiles
    public float noiseScale = 0.1f; // Scale of Perlin noise (controls terrain smoothness)

    [Header("Tilemap References")]
    public Tilemap terrainTilemap; // Tilemap for terrain (snow, forest, water)
    public Tilemap objectTilemap; // Tilemap for objects (trees, buildings)

    [Header("Terrain Tiles")]
    public TileBase snowTile;
    public TileBase forestTile;
    public TileBase waterTile;

    [Header("Object Tiles")]
    public TileBase treeTile;
    public TileBase buildingTile;
    public float treeDensity = 0.1f; // Chance to spawn a tree
    public float buildingDensity = 0.02f; // Chance to spawn a building

    // Generate the map
    public void GenerateMap()
    {
        // Clear the tilemaps
        terrainTilemap.ClearAllTiles();
        objectTilemap.ClearAllTiles();

        // Generate terrain using Perlin noise
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                Vector3Int position = new Vector3Int(x, y, 0);

                // Assign terrain based on noise value
                if (noiseValue < 0.3f)
                {
                    terrainTilemap.SetTile(position, waterTile); // Water
                }
                else if (noiseValue < 0.6f)
                {
                    terrainTilemap.SetTile(position, forestTile); // Forest
                }
                else
                {
                    terrainTilemap.SetTile(position, snowTile); // Snow
                }
            }
        }

        // Place objects (trees, buildings) on non-water tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase terrainTile = terrainTilemap.GetTile(position);

                // Only place objects on forest or snow tiles
                if (terrainTile != waterTile)
                {
                    // Place trees
                    if (terrainTile == forestTile && Random.value < treeDensity)
                    {
                        objectTilemap.SetTile(position, treeTile);
                    }

                    // Place buildings
                    if (terrainTile == snowTile && Random.value < buildingDensity)
                    {
                        objectTilemap.SetTile(position, buildingTile);
                    }
                }
            }
        }
    }

    // Clear the map
    public void ClearMap()
    {
        terrainTilemap.ClearAllTiles();
        objectTilemap.ClearAllTiles();
    }
}

// Custom editor to add a "Generate Map" button in the Inspector
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGenerator mapGenerator = (MapGenerator)target;

        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.GenerateMap();
        }

        if (GUILayout.Button("Clear Map"))
        {
            mapGenerator.ClearMap();
        }
    }
}