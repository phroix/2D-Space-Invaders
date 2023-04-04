using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public double timer = 0.0;
    public double moveTime = 5.0;

    //0 steht f�r rechts und 1 f�r links
    public int richtung = 0;

    public int anzahlEne = 65;

    BoxCollider2D myBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Timer and getting the Number of Child Elements ( from Enemys_Group )
        timer += Time.deltaTime;
        anzahlEne = transform.childCount; 

        //Movement of the Enemys
        speedUp(anzahlEne);
        move();
    }

    void move()
    {
        if (timer > moveTime && richtung == 0)
        {
            transform.Translate(new Vector3(.5f, 0, 0));
            timer = 0.0;
        }
        else if (timer > moveTime && richtung == 1)
        {
            transform.Translate(new Vector3(-.5f, 0, 0));
            timer = 0.0;
        }
    }

    void speedUp(int controll)
    {
        if(controll < 40 && controll > 20){
            moveTime = 0.4;
        }else if(controll < 20){
            moveTime = 0.2;
        }
    }

    void OnTriggerEnter2D(Collider2D coll) //Wird aufgrerufen wenn ein Trigger collider nen anderen Trigger collider bner�hrt
    {
        //Checks the collider type �--> in this case its only working by the Boundarys
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Boundarys"))) 
        {
            transform.Translate(new Vector3(0, -1.5f, 0));

            switch (richtung)
            {
                case 0: richtung = 1; break;
                case 1: richtung = 0; break;
            }
        }
    }
}
