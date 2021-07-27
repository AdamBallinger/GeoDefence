using GeoDefence.Map;
using UnityEngine;

namespace GeoDefence.Unit
{
    public class EnemyUnitController : MonoBehaviour
    {
        [SerializeField] private EnemyUnitProperties unitProperties;

        [SerializeField] private SpriteRenderer unitSpriteRenderer;

        private MapManager mapManager;

        // Index of path the unit is currently at.
        private int currentPathIndex;
        
        // Index of path the unit is moving towards.
        private int nextPathIndex = 1;

        /// <summary>
        /// Curve controlling how the unit moves from to the next point in the path.
        /// </summary>
        private AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private float t;

        // Cache transform reference.
        private Transform _transform;

        private void Start()
        {
            mapManager = FindObjectOfType<MapManager>();
            _transform = transform;
        }

        private void Update()
        {
            var currentPos = mapManager.GetPathWorldPosition(currentPathIndex);
            var targetPos = mapManager.GetPathWorldPosition(nextPathIndex);
            currentPos.x += 0.5f;
            currentPos.y += 0.5f;
            targetPos.x += 0.5f;
            targetPos.y += 0.5f;

            t += Time.deltaTime / (Vector3.Distance(currentPos, targetPos) / unitProperties.BaseMovementSpeed);

            _transform.position = Vector3.Lerp(currentPos, targetPos, movementCurve.Evaluate(t));

            if (t >= 1.0f)
            {
                t = 0.0f;
                currentPathIndex = nextPathIndex;
                nextPathIndex++;

                if (nextPathIndex >= mapManager.PathData.CellPositions.Count)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}