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

    [Header("Text setup")]
    public TextMeshProUGUI quest1Number;
    public TextMeshProUGUI quest2Number;
    public TextMeshProUGUI quest3Number;
    public TextMeshProUGUI scrollNumber;

    [Header("Energy setup")]
    public float maxEnergy;
    public float currentEnergy;
    public Image energyFillImage;
    public TextMeshProUGUI currentEnergyText;

    [Header("scroll setup")]
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
    }

    public void Start()
    {
        quest1Number.text = $"{QuestManager.Instance.smallCurrency}/3";
        quest2Number.text = $"{QuestManager.Instance.bigCurrency}/1";
        quest3Number.text = QuestManager.Instance.hasKey ? "1/1" : "0/1";

        currentEnergy = maxEnergy;
    }

    public void UpdateQuestNumbers()
    {
        quest1Number.text = $"{QuestManager.Instance.smallCurrency}/3";
        quest2Number.text = $"{QuestManager.Instance.bigCurrency}/1";
        quest3Number.text = QuestManager.Instance.hasKey ? "1/1" : "0/1";
    }

    public void UpdateEnergyUI()
    {
        currentEnergy = _player.energy;
        currentEnergyText.text = _player.energy.ToString("00");
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        float fillAmount = currentEnergy / maxEnergy;
        energyFillImage.fillAmount = fillAmount;
    }

    public void UpdateScrollNumber(int scrollNum)
    {
        scrollNumText.text = scrollNum.ToString();
        Debug.Log("Update : "+scrollNum);
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
