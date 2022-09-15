using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinAmount = 5;

  
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            transform.Find("MinimapIcon").GetComponent<SpriteRenderer>().enabled = false;
            transform.Find("ChestLight").GetComponent<Light2D>().enabled = false;
            GameManager.instance.coins += coinAmount;
            GameManager.instance.hud.UpdateHUD();
            GameManager.instance.CoinsSound();
            GameManager.instance.ShowText("+" + coinAmount + " Coins", 25, Color.yellow, transform.position, Vector3.up * 45, 1.5f);
        }
    }
}
