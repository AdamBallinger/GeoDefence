using System.Collections.Generic;
using System.Linq;
using GeoDefence.Map;
using GeoDefence.Unit;
using UnityEngine;

namespace GeoDefence
{
    public class BuildableManager : MonoBehaviour
    {
        [SerializeField] private GameObject unitPrefab;
        [SerializeField] private PlayerUnitProperties[] unitProperties;

        private MapManager mapManager;

        private List<Vector3> buildablePositions = new List<Vector3>();

        /// <summary>
        /// Maps which buildable slot positions (World position) are occupied by a unit.
        /// </summary>
        private Dictionary<Vector3, bool> placementMap = new Dictionary<Vector3, bool>();

        private void Start()
        {
            mapManager = FindObjectOfType<MapManager>();

            foreach (var pos in mapManager.GetBuildablePositions())
            {
                placementMap.Add(pos, false);
                buildablePositions.Add(pos);
            }

            TestSpawn();
        }

        private void TestSpawn()
        {
            for (var i = 0; i < 10; ++i)
            {
                var positions = placementMap.Where(p => !p.Value)
                    .Select(p => p.Key).ToList();
                
                SpawnUnit(positions[Random.Range(0, positions.Count)]);
            }
        }

        /// <summary>
        /// Creates a new random unit at given world position.
        /// </summary>
        /// <param name="_worldPos"></param>
        private void SpawnUnit(Vector3 _worldPos)
        {
            var unit = Instantiate(unitPrefab, _worldPos, Quaternion.identity).GetComponent<PlayerUnitController>();
            unit.SetProperty(unitProperties[0]);
            placementMap[_worldPos] = true;
        }
    }
}