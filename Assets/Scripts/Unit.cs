using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : HitableObject
{ 

    [SerializeField] protected float moveSpeed;
    protected bool isDead=false;

   
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
       // (!LastObjHit || collision.gameObject != LastObjHit)
       if(!isDead)
        {
            base.OnTriggerEnter2D(collision);
        }
    }


    protected override void GetHit(GameObject hitObj)
    {
        base.GetHit(hitObj);
        Debug.Log(gameObject +" is Dead");
        isDead = true;
      
    }

    protected void Reborn()
    {
        ClearHitObj();
        isDead = false;
    }
}
