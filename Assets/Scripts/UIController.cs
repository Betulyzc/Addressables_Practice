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

    private EnvironmentType? selectedEnvironment;
    private AddressablesManager addressablesManager;

    void Start()
    {
        addressablesManager = GetComponent<AddressablesManager>();

        charactersPanel.SetActive(true);
        choiceGameplay.SetActive(false);

        warriorButton.onClick.AddListener(() => OnCharacterSelected(EnvironmentType.Forest));
        archerButton.onClick.AddListener(() => OnCharacterSelected(EnvironmentType.Desert));

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

        addressablesManager.LoadEnvironment(environmentType, () =>
        {
            charactersPanel.SetActive(false);
            choiceGameplay.SetActive(true);
        });
    }
}
