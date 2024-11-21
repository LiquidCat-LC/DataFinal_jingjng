using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPDefense : Identity
{
    public override void Hit()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.itemPickupSound);
        mapGenerator.player.inventory.AddItem("Defense");
        mapGenerator.defended[positionX, positionY] = null;
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        Destroy(gameObject);

        OOPPlayer player = FindObjectOfType<OOPPlayer>();
        if (player != null)
        {
            player.ActivateDefenseEffect();
        }
    }
}
