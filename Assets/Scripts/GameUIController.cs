using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

/// <summary>
/// Handles UI interactions and passes selections to the AddressablesManager.
/// </summary>
public class GameUIController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button warriorButton;
    [SerializeField] private Button archerButton;
    [SerializeField] private Button letsFightButton;
    [SerializeField] private Button backToOptions;

    [Header("Panels")]
    [SerializeField] private GameObject charactersPanel;
    [SerializeField] private GameObject choiceGameplay;

    private EnvironmentType? selectedEnvironment;
    private AddressablesManager addressablesManager;

    [Header("Loading UI")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingSliderText;
    void Start()
    {
        addressablesManager = GetComponent<AddressablesManager>();

        charactersPanel.SetActive(true);
        choiceGameplay.SetActive(false);
        loadingPanel.SetActive(false);

        warriorButton.onClick.AddListener(() => OnCharacterSelected(EnvironmentType.Forest));
        archerButton.onClick.AddListener(() => OnCharacterSelected(EnvironmentType.Desert));

        backToOptions.onClick.AddListener(BackOptions);



        letsFightButton.onClick.AddListener(() =>
        {
            if (selectedEnvironment.HasValue)
            {
                addressablesManager.LoadEnemy(selectedEnvironment.Value);
                letsFightButton.interactable = false;
            }
            else
            {
                Debug.LogWarning("No environment selected.");
            }
        });
    }

    private void OnCharacterSelected(EnvironmentType environmentType)
    {
        selectedEnvironment = environmentType;

        loadingPanel.SetActive(true);
        loadingSlider.value = 0;

        addressablesManager.LoadEnvironment(environmentType, (progress) => {
            loadingSlider.value = progress;
            loadingSliderText.text = "Loading %" + Mathf.RoundToInt(progress * 100);
        },
        ()=>
        {
            loadingPanel.SetActive(false);
            charactersPanel.SetActive(false);
            choiceGameplay.SetActive(true);
        });
    }


    public void BackOptions() { 
    
        charactersPanel.SetActive(true);
        choiceGameplay.SetActive(false);
        letsFightButton.interactable = true;
        addressablesManager.ReleaseSpawnedObjects();
        StartCoroutine(CleanupMemory());
    }

    IEnumerator CleanupMemory()
    {
        
        yield return Resources.UnloadUnusedAssets();
        yield return null; 
       
    }


}
