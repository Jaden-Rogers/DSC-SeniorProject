using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MoneyBank : MonoBehaviour
{
    public int money = 0;
    public TMPro.TextMeshProUGUI moneyText;
    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = "Money: " + money;
    }
    // Start is called before the first frame update
    void Start()
    {
        // find moneytext by name
        moneyText = GameObject.Find("MoneyText").GetComponent<TMPro.TextMeshProUGUI>();
        moneyText.text = "Money: " + money;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
