using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public bool canTakeDamage = true;
    private LevelLoader levelLoader;
    private FPSInput fpsInput;
    private MouseLook[] mouseLooks;
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        levelLoader = GameObject.FindObjectOfType<LevelLoader>();
        fpsInput = GameObject.FindObjectOfType<FPSInput>();
        mouseLooks = GameObject.FindObjectsOfType<MouseLook>();
        playerCamera = GameObject.FindObjectOfType<Camera>();
        
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
        Debug.Log("Player died");
        fpsInput.enabled = false;
        foreach (MouseLook mouseLook in mouseLooks)
        {
            mouseLook.enabled = false;
        }
        playerCamera.transform.Rotate(Vector3.back, 90);
        levelLoader.LoadCurrentLevel();
        
    }

    private IEnumerator InvincibilityFrames()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(.5f);
        canTakeDamage = true;
    }

}
