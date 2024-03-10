using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shopUI : MonoBehaviour
{
    public int currentCash = 500;

    public TMP_Text currentCashText;

    public GameObject shotgun, assaultRifle, grenade, shopkeeper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentCashText.text = "Current Cash: $" + currentCash.ToString();
    }

    public void buy_shotgun()
    {
        // If player has enough money
        if (currentCash >= 600)
        {
            // Deduct Money
            currentCash = currentCash - 300;

            // Spawn shotgun
            shopkeeper.GetComponent<shopkeeper>().spawnItem(shotgun);
        }
    }

    public void buy_rifle()
    {
        // If player has enough money
        if (currentCash >= 650)
        {
            // Deduct Money
            currentCash = currentCash - 650;

            shopkeeper.GetComponent<shopkeeper>().spawnItem(assaultRifle);
        }
    }

    public void buy_grenade()
    {
        if (currentCash >= 200)
        {
            // Deduct Money
            currentCash = currentCash - 200;

            // Spawn Grenade
            shopkeeper.GetComponent<shopkeeper>().spawnItem(grenade);
        }
    }

    public void bargain_shotgun()
    {
        Debug.Log("bargain_shotgun");

        int price = 600;

        // Start shotgun bargain
        shopkeeper.GetComponent<shopkeeper>().Bargain(shotgun, price);
    }

    public void bargain_rifle()
    {
        Debug.Log("bargain_rifle");

        int price = 650;

        // Start rifle bargain
        shopkeeper.GetComponent<shopkeeper>().Bargain(assaultRifle, price);
    }

    public void bargain_grenade()
    {
        Debug.Log("bargain_grenade");

        int price = 300;

        // Start grenade bargain
        shopkeeper.GetComponent<shopkeeper>().Bargain(grenade, price);
    }

    public bool deductMoney(int price)
    {

        Debug.Log(price);

        if (currentCash >= price)
        {
            currentCash -= price;

            return true;
        }

        return false;
    }


    public void exit_Shop_UI()
    {

        // Resume movement
        Time.timeScale = 1f;

        // Set ui as false
        gameObject.SetActive(false);
    }
}
