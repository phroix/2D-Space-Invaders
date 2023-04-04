using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int Leben = 10;

    public int shoot_frequency = 5;
    float shoot_speed = 10f;
    float shoot_timer;
    float nextFire;
    bool shooting = false;
    bool barrel = true;

    public GameObject projectilePrefab;
    public GameObject Barrel_left;
    public GameObject Barrel_right;

    public PolygonCollider2D beere;

    void Start()
    {
        beere = GetComponent<PolygonCollider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        ShootProjectile();
    }

    private void ShootProjectile(){
        shoot_timer = Random.Range(1, shoot_frequency);

        if (!shooting){
            shooting = true;
            nextFire = Time.time + shoot_timer;
            return;
        }
        if(Time.time > nextFire){
            nextFire = Time.time + shoot_timer;
            barrel = !barrel;
            if (barrel)
            {
                GameObject eLaser = Instantiate(projectilePrefab, Barrel_left.transform.position, Quaternion.identity) as GameObject;
                eLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shoot_speed);
            }
            else
            {
                GameObject eLaser = Instantiate(projectilePrefab, Barrel_right.transform.position, Quaternion.identity) as GameObject;
                eLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -shoot_speed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll){
        Projectile projectile = coll.gameObject.GetComponent<Projectile>();
        if (!projectile) { return; }
        gotHit();

        if(Leben == 0){
            death();
        }
    }

    void gotHit(){
        Leben -= 1;
    }

    void death()
    {
        Destroy(gameObject);
    }
}
