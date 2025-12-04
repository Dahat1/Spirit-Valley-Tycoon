using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gold Settings")]
    public double gold = 10000; // Başlangıç gold
    public TMP_Text goldTextUI;

    [Header("Economy Settings")]
    public BuildingEconomy economy;  // Formül tabanlı ekonomi

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        UpdateGoldUI();
    }

    public bool SpendGold(double amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldUI();
            Debug.Log(amount + " gold spent, remaining: " + gold);
            return true;
        }
        else
        {
            Debug.Log("Not enough gold! Current: " + gold + " / Required: " + amount);
            return false;
        }
    }

    public void AddGold(double amount)
    {
        gold += amount;
        UpdateGoldUI();
        Debug.Log(amount + " gold added, total: " + gold);
    }

    void UpdateGoldUI()
    {
        if (goldTextUI != null)
            goldTextUI.text = NumberFormatter.FormatNumber(gold);
    }
}
