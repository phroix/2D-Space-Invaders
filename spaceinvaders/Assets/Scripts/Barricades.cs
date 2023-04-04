using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barricades : MonoBehaviour
{
    public int health = 50;
    public Text healthText;

    private Gamecontroller gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<Gamecontroller>();
    }

    public void IncreaseHealth()
    {
        health += 25;
    }

    // Update is called once per frame
    void Update()
    {
        //healthText.text = health.ToString();
        Debug.Log(health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>(); //checks if other collision object has Projectile.cs component
        if (!projectile) return;

        health -= 1;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
