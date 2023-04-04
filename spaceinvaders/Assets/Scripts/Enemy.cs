using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int Leben;

    public int shoot_frequency = 75;
    float shoot_speed = 10f;
    float shoot_timer;
    float nextFire;

    public GameObject projectilePrefab;
    public GameObject firePoint;

    public PolygonCollider2D baum;

    bool shooting = false;

    private void Start()
    {
        baum = GetComponent<PolygonCollider2D>();        
    }

    private void Update()
    {
        Debug.Log(Leben);
        //Generiert random Zahlen von 1 - 10 --> so generiert man das Random schießen der Enemies
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        shoot_timer = Random.Range(1, shoot_frequency);

        if (!shooting)
        {
            shooting = true;
            nextFire = Time.time + shoot_timer;
            return;
        }
        if (Time.time > nextFire)
        {

            nextFire = Time.time + shoot_timer;
            GameObject eLaser = Instantiate(projectilePrefab, firePoint.transform.position, Quaternion.identity) as GameObject;
            eLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shoot_speed);
        }
    }

    void OnTriggerEnter2D(Collider2D coll) //Wird aufgrerufen wenn ein Trigger collider nen anderen Trigger collider bnerührt
    {
        Projectile projectile = coll.gameObject.GetComponent<Projectile>();
        if(!projectile)
       //if (baum.IsTouchingLayers(LayerMask.GetMask("Player_Projectile")))
        {
            return;
        }
        gotHit();

        if (Leben == 0)
        {
            death();
        }
    }

    void gotHit(){
        Leben -= 1;
    }

    void death(){
        Destroy(gameObject);
    }

}