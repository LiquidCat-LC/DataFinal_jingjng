using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPItemKey : Identity
{
    public string key;
    public QuestManager _QuestManager;

    public void Start()
    {
        _QuestManager = FindAnyObjectByType<QuestManager>();
    }

    public override void Hit()
    {
        Destroy(gameObject);
        mapGenerator.player.inventory.AddItem(key);
        mapGenerator.keys[positionX, positionY] = null;
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        _QuestManager.CollectKey();
    }
}
