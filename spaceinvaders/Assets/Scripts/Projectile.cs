using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    CapsuleCollider2D myCollider;
    private void Start()
    {
        myCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (gameObject.name == "Player_Projectile(Clone)")
        {
            if (myCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "TopBoundary", "Enemy_Projectile", "Baricade")))
                Destroy(gameObject);
        }

        if (gameObject.name == "Enemy_Projectile(Clone)" || gameObject.name == "Boss_shoot(Clone)")
        {
            if (myCollider.IsTouchingLayers(LayerMask.GetMask("Player", "BottomBoundary", "Player_Projectile", "Baricade")))
                Destroy(gameObject);
        }


    }
}
