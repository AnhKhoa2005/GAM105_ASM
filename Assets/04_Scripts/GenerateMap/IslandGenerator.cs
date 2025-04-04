using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IslandGenerator : MonoBehaviour
{
    public enum TileType
    {
        Water,
        Sand,
        Grass,
        Tree,
        Lake
    }

    public enum IslandShape
    {
        CurvedIsland,
        RectangularIsland,
        CircularIsland,
        CrossIsland,
        IrregularIslandWithLake
    }

    [System.Serializable]
    public class TilemapConfig
    {
        public Tilemap tilemap;
        public TileBase specificTile;
        public List<TileBase> tileOptions;
        public bool useRandomTile;
        public TileType tileType;
    }

    public List<TilemapConfig> tilemapConfigs;
    public int width = 50;
    public int height = 50;
    public float scale = 20f;
    public int borderSize = 5;
    public float islandRadiusFactor = 0.7f;
    public float sandWidth = 0.15f;
    public int treeCount = 10;
    [SerializeField]
    public IslandShape islandShape = IslandShape.IrregularIslandWithLake;
    public float lakeRadius = 3f;
    public float lakeNoiseScale = 10f;

    [Header("Island Generation Settings")]
    [Tooltip("Số lượng đảo trên bản đồ (1 trở lên)")]
    public int islandCount = 1;

    [Tooltip("Seed để tái tạo bản đồ (0 để ngẫu nhiên)")]
    public int seed = 0;

    [Tooltip("Tỷ lệ kích thước đảo so với width/height (0.1 - 1.0)")]
    [Range(0.1f, 1.0f)]
    public float islandSizeFactor = 0.5f; // Tỷ lệ kích thước đảo

    [Header("Custom Island Shape (for IrregularIslandWithLake)")]
    [Tooltip("Hình ảnh đen trắng định nghĩa hình dạng đảo. Đen = nước, Xám = cát, Trắng = cỏ")]
    public Texture2D islandShapeMask;

    void Start()
    {
        GenerateIsland();
    }

    void GenerateIsland()
    {
        // Khởi tạo seed
        if (seed == 0)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(seed);
        Debug.Log($"Using seed: {seed}");

        // Xóa tất cả tilemap
        foreach (var config in tilemapConfigs)
        {
            if (config.tilemap != null)
            {
                config.tilemap.ClearAllTiles();
            }
        }

        // Đặt toàn bộ bản đồ thành nước trước
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetTile(new Vector3Int(x, y, 0), TileType.Water);
            }
        }

        islandCount = Mathf.Max(1, islandCount);

        List<Vector2> islandCenters = GenerateIslandCenters();
        float maxDistance = (Mathf.Min(width, height) * islandSizeFactor) / Mathf.Sqrt(islandCount);

        foreach (Vector2 center in islandCenters)
        {
            switch (islandShape)
            {
                case IslandShape.CurvedIsland:
                    GenerateCurvedIsland(center, maxDistance);
                    break;
                case IslandShape.RectangularIsland:
                    GenerateRectangularIsland(center, maxDistance);
                    break;
                case IslandShape.CircularIsland:
                    GenerateCircularIsland(center, maxDistance);
                    break;
                case IslandShape.CrossIsland:
                    GenerateCrossIsland(center, maxDistance);
                    break;
                case IslandShape.IrregularIslandWithLake:
                    GenerateIrregularIslandWithLake(center, maxDistance);
                    break;
            }
        }

        AddTreeTiles();
    }

    List<Vector2> GenerateIslandCenters()
    {
        List<Vector2> centers = new List<Vector2>();

        if (islandCount == 1)
        {
            centers.Add(new Vector2(width / 2f, height / 2f));
            return centers;
        }

        float minDistanceBetweenIslands = (width + height) / 4f / Mathf.Sqrt(islandCount);

        for (int i = 0; i < islandCount; i++)
        {
            int attempts = 0;
            const int maxAttempts = 50;
            Vector2 newCenter;

            do
            {
                newCenter = new Vector2(
                    Random.Range(borderSize + 5, width - borderSize - 5),
                    Random.Range(borderSize + 5, height - borderSize - 5)
                );

                attempts++;
                if (attempts > maxAttempts)
                {
                    Debug.LogWarning("Không thể tìm vị trí cho tất cả các đảo mà không chồng lấn!");
                    break;
                }
            }
            while (!IsValidCenter(newCenter, centers, minDistanceBetweenIslands));

            centers.Add(newCenter);
        }

        return centers;
    }

    bool IsValidCenter(Vector2 newCenter, List<Vector2> existingCenters, float minDistance)
    {
        foreach (Vector2 center in existingCenters)
        {
            if (Vector2.Distance(newCenter, center) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    void GenerateCurvedIsland(Vector2 center, float maxDistance)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < borderSize || x >= width - borderSize || y < borderSize || y >= height - borderSize)
                {
                    continue; // Đã đặt nước ở bước đầu, không cần ghi đè
                }

                float distanceFromCenter = Vector2.Distance(new Vector2(x, y), center);
                float normalizedDistance = distanceFromCenter / maxDistance;
                float perlinValue = Mathf.PerlinNoise(x / scale, y / scale);
                float combinedValue = perlinValue * (1 - normalizedDistance);

                if (distanceFromCenter > maxDistance || normalizedDistance > islandRadiusFactor || combinedValue < 0.3f)
                {
                    continue; // Để nước, không cần đặt lại
                }
                else if (combinedValue < 0.3f + sandWidth)
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Sand);
                }
                else
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Grass);
                }
            }
        }
    }

    void GenerateRectangularIsland(Vector2 center, float maxDistance)
    {
        int islandWidth = (int)(maxDistance * islandRadiusFactor * 2);
        int islandHeight = (int)(maxDistance * islandRadiusFactor * 2);
        int sandThickness = (int)(Mathf.Min(islandWidth, islandHeight) * sandWidth);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < borderSize || x >= width - borderSize || y < borderSize || y >= height - borderSize)
                {
                    continue;
                }

                int leftEdge = (int)center.x - islandWidth / 2;
                int rightEdge = (int)center.x + islandWidth / 2;
                int bottomEdge = (int)center.y - islandHeight / 2;
                int topEdge = (int)center.y + islandHeight / 2;

                if (x < leftEdge || x >= rightEdge || y < bottomEdge || y >= topEdge)
                {
                    continue;
                }
                else if (x < leftEdge + sandThickness || x >= rightEdge - sandThickness ||
                         y < bottomEdge + sandThickness || y >= topEdge - sandThickness)
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Sand);
                }
                else
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Grass);
                }
            }
        }
    }

    void GenerateCircularIsland(Vector2 center, float maxDistance)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < borderSize || x >= width - borderSize || y < borderSize || y >= height - borderSize)
                {
                    continue;
                }

                float distanceFromCenter = Vector2.Distance(new Vector2(x, y), center);
                float normalizedDistance = distanceFromCenter / maxDistance;

                if (distanceFromCenter > maxDistance || normalizedDistance > islandRadiusFactor)
                {
                    continue;
                }
                else if (normalizedDistance > islandRadiusFactor - sandWidth)
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Sand);
                }
                else
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Grass);
                }
            }
        }
    }

    void GenerateCrossIsland(Vector2 center, float maxDistance)
    {
        int crossWidth = (int)(maxDistance * islandRadiusFactor * 1.6f);
        int crossHeight = (int)(maxDistance * islandRadiusFactor * 1.6f);
        int armThickness = (int)(Mathf.Min(maxDistance, maxDistance) * 0.4f);
        int sandThickness = (int)(Mathf.Min(maxDistance, maxDistance) * sandWidth);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < borderSize || x >= width - borderSize || y < borderSize || y >= height - borderSize)
                {
                    continue;
                }

                bool isHorizontalArm = Mathf.Abs(x - center.x) < crossWidth / 2 && Mathf.Abs(y - center.y) < armThickness / 2;
                bool isVerticalArm = Mathf.Abs(y - center.y) < crossHeight / 2 && Mathf.Abs(x - center.x) < armThickness / 2;

                if (!isHorizontalArm && !isVerticalArm)
                {
                    continue;
                }
                else if (Mathf.Abs(x - center.x) > crossWidth / 2 - sandThickness ||
                         Mathf.Abs(y - center.y) > crossHeight / 2 - sandThickness)
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Sand);
                }
                else
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Grass);
                }
            }
        }
    }

    void GenerateIrregularIslandWithLake(Vector2 center, float maxDistance)
    {
        if (islandShapeMask != null)
        {
            GenerateIslandFromMask(center, maxDistance);
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x < borderSize || x >= width - borderSize || y < borderSize || y >= height - borderSize)
                    {
                        continue;
                    }

                    float distanceFromCenter = Vector2.Distance(new Vector2(x, y), center);
                    float normalizedDistance = distanceFromCenter / maxDistance;
                    float perlinValue = Mathf.PerlinNoise(x / scale, y / scale);
                    float combinedValue = perlinValue * (1 - normalizedDistance);

                    if (distanceFromCenter > maxDistance || normalizedDistance > islandRadiusFactor || combinedValue < 0.3f)
                    {
                        continue;
                    }
                    else if (combinedValue < 0.3f + sandWidth)
                    {
                        SetTile(new Vector3Int(x, y, 0), TileType.Sand);
                    }
                    else
                    {
                        SetTile(new Vector3Int(x, y, 0), TileType.Grass);
                    }
                }
            }
        }

        AddLake(center, maxDistance);
    }

    void GenerateIslandFromMask(Vector2 center, float maxDistance)
    {
        if (islandShapeMask == null)
        {
            Debug.LogWarning("Không có hình ảnh mặt nạ được cung cấp!");
            return;
        }

        int maskWidth = Mathf.Min(islandShapeMask.width, (int)(maxDistance * 2));
        int maskHeight = Mathf.Min(islandShapeMask.height, (int)(maxDistance * 2));

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float distanceFromCenter = Vector2.Distance(new Vector2(x, y), center);
                if (distanceFromCenter > maxDistance)
                {
                    continue;
                }

                int maskX = Mathf.RoundToInt((x - (center.x - maskWidth / 2f)) / (float)maskWidth * islandShapeMask.width);
                int maskY = Mathf.RoundToInt((y - (center.y - maskHeight / 2f)) / (float)maskHeight * islandShapeMask.height);

                maskX = Mathf.Clamp(maskX, 0, islandShapeMask.width - 1);
                maskY = Mathf.Clamp(maskY, 0, islandShapeMask.height - 1);

                Color pixelColor = islandShapeMask.GetPixel(maskX, maskY);
                float brightness = pixelColor.grayscale;

                if (x < borderSize || x >= width - borderSize || y < borderSize || y >= height - borderSize)
                {
                    continue;
                }
                else if (brightness < 0.3f)
                {
                    continue;
                }
                else if (brightness < 0.6f)
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Sand);
                }
                else
                {
                    SetTile(new Vector3Int(x, y, 0), TileType.Grass);
                }
            }
        }
    }

    void AddLake(Vector2 center, float maxDistance)
    {
        Vector2 lakeCenter = center + new Vector2(
            Random.Range(-maxDistance * 0.3f, maxDistance * 0.3f),
            Random.Range(-maxDistance * 0.3f, maxDistance * 0.3f)
        );

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                float distanceFromLakeCenter = Vector2.Distance(pos, lakeCenter);
                float lakePerlinValue = Mathf.PerlinNoise(x / lakeNoiseScale, y / lakeNoiseScale);
                float lakeThreshold = lakeRadius * (0.8f + lakePerlinValue * 0.4f);

                if (distanceFromLakeCenter < lakeThreshold)
                {
                    Tilemap grassTilemap = tilemapConfigs.Find(config => config.tileType == TileType.Grass)?.tilemap;
                    if (grassTilemap != null && grassTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        SetTile(new Vector3Int(x, y, 0), TileType.Lake);
                    }
                }
            }
        }
    }

    void SetTile(Vector3Int position, TileType tileType)
    {
        foreach (var config in tilemapConfigs)
        {
            if (config.tileType == tileType && config.tilemap != null)
            {
                TileBase tileToSet;
                if (config.useRandomTile && config.tileOptions != null && config.tileOptions.Count > 0)
                {
                    int tileIndex = Random.Range(0, config.tileOptions.Count);
                    tileToSet = config.tileOptions[tileIndex];
                }
                else
                {
                    tileToSet = config.specificTile;
                }

                if (tileToSet != null)
                {
                    config.tilemap.SetTile(position, tileToSet);
                }
                break;
            }
        }
    }

    void AddTreeTiles()
    {
        Tilemap grassTilemap = null;
        Tilemap treeTilemap = null;
        Tilemap lakeTilemap = null;

        foreach (var config in tilemapConfigs)
        {
            if (config.tileType == TileType.Grass)
            {
                grassTilemap = config.tilemap;
            }
            else if (config.tileType == TileType.Tree)
            {
                treeTilemap = config.tilemap;
            }
            else if (config.tileType == TileType.Lake)
            {
                lakeTilemap = config.tilemap;
            }
        }

        if (grassTilemap == null || treeTilemap == null)
        {
            Debug.LogWarning("Grass Tilemap hoặc Tree Tilemap chưa được gán!");
            return;
        }

        int placedTrees = 0;
        int attempts = 0;
        int maxAttempts = treeCount * islandCount;

        while (placedTrees < treeCount * islandCount && attempts < maxAttempts)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            Vector3Int tilePos = new Vector3Int(x, y, 0);

            if (grassTilemap.GetTile(tilePos) != null && (lakeTilemap == null || lakeTilemap.GetTile(tilePos) == null))
            {
                SetTile(tilePos, TileType.Tree);
                placedTrees++;
            }
            attempts++;
        }

        if (placedTrees < treeCount * islandCount)
        {
            Debug.LogWarning($"Chỉ đặt được {placedTrees}/{treeCount * islandCount} cây do không đủ ô cỏ.");
        }
    }

    [ContextMenu("Generate New Island")]
    void GenerateNewIsland()
    {
        GenerateIsland();
    }
}