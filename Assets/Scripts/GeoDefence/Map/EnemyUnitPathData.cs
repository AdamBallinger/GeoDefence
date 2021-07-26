using System.Collections.Generic;
using UnityEngine;

namespace GeoDefence.Map
{
    [CreateAssetMenu(fileName = "PathData_", menuName = "GeoDefence/Path Data")]
    public class EnemyUnitPathData : ScriptableObject
    {
        [SerializeField] private List<Vector3Int> cellPositions;

        public List<Vector3Int> CellPositions => cellPositions;

        /// <summary>
        /// Copy given positions into self.
        /// </summary>
        /// <param name="_positions"></param>
        public void SetPosition(List<Vector3Int> _positions)
        {
            cellPositions = new List<Vector3Int>(_positions.Count);
            foreach (var pos in _positions)
            {
                cellPositions.Add(pos);
            }
        }
    }
}