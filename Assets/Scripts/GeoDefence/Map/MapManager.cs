using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GeoDefence.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap tileMap;
        
        [SerializeField] private TileProperties[] tileProperties;

        [SerializeField] private EnemyUnitPathData pathData;

        public EnemyUnitPathData PathData => pathData;

        /// <summary>
        /// Map for each tilemap position (cell space) and its tile properties.
        /// </summary>
        private Dictionary<Vector3Int, TileProperties> tilePropertyMap;

        private void Awake()
        {
            GeneratePropertyMap();
        }

        /// <summary>
        /// Builds the lookup table for each position in the tilemap to retrieve tile properties.
        /// </summary>
        public void GeneratePropertyMap(bool _forceRebuild = false)
        {
            if (tilePropertyMap != null && !_forceRebuild) return;

            tilePropertyMap = new Dictionary<Vector3Int, TileProperties>();

            var mapBounds = tileMap.cellBounds;
            var tiles = tileMap.GetTilesBlock(mapBounds);

            for (var x = 0; x < mapBounds.size.x; x++)
            {
                for (var y = 0; y < mapBounds.size.y; y++)
                {
                    foreach (var property in tileProperties)
                    {
                        var tileBase = tiles[x + y * mapBounds.size.x];
                        if (property.Tiles.Contains(tileBase))
                        {
                            tilePropertyMap.Add(new Vector3Int(mapBounds.x + x, mapBounds.y + y, mapBounds.z), property);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the world space position of a random tile matching the provided type.
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public Vector3 GetRandomTileOfType(TileType _type)
        {
            var set = (from pair in tilePropertyMap where pair.Value.Type == _type select pair.Key).ToList();
            return set[Random.Range(0, set.Count)];
        }

        /// <summary>
        /// Returns the property for the tile at the given world position.
        /// </summary>
        /// <param name="_worldPos"></param>
        /// <returns></returns>
        public TileProperties GetTileProperty(Vector3 _worldPos)
        {
            return GetTileProperty(WorldToCell(_worldPos));
        }

        /// <summary>
        /// Returns the property for the tile at the given cell position.
        /// </summary>
        /// <param name="_cellPos"></param>
        /// <returns></returns>
        public TileProperties GetTileProperty(Vector3Int _cellPos)
        {
            return tilePropertyMap.TryGetValue(_cellPos, out var property) ? property : null;
        }

        public Vector3Int WorldToCell(Vector3 _worldPos)
        {
            return tileMap.WorldToCell(_worldPos);
        }

        public Vector3 CellToWorld(Vector3Int _cellPosition)
        {
            return tileMap.CellToWorld(_cellPosition);
        }

        public Vector3 GetPathWorldPosition(int _index)
        {
            return CellToWorld(PathData.CellPositions[_index]);
        }

        public List<Vector3> GetBuildablePositions()
        {
            return (from pair in tilePropertyMap where pair.Value.Type == TileType.Buildable select CellToWorld(pair.Key) + Vector3.one * 0.5f).ToList();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log(GetTileProperty(mousePos));
            }
        }
    }
}
