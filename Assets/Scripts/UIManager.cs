using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public OOPPlayer _player;
    public Inventory _inventory;

    [Header("Game result")]
    public GameObject winpanel;
    public GameObject losepanel;

    [Header("Quest Setup")]
    public TextMeshProUGUI quest1Number;
    public TextMeshProUGUI quest2Number;
    public TextMeshProUGUI quest3Number;
    public int smallCurrency = 0;
    public int bigCurrency = 0;
    public bool hasKey = false;

    [Header("Energy setup")]
    public float maxEnergy;
    public float currentEnergy;
    public Image energyFillImage;
    public TextMeshProUGUI currentEnergyText;

    [Header("scroll setup")]
    public int remainScrollNum;
    public TextMeshProUGUI scrollNumText;

    [Header("defense setup")]
    public Image defenseImage;
    public Sprite[] defenseSprite;

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

        maxEnergy = _player.energy;
        remainScrollNum = 0;
    }

    public void Start()
    {
        quest1Number.text = $"{smallCurrency}/3";
        quest2Number.text = $"{bigCurrency}/1";
        quest3Number.text = hasKey ? "1/1" : "0/1";

        currentEnergy = maxEnergy;
        scrollNumText.text = remainScrollNum.ToString();
    }

    public void CollectSmallCurrency()
    {
        smallCurrency++;
        UpdateQuestNumbers("smallCurrency");
    }

    public void CollectBigCurrency()
    {
        bigCurrency++;
        UpdateQuestNumbers("bigCurrency");
    }

    public void CollectKey()
    {
        hasKey = true;
        UpdateQuestNumbers("hasKey");
    }

    public void UpdateQuestNumbers(string questType)
    {
        switch (questType)
        {
            case "smallCurrency":
                quest1Number.text = $"{smallCurrency}/3";
                break;

            case "bigCurrency":
                quest2Number.text = $"{bigCurrency}/1";
                break;

            case "hasKey":
                quest3Number.text = hasKey ? "1/1" : "0/1";
                break;

            default:
                Debug.LogWarning($"Unknown quest type: {questType}");
                break;
        }
    }

    public void UpdateEnergyUI()
    {
        currentEnergy = _player.energy;
        currentEnergyText.text = _player.energy.ToString("00");
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        float fillAmount = currentEnergy / maxEnergy;
        energyFillImage.fillAmount = fillAmount;
    }

    public void UpdateScrollNum(int scrollNum)
    {
        remainScrollNum += scrollNum;
        if (remainScrollNum <= 0)
        {
            remainScrollNum = 0;
        }
        UpdateScrollUI(remainScrollNum);
    }

    public void UpdateScrollUI(int scrollNum)
    {
        scrollNumText.text = scrollNum.ToString();
        Debug.Log("Updatescroll : " + scrollNum);
    }

    public void Updatedefense(bool isDefense)
    {
        if (isDefense)
        {
            defenseImage.GetComponent<Image>().sprite = defenseSprite[1];
        }
        else
        {
            defenseImage.GetComponent<Image>().sprite = defenseSprite[0];
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
