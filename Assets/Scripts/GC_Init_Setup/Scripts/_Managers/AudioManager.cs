
using System.Collections.Generic;
using UnityEngine;

namespace GeniusCrate.Utility
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviourSingletonPersistent<AudioManager>
    {
        protected AudioSource mAudioSource;
        [SerializeField] protected List<AudioClip> mMusicsList = new List<AudioClip>();
        [SerializeField] protected List<SoundEffect> mSoundEffectsList = new List<SoundEffect>(6);
        protected int mCurrentMusicIndex;
        protected bool IsSoundEffectsOn;
        protected bool IsMusicOn;
        public virtual void OnEnable()
        {
            SettingsManager.OnMusicValueChange += SetMusic;
            SettingsManager.OnSoundValueChange += SetSoundEffect;
            GameManager.OnGameStart += PlayCurrentMusic;
            GameManager.OnGameReEnter += PlayCurrentMusic;
            GameManager.OnGameOver += StopMusic;


            mAudioSource = GetComponent<AudioSource>();

            mAudioSource.volume = PlayerPrefs.GetFloat("Volume", .5f);
            bool musicState = PlayerPrefs.GetInt("MusicState", 1) == 1;
            IsSoundEffectsOn = PlayerPrefs.GetInt("SoundEffectState", 1) == 1;

            SetMusic(musicState);
        }


        public virtual void SetVolume(float volume)
        {
            mAudioSource.volume = volume;
            PlayerPrefs.SetFloat("Volume", volume);
        }
        public virtual void SetSoundEffect(bool on)
        {
            IsSoundEffectsOn = on;
            Debug.Log(this.name + "Sound To " + on);

            PlayerPrefs.SetInt("SoundEffectState", on ? 1 : 0);
        }
        public virtual void SetMusic(bool on)
        {
            if (mMusicsList.Count == 0) return;
            PlayerPrefs.SetInt("MusicState", on ? 1 : 0);
            IsMusicOn = on;
            if (on)
                PlayCurrentMusic();
            else
                mAudioSource.Stop();

        }
        public virtual void ChangeMusic(int musicIndex)
        {
            mCurrentMusicIndex = musicIndex;
            PlayCurrentMusic();
        }

        public virtual void PlaySoundOfType(SoundEffectType soundType)
        {
            if (!IsSoundEffectsOn) return;
            mAudioSource.PlayOneShot(GetSoundOfType(soundType));
        }


        AudioClip GetSoundOfType(SoundEffectType soundType)
        {
            foreach (var _soundEffect in mSoundEffectsList)
            {
                if (_soundEffect.type == soundType) return _soundEffect.clip;
            }
            Debug.LogError("Can't Find Sound Effect With Type " + soundType.ToString());
            return null;
        }
        void PlayCurrentMusic()
        {
            if (!IsMusicOn || !GameManager.Instance.isGameStarted) return;
            mAudioSource.clip = mMusicsList[mCurrentMusicIndex];
            mAudioSource.Play();
        }
        void StopMusic()
        {
            mAudioSource.Stop();
        }

        private void OnValidate()
        {
            foreach (var Sfx in mSoundEffectsList)
            {
                Sfx.sfxName = Sfx.type.ToString();
            }
        }

        public virtual void OnDisable()
        {
            SettingsManager.OnMusicValueChange -= SetMusic;
            SettingsManager.OnSoundValueChange -= SetSoundEffect;
            GameManager.OnGameStart -= PlayCurrentMusic;
            GameManager.OnGameOver -= StopMusic;
            GameManager.OnGameReEnter -= PlayCurrentMusic;


        }
    }
    [System.Serializable]
    public class SoundEffect
    {
        [HideInInspector]
        public string sfxName;
        public SoundEffectType type;
        public AudioClip clip;
    }
}

