using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GeoDefence.Map
{
    [CreateAssetMenu(fileName="Tile_", menuName="GeoDefence/Tile Property")]
    public class TileProperties : ScriptableObject
    {
        [SerializeField] private List<TileBase> tiles;
        [SerializeField] private TileType type;

        public List<TileBase> Tiles => tiles;
        public TileType Type => type;
    }
}
