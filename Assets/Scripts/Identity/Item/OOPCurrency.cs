using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPCurrency : Identity
{

    public override void Hit()
    {
        if (Name == "SmallCurrency")
        {
            Debug.Log("small");
            mapGenerator.player.inventory.AddItem("SmallCurrency");
            mapGenerator.smallCurrencies[positionX, positionY] = null;
        }
        else if (Name == "BigCurrency")
        {
            Debug.Log("big");
            mapGenerator.player.inventory.AddItem("BigCurrency");
            mapGenerator.bigCurrencies[positionX, positionY] = null;
        }
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        Destroy(gameObject);
    }
}
