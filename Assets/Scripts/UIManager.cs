using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI quest1Number;
    public TextMeshProUGUI quest2Number;
    public TextMeshProUGUI quest3Number;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        quest1Number.text = $"{QuestManager.Instance.smallCurrency}/3";
        quest2Number.text = $"{QuestManager.Instance.bigCurrency}/1";
        quest3Number.text = QuestManager.Instance.hasKey ? "1/1" : "0/1";
    }

    public void UpdateQuestNumbers()
    {
        quest1Number.text = $"{QuestManager.Instance.smallCurrency}/3";
        quest2Number.text = $"{QuestManager.Instance.bigCurrency}/1";
        quest3Number.text = QuestManager.Instance.hasKey ? "1/1" : "0/1";
    }
}
