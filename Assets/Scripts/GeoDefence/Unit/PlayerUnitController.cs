using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GeoDefence.Unit
{
    public class PlayerUnitController : MonoBehaviour
    {
        private const int MAX_LEVEL = 4;
        
        [SerializeField] private Sprite[] baseSprites;
        [SerializeField] private SpriteRenderer baseSpriteRenderer;
        [SerializeField] private SpriteRenderer topSpriteRenderer;

        private PlayerUnitProperties properties;
        
        private int currentLevel = 1;

        public void SetProperty(PlayerUnitProperties _properties)
        {
            properties = _properties;
            UpdateSpriteBase();
            topSpriteRenderer.sprite = properties.TurretSprite;
            AddLevel(Random.Range(0, MAX_LEVEL));
        }

        public void AddLevel(int _amt)
        {
            currentLevel = Math.Min(currentLevel + _amt, MAX_LEVEL);
            UpdateSpriteBase();
        }

        private void UpdateSpriteBase()
        {
            baseSpriteRenderer.sprite = baseSprites[currentLevel - 1];
        }
    }
}