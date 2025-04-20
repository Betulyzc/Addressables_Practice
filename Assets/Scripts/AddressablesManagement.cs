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

    private Dictionary<EnvironmentType, AssetReferenceGameObject> envDict;
    private Dictionary<EnvironmentType, AssetReferenceGameObject> enemyDict;

    private Transform currentEnemySpawnPoint;
    private GameObject currentEnemyInstance;


    void Awake()
    {
        envDict = new()
        {
            { EnvironmentType.Forest, forestEnvironmentPrefab },
            { EnvironmentType.Desert, desertEnvironmentPrefab }
        };

        enemyDict = new()
        {
            { EnvironmentType.Forest, enemyForestPrefab },
            { EnvironmentType.Desert, enemyDesertPrefab }
        };
    }

    /// <summary>
    /// Loads the selected environment and finds spawn point.
    /// </summary>
    public void LoadEnvironment(EnvironmentType environmentType, System.Action onLoadedCallback)
    {
        if (!envDict.ContainsKey(environmentType))
        {
            Debug.LogWarning($"No environment found for key: {environmentType}");
            return;
        }

        envDict[environmentType].InstantiateAsync().Completed += (handle) =>
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
    public void LoadEnemy(EnvironmentType environmentType)
    {
        if (!enemyDict.ContainsKey(environmentType))
        {
            Debug.LogWarning($"No enemy found for key: {environmentType}");
            return;
        }

        if (currentEnemySpawnPoint == null)
        {
            Debug.LogWarning("Enemy spawn point is not set.");
            return;
        }

        enemyDict[environmentType].InstantiateAsync(currentEnemySpawnPoint.position, Quaternion.identity).Completed += (handle) =>
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
public enum EnvironmentType
{
    Forest,
    Desert

}


