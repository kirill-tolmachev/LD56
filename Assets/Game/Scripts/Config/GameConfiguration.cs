using Game.Scripts.Entities;
using Game.Scripts.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Config
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/GameConfiguration")]
    public class GameConfiguration : ScriptableObject
    {
        public float PlayerSpeed = 20f;
        public float PlayerRadius = 0.5f;
        public float OutTubeShotInterval;
        public float ProjectileSpeed = 10f;
        public float WooSizeScoreMultiplier = 0.3f;
        
        [Header("Prefabs")]
        public Projectile ProjectilePrefab;
        public ProjectileBBox ProjectileBBoxPrefab;
        
        public Woo WooSquare;
        public Woo WooSquareRed;
        public Woo WooCircle;
        
        [Header("Particles")]
        public ParticleSystem WooDestroyedParticlePrefab;
        public ParticleSystem PressSqueezeParticlePrefab;
        public ParticleSystem WooCreatedParticlePrefab;
    }
}