using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using GameCore.Data;
using AudioSettings = HyperCasual.Core.AudioSettings;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A simple class used to save a load values
    /// using PlayerPrefs.
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the SaveManager.
        /// </summary>
        public static SaveManager Instance => s_Instance;

        private static SaveManager s_Instance;

        private const string k_LevelProgress = "LevelProgress";
        private const string k_Currency = "Currency";
        private const string k_Xp = "Xp";
        private const string k_AudioSettings = "AudioSettings";
        private const string k_QualityLevel = "QualityLevel";

        private void Awake()
        {
            s_Instance = this;
        }

        /// <summary>
        /// Save and load level progress as an integer
        /// </summary>
        public int LevelProgress
        {
            get => PlayerPrefs.GetInt(k_LevelProgress);
            set => PlayerPrefs.SetInt(k_LevelProgress, value);
        }

        /// <summary>
        /// Save and load currency as an integer
        /// </summary>
        public int Currency
        {
            get => PlayerPrefs.GetInt(k_Currency);
            set => PlayerPrefs.SetInt(k_Currency, value);
        }

        public float XP
        {
            get => PlayerPrefs.GetFloat(k_Xp);
            set => PlayerPrefs.SetFloat(k_Xp, value);
        }

        public bool IsQualityLevelSaved => PlayerPrefs.HasKey(k_QualityLevel);

        public int QualityLevel
        {
            get => PlayerPrefs.GetInt(k_QualityLevel);
            set => PlayerPrefs.SetInt(k_QualityLevel, value);
        }

        public AudioSettings LoadAudioSettings()
        {
            return PlayerPrefsUtils.Read<AudioSettings>(k_AudioSettings);
        }

        public void SaveAudioSettings(AudioSettings audioSettings)
        {
            PlayerPrefsUtils.Write(k_AudioSettings, audioSettings);
        }

        public void SaveLevelData(string key, LevelData levelData)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(levelData));
        }

        public LevelData GetLevelData(string key)
        {
            return JsonUtility.FromJson<LevelData>(PlayerPrefs.GetString(key));
        }
    }
}