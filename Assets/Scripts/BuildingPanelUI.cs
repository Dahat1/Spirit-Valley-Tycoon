using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingPanelUI : MonoBehaviour
{
    public static BuildingPanelUI instance;

    [Header("Panel Elements")]
    public GameObject panelRoot;
    public GameObject backgroundOverlay;
    public Image buildingImage;
    public TMP_Text buildingNameText;

    [Header("Info Elements")]
    public TMP_Text buildingLevelText;
    public TMP_Text currentGoldText;
    public TMP_Text nextLevelText;
    public TMP_Text nextGoldText;
    public Image goldIconCurrent;
    public Image goldIconNext;

    [Header("Buttons")]
    public Button upgradeButton;
    public Button closeButton;

    private Building currentBuilding;

    void Awake()
    {
        instance = this;
        panelRoot.SetActive(false);
        if (backgroundOverlay != null) backgroundOverlay.SetActive(false);

        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Hide);
        }
    }

    void Update()
    {
        if (panelRoot.activeSelf && currentBuilding != null)
            UpdateUpgradeButtonState();
    }

    public void Show(Building building, Sprite buildingSprite, string buildingName)
    {
        currentBuilding = building;
        buildingImage.sprite = buildingSprite;
        buildingNameText.text = buildingName;

        Debug.Log($"[Panel Opened] Building: {buildingName}, Level: {building.level}, Upgrade Cost: {currentBuilding.upgradeCost}");

        UpdateTexts();

        if (backgroundOverlay != null)
            backgroundOverlay.SetActive(true);

        panelRoot.SetActive(true);
        panelRoot.transform.localScale = Vector3.zero;
        LeanTween.scale(panelRoot, Vector3.one, 0.3f).setEaseOutBack();
    }

    public void Hide()
    {
        LeanTween.scale(panelRoot, Vector3.zero, 0.2f)
            .setEaseInBack()
            .setOnComplete(() =>
            {
                panelRoot.SetActive(false);
                if (backgroundOverlay != null)
                    backgroundOverlay.SetActive(false);
                Debug.Log("[Panel Closed]");
            });
    }

    private void OnUpgradeClicked()
    {
        if (currentBuilding != null)
        {
            Debug.Log($"[Upgrade Button] Building: {currentBuilding.name}, Current Gold: {GameManager.instance.gold}, Required: {currentBuilding.upgradeCost}");
            currentBuilding.UpgradeBuilding();
            UpdateTexts();
        }
        else
        {
            Debug.LogWarning("[Upgrade Button] currentBuilding is NULL!");
        }
    }

    private void UpdateTexts()
    {
        if (currentBuilding != null)
        {
            int currentGold = currentBuilding.GetGoldPerSecond();
            int nextGold = currentBuilding.GetGoldPerSecondNextLevel();

            buildingLevelText.text = "Current Level: " + currentBuilding.level;
            currentGoldText.text = NumberFormatter.FormatNumber(currentGold);

            nextLevelText.text = "Next Level: " + (currentBuilding.level + 1);
            nextGoldText.text = "+" + NumberFormatter.FormatNumber(nextGold);

            upgradeButton.GetComponentInChildren<TMP_Text>().text =
                "Upgrade (Cost: " + NumberFormatter.FormatNumber(currentBuilding.upgradeCost) + ")";

            UpdateUpgradeButtonState();
        }
    }

    private void UpdateUpgradeButtonState()
    {
        bool canAfford = GameManager.instance.gold >= currentBuilding.upgradeCost;

        ColorBlock colors = upgradeButton.colors;
        colors.normalColor = canAfford ? Color.green : Color.gray;
        colors.highlightedColor = canAfford ? new Color(0.7f, 1f, 0.7f) : new Color(0.8f, 0.8f, 0.8f);
        upgradeButton.colors = colors;

        upgradeButton.interactable = canAfford;
    }
}
