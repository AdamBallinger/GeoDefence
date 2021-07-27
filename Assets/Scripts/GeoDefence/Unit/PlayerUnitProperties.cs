using UnityEngine;

namespace GeoDefence.Unit
{
    [CreateAssetMenu(fileName="Unit_Player_", menuName="GeoDefence/Player Unit Property")]
    public class PlayerUnitProperties : ScriptableObject
    {
        [SerializeField] private string unitName;
        [SerializeField] [Tooltip("Attacks per second")] private float baseAttackSpeed;
        [SerializeField] private float baseDamage;

        [SerializeField] private Sprite turretSprite;

        /// <summary>
        /// Name of the unit.
        /// </summary>
        public string UnitName => unitName;
        
        /// <summary>
        /// Base attack speed of unit (Attacks per second).
        /// </summary>
        public float BaseAttackSpeed => baseAttackSpeed;
        
        /// <summary>
        /// Base damage per attack of unit.
        /// </summary>
        public float BaseDamage => baseDamage;

        /// <summary>
        /// Sprite used to represent the "shooting" part of the unit.
        /// </summary>
        public Sprite TurretSprite => turretSprite;
    }
}