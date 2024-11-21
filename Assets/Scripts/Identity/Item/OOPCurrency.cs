using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OOPCurrency : Identity
{
    public QuestManager _QuestManager;

    public void Start()
    {
        _QuestManager = FindAnyObjectByType<QuestManager>();
    }
    public override void Hit()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.itemPickupSound);
        if (Name == "SmallCurrency")
        {
            Debug.Log("hit small");
            mapGenerator.player.inventory.AddItem("SmallCurrency");
            mapGenerator.smallCurrencies[positionX, positionY] = null;
            _QuestManager.CollectSmallCurrency();

            if (mapGenerator.player.inventory.numberOfItem("SmallCurrency") >= 3)
            {
                bool placed = false;

                while (!placed)
                {
                    int x = Random.Range(0, mapGenerator.X);
                    int y = Random.Range(0, mapGenerator.Y);

                    if (mapGenerator.mapdata[x, y] == mapGenerator.empty)
                    {
                        mapGenerator.PlaceBigCurrency(x, y);
                        mapGenerator.mapdata[x, y] = mapGenerator.bigCurrency;
                        Debug.Log($"Placed BigCurrency at ({x}, {y})");
                        placed = true;
                    }
                }
            }

        }
        else if (Name == "BigCurrency")
        {
            Debug.Log("hit big");
            mapGenerator.player.inventory.AddItem("BigCurrency");
            mapGenerator.bigCurrencies[positionX, positionY] = null;
            _QuestManager.CollectBigCurrency();
        }
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        Destroy(gameObject);
    }
}
