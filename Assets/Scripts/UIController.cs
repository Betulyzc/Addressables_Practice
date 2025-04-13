using UnityEngine;
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

    [Header("Panels")]
    [SerializeField] private GameObject charactersPanel;
    [SerializeField] private GameObject choiceGameplay;

    private string selectedEnvironment;
    private AddressablesManager addressablesManager;

    void Start()
    {
        addressablesManager = GetComponent<AddressablesManager>();

        charactersPanel.SetActive(true);
        choiceGameplay.SetActive(false);

        warriorButton.onClick.AddListener(() => OnCharacterSelected("Forest"));
        archerButton.onClick.AddListener(() => OnCharacterSelected("Desert"));

        letsFightButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(selectedEnvironment))
            {
                addressablesManager.LoadEnemy(selectedEnvironment);
                letsFightButton.interactable = false;
            }
            else
            {
                Debug.LogWarning("No environment selected.");
            }
        });
    }

    private void OnCharacterSelected(string environmentKey)
    {
        selectedEnvironment = environmentKey;

        addressablesManager.LoadEnvironment(environmentKey, () =>
        {
            charactersPanel.SetActive(false);
            choiceGameplay.SetActive(true);
        });
    }
}
