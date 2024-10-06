using Game.Scripts.Config;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Util
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource BackgroundMusic;
        public AudioSource Effects;

        private GameConfiguration _gameConfiguration;
        
        [Inject]
        public void Construct(GameConfiguration gameConfiguration)
        {
            _gameConfiguration = gameConfiguration;
        }
        
        private void PlaySFX(AudioClip clip, float pitch = 1f)
        {
            Effects.pitch = pitch;
            Effects.PlayOneShot(clip);
        }
        
        public void PlaySwapWooAudio()
        {
            float pitch = Random.Range(0.7f, 1.1f);
            PlaySFX(_gameConfiguration.SwapWooAudioClip, pitch);
        }
        
        public void PlayExplosionAudio()
        {
            PlaySFX(_gameConfiguration.ExplosionAudioClip);
        }
    }
}