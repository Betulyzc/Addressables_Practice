using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressablesManagement : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button warriorButton;
    [SerializeField] Button archerButton;

    [Header("Panels")]
    [SerializeField] GameObject charactersPanel;

    [Header("Addressables")]
    [SerializeField] AssetReferenceGameObject forestEnviromentPrefab_addressable;
    [SerializeField] AssetReferenceGameObject desertHouseEnviromentPrefab_addressable;


    void Start()
    {
        charactersPanel.SetActive(true);
        warriorButton.onClick.AddListener(() => LoadEnvAsset(forestEnviromentPrefab_addressable));
        archerButton.onClick.AddListener(() => LoadEnvAsset(desertHouseEnviromentPrefab_addressable));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEnvAsset(AssetReferenceGameObject referenceGameObject)
    {
        charactersPanel.SetActive(false);

        referenceGameObject.LoadAssetAsync<GameObject>().Completed +=
            (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Instantiate(asyncOperationHandle.Result);

                }
                else {
                    Debug.Log("Error");
                
                }

            };
        


    }




}
