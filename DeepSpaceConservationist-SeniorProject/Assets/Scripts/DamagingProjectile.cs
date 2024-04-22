using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingProjectile : MonoBehaviour
{
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        //destroy the projectile after 3 seconds
        Destroy(gameObject, 3);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
