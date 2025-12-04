using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUnlockUI : MonoBehaviour
{
    public static BuildingUnlockUI instance;

    [Header("UI Elements")]
    public GameObject panelRoot;
    public Image buildingImage;
    public TMP_Text buildingNameText;
    public TMP_Text productionText;
    public TMP_Text unlockCostText;
    public Button unlockButton;
    public Button closeButton;

    private Building currentBuilding;

    void Awake()
    {
        instance = this;
        panelRoot.SetActive(false);

        if (unlockButton != null)
        {
            unlockButton.onClick.RemoveAllListeners();
            unlockButton.onClick.AddListener(OnUnlockClicked);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Hide);
        }
    }

    void Update()
    {
        // Panel açıksa ve bina atanmışsa butonun aktif/pasif durumunu anlık kontrol et
        if (panelRoot.activeSelf && currentBuilding != null)
        {
            unlockButton.interactable = GameManager.instance.gold >= currentBuilding.unlockCost;
        }
    }

    public void Show(Building building)
    {
        currentBuilding = building;

        // Fill UI
        if (building.unlockedVisual != null)
        {
            Sprite sprite = building.unlockedVisual.GetComponent<SpriteRenderer>().sprite;
            buildingImage.sprite = sprite;
        }

        buildingNameText.text = building.gameObject.name.ToUpper();
        productionText.text = NumberFormatter.FormatNumber(building.baseGoldPerSecond) + " /sec";
        unlockCostText.text = NumberFormatter.FormatNumber(building.unlockCost);

        // Buton ilk gösterildiğinde kontrol
        unlockButton.interactable = GameManager.instance.gold >= building.unlockCost;

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
            });
    }

    private void OnUnlockClicked()
    {
        if (currentBuilding == null) return;

        if (GameManager.instance.SpendGold(currentBuilding.unlockCost))
        {
            currentBuilding.isUnlocked = true;
            currentBuilding.level = 1;
            currentBuilding.UpdateVisuals();
            Hide();

            // İsteğe bağlı → bina açıldıktan sonra normal panel açılır
            Sprite sprite = currentBuilding.unlockedVisual.GetComponent<SpriteRenderer>().sprite;
            BuildingPanelUI.instance.Show(currentBuilding, sprite, currentBuilding.gameObject.name);
        }
    }
}
