using DG.Tweening;
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
        
        public void PlayNormalBackgroundMusic()
        {
            BackgroundMusic.clip = _gameConfiguration.BackgroundMusicClip;
            BackgroundMusic.DOFade(1f, 0.5f).From(0f).SetEase(Ease.InOutSine).SetAutoKill(true);
            BackgroundMusic.Play();
        }
        
        public void PlayReversedBackgroundMusic()
        {
            BackgroundMusic.clip = _gameConfiguration.ReversedBackgroundMusicClip;
            BackgroundMusic.DOFade(1f, 0.2f).From(0f).SetEase(Ease.InOutSine).SetAutoKill(true);
            BackgroundMusic.Play();
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

        public void PlaySqueeze()
        {
            float pitch = Random.Range(0.5f, 1.1f);
            PlaySFX(_gameConfiguration.SqueezeAudioClip, pitch);
        }
        
        public void PlayExplosionAudio()
        {
            PlaySFX(_gameConfiguration.ExplosionAudioClip);
        }
    }
}