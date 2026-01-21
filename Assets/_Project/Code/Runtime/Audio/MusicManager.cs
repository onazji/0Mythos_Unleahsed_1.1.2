using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Mythos.Unleashed.Runtime.Audio
{
    /// <summary>
    /// Handles dynamic scene-based music playback.
    /// Crossfades between tracks and persists across scenes.
    /// Automatically mutes during loading screens and skips Bootstrap.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        [Header("Music Library")]
        [Tooltip("All available music clips for rotation.")]
        [SerializeField] private List<AudioClip> mainMenuTracks = new();
        [SerializeField] private List<AudioClip> museumTracks = new();
        [SerializeField] private List<AudioClip> mazeTracks = new();

        [Header("Settings")]
        [Range(0.1f, 1f)] public float targetVolume = 0.35f;
        [Range(1f, 5f)] public float fadeDuration = 2f;

        private AudioSource _source;
        private AudioClip _currentTrack;
        private Coroutine _fadeRoutine;
        private string _currentSceneName = "";
        private readonly HashSet<AudioClip> _playedTracks = new();

        private void Awake()
        {
            // Enforce singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize AudioSource
            _source = GetComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.loop = false;
            _source.spatialBlend = 0f; // 2D
            _source.volume = 0f;

            // Ensure there’s always at least one AudioListener in the scene
            if (FindObjectOfType<AudioListener>() == null)
            {
                gameObject.AddComponent<AudioListener>();
                Debug.Log("[MusicManager] No AudioListener detected. Added one automatically.");
            }

            // Subscribe to scene load events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _currentSceneName = scene.name;

            // 🔇 Skip Bootstrap and mute during Loading scenes
            if (_currentSceneName.Contains("Boot"))
            {
                _source.Stop();
                _source.volume = 0f;
                Debug.Log("[MusicManager] Skipping Bootstrap scene.");
                return;
            }

            if (_currentSceneName.Contains("Loading"))
            {
                _source.Stop();
                _source.volume = 0f;
                Debug.Log("[MusicManager] Muted during loading screen.");
                return;
            }

            // Otherwise, pick the proper track
            PickTrackForScene(_currentSceneName);
        }

        private void PickTrackForScene(string sceneName)
        {
            List<AudioClip> pool = null;

            if (sceneName.Contains("MainMenu"))
                pool = mainMenuTracks;
            else if (sceneName.Contains("Museum"))
                pool = museumTracks;
            else if (sceneName.Contains("Maze"))
                pool = mazeTracks;

            if (pool == null || pool.Count == 0)
            {
                // Skip warning if it’s just Bootstrap or Loading
                if (!sceneName.Contains("Boot") && !sceneName.Contains("Loading"))
                    Debug.LogWarning($"[MusicManager] No tracks assigned for scene: {sceneName}");
                return;
            }

            AudioClip nextTrack = GetNextTrack(pool);
            if (nextTrack == null)
                return;

            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(CrossfadeTo(nextTrack));
        }

        private AudioClip GetNextTrack(List<AudioClip> pool)
        {
            // Reset rotation when all tracks have been played
            if (_playedTracks.Count >= pool.Count)
                _playedTracks.Clear();

            // Pick a random unplayed track
            AudioClip next = null;
            int safety = 10;

            while (next == null && safety-- > 0)
            {
                var candidate = pool[Random.Range(0, pool.Count)];
                if (!_playedTracks.Contains(candidate))
                {
                    _playedTracks.Add(candidate);
                    next = candidate;
                }
            }

            return next ?? pool[Random.Range(0, pool.Count)];
        }

        private IEnumerator CrossfadeTo(AudioClip newClip)
        {
            if (newClip == null || newClip == _currentTrack)
                yield break;

            _currentTrack = newClip;

            float fadeOutTime = fadeDuration * 0.5f;

            // Fade out old
            float startVol = _source.volume;
            float t = 0f;
            while (t < fadeOutTime)
            {
                t += Time.unscaledDeltaTime;
                _source.volume = Mathf.Lerp(startVol, 0f, t / fadeOutTime);
                yield return null;
            }

            // Switch clip and fade in new
            _source.clip = newClip;
            _source.Play();

            t = 0f;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                _source.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
                yield return null;
            }

            _source.volume = targetVolume;
            Debug.Log($"[MusicManager] Now playing: {newClip.name}");
        }
    }
}
