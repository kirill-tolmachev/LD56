using Game.Scripts.Entities;
using Game.Scripts.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Config
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/GameConfiguration")]
    public class GameConfiguration : ScriptableObject
    {
        public float WooSizeScoreMultiplier = 0.3f;
        public Narrators Narrators;
        
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
        public ParticleSystem BigExplosionParticlePrefab;

        [Header("Audio")] 
        public AudioClip BackgroundMusicClip;
        public AudioClip ReversedBackgroundMusicClip;
        
        public AudioClip SwapWooAudioClip;
        public AudioClip ExplosionAudioClip;
    }
}