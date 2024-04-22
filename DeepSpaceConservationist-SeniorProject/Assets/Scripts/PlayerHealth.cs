using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public bool canTakeDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            Debug.Log("Player took damage: " + damage);
            health -= damage;
            if (health <= 0)
            {
                PlayerDeath();
            }
            StartCoroutine(InvincibilityFrames());
        }
    }

    private void PlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator InvincibilityFrames()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(.5f);
        canTakeDamage = true;
    }

}
