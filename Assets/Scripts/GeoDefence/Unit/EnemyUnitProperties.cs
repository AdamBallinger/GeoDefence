using UnityEngine;

namespace GeoDefence.Unit
{
    [CreateAssetMenu(fileName="Unit_Enemy_", menuName="GeoDefence/Enemy Unit Property")]
    public class EnemyUnitProperties : ScriptableObject
    {
        [SerializeField] private float baseMovementSpeed;
        [SerializeField] private float baseHealth;
        [SerializeField] private float baseArmor;

        /// <summary>
        /// Base speed the unit moves at.
        /// </summary>
        public float BaseMovementSpeed => baseMovementSpeed;
        
        /// <summary>
        /// The base max health of the unit.
        /// </summary>
        public float BaseHealth => baseHealth;
        
        /// <summary>
        /// The base armor of the unit.
        /// </summary>
        public float BaseArmor => baseArmor;
    }
}