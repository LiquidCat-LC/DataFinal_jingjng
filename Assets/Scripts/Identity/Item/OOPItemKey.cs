using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPItemKey : Identity
{
    public string key;
    public override void Hit()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.itemPickupSound);
        Destroy(gameObject);
        mapGenerator.player.inventory.AddItem(key);
        mapGenerator.keys[positionX, positionY] = null;
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        UIManager.Instance.CollectKey();
    }
}
