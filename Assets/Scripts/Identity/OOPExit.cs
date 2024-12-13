using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPExit : Identity
{
    public string unlockKey;

    public override void Hit()
    {
        if (mapGenerator.player.inventory.numberOfItem(unlockKey) > 0)
        {
            Debug.Log("Exit unlocked");
            mapGenerator.player.enabled = false;
            UIManager.Instance.winpanel.SetActive(true);
            Debug.Log("You win");
        }
        else
        {
            Debug.Log($"Exit locked, require key: {unlockKey}");
        }
    }
}
