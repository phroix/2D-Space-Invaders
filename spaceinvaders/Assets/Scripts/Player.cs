using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Player variables
    [Header("Player")]
    public float moveSpeed = 10f;
    public float padding = 1f;

    public int health = 3;
    public GameObject [] imageHeart;

    //Projectile variables
    [Header("Projectile")]
    public GameObject projectilePrefab;

    public GameObject leftCannon;
    public GameObject rightCannon;

    public float projectileSpeed = 10f;
    public float projectileFiringPeriod = .7f;


    //non public variables
    float xMin, xMax;//min, max for Boundaries

    //for shooting
    bool alternate = false;
    float nextFire = 0;



    void Start()
    {
        SetUpMoveBoundaries();
    }

    void Update()
    {
        move();
        fireProjectile();
    }

    //If player is hit by Object (Projectile)
    private void OnTriggerEnter2D(Collider2D coll)
    {
        ProcessHit(coll);
    }

    private void ProcessHit(Collider2D coll)
    {
        Projectile projectile = coll.gameObject.GetComponent<Projectile>(); //checks if other collision object has Projectile.cs component
        if (!projectile) return;

        health -= 1;
        Destroy(imageHeart[health]);//Destroy 1 heart after hit

        if (health <= 0)
        {
            //load gamescene
            Destroy(gameObject);
            SceneManager.LoadScene("GameOverScene");
        }
    }
    //Player Movement
    private void move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;//Gets Axis from Project setting, Time.deltaTime makes frame rate independet

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax); //position of x-axis + deltaX, Clamp -> The float result between the min and max values

        Vector2 playPos = new Vector2(newXPos, transform.position.y);//creates new Vector with new X-Axis position
        transform.position = playPos; //player position updated every frame
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;//give the value of the variable xMin that comes back from saying what is world space value on x axis
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
    }


    private void fireProjectile()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)//Cecks if "Fire1" Button is pressed AND time at the beginning of this frame 
        {
            nextFire = Time.time + projectileFiringPeriod;//nextFire set to time at the beginning of this frame + projectileFiringPeriod
            alternate = !alternate; //To Change the Cannons

            if (alternate)
            {
                fireAlternate(leftCannon);//Shoot at leftCanon
            }
            else
            {
                fireAlternate(rightCannon);//Shoot at rightCanon
            }
        }
    }
    
    void fireAlternate(GameObject g)//Instantiate laserPrefab at leftCanon/rigthCanon and sets velocity equal to projectileSpeed
    {
        GameObject laser = Instantiate(projectilePrefab, g.transform.position, Quaternion.identity) as GameObject;//Instantiate object laserPrefab at g position
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed); //Gets Component of Rigidbody and sets it velocity to Vector 2
    }


}