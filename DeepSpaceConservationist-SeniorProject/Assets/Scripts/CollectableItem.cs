using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public MoneyBank moneyBank;

    // Start is called before the first frame update
    void Start()
    {
        moneyBank = GameObject.Find("MoneyText").GetComponent<MoneyBank>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moneyBank.AddMoney(1);
            Destroy(gameObject);
        }
    }
}
