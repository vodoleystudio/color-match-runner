using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using AudioSettings = HyperCasual.Core.AudioSettings;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Handles playing sounds and music based on their sound ID
    /// </summary>
    public class AudioManager : AbstractSingleton<AudioManager>
    {
        [Serializable]
        private class SoundIDClipPair
        {
            public SoundID m_SoundID;
            public AudioClip m_AudioClip;
        }

        [SerializeField]
        private AudioSource m_MusicSource;

        [SerializeField]
        private AudioSource m_MusicEffectSource;

        [SerializeField]
        private AudioSource m_EffectSource;

        [SerializeField, Min(0f)]
        private float m_MinSoundInterval = 0.1f;

        [SerializeField]
        private SoundIDClipPair[] m_Sounds;

        private float m_LastSoundPlayTime;
        private readonly Dictionary<SoundID, AudioClip> m_Clips = new();

        private AudioSettings m_AudioSettings = new();

        /// <summary>
        /// Unmute/mute the music
        /// </summary>
        public bool EnableMusic
        {
            get => m_AudioSettings.EnableMusic;
            set
            {
                m_AudioSettings.EnableMusic = value;
                m_MusicSource.mute = !value;
            }
        }

        /// <summary>
        /// Unmute/mute all sound effects
        /// </summary>
        public bool EnableSfx
        {
            get => m_AudioSettings.EnableSfx;
            set
            {
                m_AudioSettings.EnableSfx = value;
                m_EffectSource.mute = !value;
            }
        }

        /// <summary>
        /// The Master volume of the audio listener
        /// </summary>
        public float MasterVolume
        {
            get => m_AudioSettings.MasterVolume;
            set
            {
                m_AudioSettings.MasterVolume = value;
                AudioListener.volume = value;
            }
        }

        private void Start()
        {
            foreach (var sound in m_Sounds)
            {
                m_Clips.Add(sound.m_SoundID, sound.m_AudioClip);
            }
        }

        private void OnEnable()
        {
            if (SaveManager.Instance == null)
            {
                // Disable music, enable sfx, and
                // set volume to a very low amount
                // in the LevelEditor
                EnableMusic = false;
                EnableSfx = true;
                MasterVolume = 0.2f;
                return;
            }

            var audioSettings = SaveManager.Instance.LoadAudioSettings();
            EnableMusic = audioSettings.EnableMusic;
            EnableSfx = audioSettings.EnableSfx;
            MasterVolume = audioSettings.MasterVolume;
        }

        private void OnDisable()
        {
            if (SaveManager.Instance == null)
            {
                return;
            }

            SaveManager.Instance.SaveAudioSettings(m_AudioSettings);
        }

        private void PlayMusic(AudioSource audioSource, AudioClip audioClip, bool looping = true)
        {
            if (audioSource.isPlaying)
                return;

            audioSource.clip = audioClip;
            audioSource.loop = looping;
            audioSource.Play();
        }

        /// <summary>
        /// Play a music based on its sound ID
        /// </summary>
        /// <param name="soundID">The ID of the music</param>
        /// <param name="looping">Is music looping?</param>
        public void PlayMusic(SoundID soundID, bool looping = true)
        {
            PlayMusic(m_MusicSource, m_Clips[soundID], looping);
        }

        /// <summary>
        /// Stop the current music
        /// </summary>
        public void StopMusic()
        {
            m_MusicSource.Stop();
        }

        /// <summary>
        /// Play a music based on its sound ID
        /// </summary>
        /// <param name="soundID">The ID of the music</param>
        /// <param name="looping">Is music looping?</param>
        public void PlayMusicEffect(SoundID soundID, bool looping = true)
        {
            PlayMusic(m_MusicEffectSource, m_Clips[soundID], looping);
        }

        /// <summary>
        /// Stop the current music
        /// </summary>
        public void StopMusicEffect()
        {
            m_MusicEffectSource.Stop();
        }

        private void PlayEffect(AudioClip audioClip)
        {
            if (Time.time - m_LastSoundPlayTime >= m_MinSoundInterval)
            {
                m_EffectSource.PlayOneShot(audioClip);
                m_LastSoundPlayTime = Time.time;
            }
        }

        /// <summary>
        /// Play a sound effect based on its sound ID
        /// </summary>
        /// <param name="soundID">The ID of the sound effect</param>
        public void PlayEffect(SoundID soundID)
        {
            if (soundID == SoundID.None)
                return;

            PlayEffect(m_Clips[soundID]);
        }
    }
}