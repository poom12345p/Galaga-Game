using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : HitableObject,PooledObject
{
    [SerializeField] float speed;

    [Tooltip("bullet move to direction")]
   public Vector3 direction;
    
  
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);//move toward direction
      

        Vector3 screenPoint =Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //  Debug.Log(onScreen);
        if(!onScreen)//disappear when out of screen
        {
            gameObject.SetActive(false);
        }

    }

   public void OnSpawn()
    {
        ClearHitObj();
    }

    /// <summary>
    ///disappear when hit
    /// </summary>
    protected override void GetHit(GameObject hitObj)
    {
        
        gameObject.SetActive(false);
    }

  
}
