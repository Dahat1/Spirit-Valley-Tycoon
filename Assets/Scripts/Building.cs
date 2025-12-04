using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Building Index")]
    public int buildingIndex = 1;   // Inspector’dan atanır (1 → ilk bina, 2 → ikinci bina...)

    [Header("Unlock Settings (otomatik hesaplanır)")]
    public int unlockCost;
    public bool isUnlocked = false;

    [Header("Production Settings (otomatik hesaplanır)")]
    public int baseGoldPerSecond;
    public int level = 0;               
    public int upgradeCost;       
    private float productionTimer;

    [Header("Visuals")]
    public GameObject lockedVisual;
    public GameObject unlockedVisual;

    void Start()
    {
        // Unlock maliyeti ve base üretim → ScriptableObject economy’den
        unlockCost = GameManager.instance.economy.GetUnlockCost(buildingIndex);
        baseGoldPerSecond = GameManager.instance.economy.GetProduction(buildingIndex, 1); 

        // Upgrade cost → unlock cost'un %50'si ile başlıyor
        upgradeCost = Mathf.RoundToInt(unlockCost * 0.5f);

        UpdateVisuals();
    }

    void Update()
    {
        if (isUnlocked)
        {
            productionTimer += Time.deltaTime;
            if (productionTimer >= 1f)
            {
                ProduceGold();
                productionTimer = 0f;
            }
        }
    }

    public void OnClick()
    {
        if (!isUnlocked)
        {
            BuildingUnlockUI.instance.Show(this);
        }
        else
        {
            Sprite buildingSprite = unlockedVisual.GetComponent<SpriteRenderer>().sprite;
            BuildingPanelUI.instance.Show(this, buildingSprite, gameObject.name);
        }
    }

    public void UpgradeBuilding()
    {
        if (GameManager.instance.SpendGold(upgradeCost))
        {
            level++;
            // Upgrade cost artışı
            upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f);
            Debug.Log($"Building upgraded! New level: {level} | New cost: {upgradeCost}");
        }
        else
        {
            Debug.Log($"Not enough gold! Current: {GameManager.instance.gold} / Required: {upgradeCost}");
        }
    }

    public int GetGoldPerSecond()
    {
        return GameManager.instance.economy.GetProduction(buildingIndex, level);
    }

    public int GetGoldPerSecondNextLevel()
    {
        return GameManager.instance.economy.GetProduction(buildingIndex, level + 1);
    }

    void ProduceGold()
    {
        int goldProduced = GetGoldPerSecond();
        GameManager.instance.AddGold(goldProduced);
        Debug.Log($"{goldProduced} gold produced! (Level: {level})");
    }

    public void UpdateVisuals()
    {
        lockedVisual.SetActive(!isUnlocked);
        unlockedVisual.SetActive(isUnlocked);
    }
}
