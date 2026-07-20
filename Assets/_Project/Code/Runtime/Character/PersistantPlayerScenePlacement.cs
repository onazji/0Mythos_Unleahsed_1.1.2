using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.TopDownEngine;

[DisallowMultipleComponent]
public class PersistentPlayerScenePlacement : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PlacePlayerAfterSceneInitialization());
    }

    private IEnumerator PlacePlayerAfterSceneInitialization()
    {
        // Allow the scene's LevelManager and CheckPoints to initialize first.
        yield return null;

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();

        if (levelManager == null)
        {
            Debug.LogWarning(
                $"No LevelManager found in scene {SceneManager.GetActiveScene().name}.",
                this
            );
            yield break;
        }

        CheckPoint targetCheckPoint = levelManager.InitialSpawnPoint;

        if (targetCheckPoint == null)
        {
            Debug.LogWarning(
                $"LevelManager in scene {SceneManager.GetActiveScene().name} has no Initial Spawn Point.",
                levelManager
            );
            yield break;
        }

        transform.SetPositionAndRotation(
            targetCheckPoint.transform.position,
            targetCheckPoint.transform.rotation
        );

        Debug.Log(
            $"Persistent player placed at checkpoint {targetCheckPoint.name} " +
            $"in scene {SceneManager.GetActiveScene().name}.",
            this
        );
    }
}