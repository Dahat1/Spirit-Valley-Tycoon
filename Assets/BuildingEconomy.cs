using UnityEngine;

[CreateAssetMenu(fileName = "BuildingEconomy", menuName = "Economy/Building Economy Data")]
public class BuildingEconomy : ScriptableObject
{
    [Header("Unlock Settings")]
    public int baseUnlockCost = 5000;         // İlk bina açılış maliyeti
    public float unlockCostMultiplier = 3f;   // Her yeni bina 3x daha pahalı

    [Header("Production Settings")]
    public int baseProduction = 20;           // İlk bina üretimi (gold/sn)
    public float productionMultiplier = 1.4f; // Her yeni bina %40 daha güçlü

    [Header("Upgrade Settings")]
    public float upgradeCostBaseFactor = 0.5f;  // Upgrade başlangıç maliyeti, unlockCost * bu değer
    public float upgradeCostMultiplier = 1.5f;  // Her seviye maliyet çarpanı

    /// <summary>
    /// Bina indexine göre açılış maliyeti hesaplar.
    /// </summary>
    public int GetUnlockCost(int buildingIndex)
    {
        return Mathf.RoundToInt(baseUnlockCost * Mathf.Pow(unlockCostMultiplier, buildingIndex - 1));
    }

    /// <summary>
    /// Bina indexi ve seviyesi ile üretim hesaplar.
    /// </summary>
    public int GetProduction(int buildingIndex, int level)
    {
        int baseProductionForThisBuilding = Mathf.RoundToInt(baseProduction * Mathf.Pow(productionMultiplier, buildingIndex - 1));
        return baseProductionForThisBuilding * Mathf.Max(1, level);
    }

    /// <summary>
    /// Belirli bina için başlangıç upgrade maliyeti.
    /// </summary>
    public int GetBaseUpgradeCost(int buildingIndex)
    {
        int unlockCost = GetUnlockCost(buildingIndex);
        return Mathf.RoundToInt(unlockCost * upgradeCostBaseFactor);
    }

    /// <summary>
    /// Bina seviyesi için upgrade maliyeti.
    /// </summary>
    public int GetUpgradeCost(int buildingIndex, int level)
    {
        int baseCost = GetBaseUpgradeCost(buildingIndex);
        return Mathf.RoundToInt(baseCost * Mathf.Pow(upgradeCostMultiplier, Mathf.Max(0, level - 1)));
    }
}