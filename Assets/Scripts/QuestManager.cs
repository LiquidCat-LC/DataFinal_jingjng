using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
   public static QuestManager Instance;

    public int smallCurrency = 0;
    public int bigCurrency = 0;
    public bool hasKey = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CollectSmallCurrency()
    {
        smallCurrency++;
        UIManager.Instance.UpdateQuestNumbers();
    }

    public void CollectBigCurrency()
    {
        bigCurrency++;
        UIManager.Instance.UpdateQuestNumbers();
    }

    public void CollectKey()
    {
        hasKey = true;
        UIManager.Instance.UpdateQuestNumbers();
    }
}
