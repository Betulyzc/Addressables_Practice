using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Loads environment and enemy prefabs via Addressables.
/// Finds spawn points and instantiates assets dynamically.
/// </summary>
public class AddressablesManager : MonoBehaviour
{
    [Header("Environment Prefabs")]
    [SerializeField] private AssetReferenceGameObject forestEnvironmentPrefab;
    [SerializeField] private AssetReferenceGameObject desertEnvironmentPrefab;

    [Header("Enemy Prefabs")]
    [SerializeField] private AssetReferenceGameObject enemyForestPrefab;
    [SerializeField] private AssetReferenceGameObject enemyDesertPrefab;

    private Dictionary<string, AssetReferenceGameObject> envDict;
    private Dictionary<string, AssetReferenceGameObject> enemyDict;

    private Transform currentEnemySpawnPoint;
    private GameObject currentEnemyInstance;


    void Awake()
    {
        envDict = new()
        {
            { "Forest", forestEnvironmentPrefab },
            { "Desert", desertEnvironmentPrefab }
        };

        enemyDict = new()
        {
            { "Forest", enemyForestPrefab },
            { "Desert", enemyDesertPrefab }
        };
    }

    /// <summary>
    /// Loads the selected environment and finds spawn point.
    /// </summary>
    public void LoadEnvironment(string environmentKey, System.Action onLoadedCallback)
    {
        if (!envDict.ContainsKey(environmentKey))
        {
            Debug.LogWarning($"No environment found for key: {environmentKey}");
            return;
        }

        envDict[environmentKey].InstantiateAsync().Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject envInstance = handle.Result;

                Transform spawn = envInstance.transform.Find("EnemySpawnPoint");
                if (spawn != null)
                {
                    currentEnemySpawnPoint = spawn;
                    Debug.Log("Enemy spawn point found.");
                }
                else
                {
                    Debug.LogWarning("Enemy spawn point not found in environment.");
                }

                onLoadedCallback?.Invoke();
            }
            else
            {
                Debug.LogError("Failed to load environment.");
            }
        };
    }

    /// <summary>
    /// Loads and spawns the enemy at the previously found spawn point.
    /// </summary>
    public void LoadEnemy(string environmentKey)
    {
        if (!enemyDict.ContainsKey(environmentKey))
        {
            Debug.LogWarning($"No enemy found for key: {environmentKey}");
            return;
        }

        if (currentEnemySpawnPoint == null)
        {
            Debug.LogWarning("Enemy spawn point is not set.");
            return;
        }

        enemyDict[environmentKey].InstantiateAsync(currentEnemySpawnPoint.position, Quaternion.identity).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                currentEnemyInstance = handle.Result;
                Debug.Log("Enemy spawned successfully.");
            }
            else
            {
                Debug.LogError("Enemy failed to load.");
            }
        };
    }

}
