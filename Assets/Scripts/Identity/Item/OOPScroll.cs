using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPScroll : Identity
{
    public override void Hit()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.itemPickupSound);
        mapGenerator.player.inventory.AddItem("FireStorm");
        UIManager.Instance.UpdateScrollNumber(mapGenerator.player.inventory.numberOfItem("FireStorm"));
        mapGenerator.fireStorms[positionX, positionY] = null;
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        Destroy(gameObject);
    }
}
