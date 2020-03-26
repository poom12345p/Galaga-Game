using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class SpaceShip : Unit
{
    [Tooltip("range taht plyership can move")]
    [SerializeField] private float xLeftBorder, xRightBorder;
    [Tooltip("pool of object that this palyer ship need to use.")]
    [SerializeField] private ObjectPooler myPool;

    [Tooltip("can shoot every x seconds")]
    [SerializeField] private float shootDelay = 0.2f;
    [Tooltip("will re spawn in x seconds")]
    [SerializeField] private float spawnDelay = 1f;

    [SerializeField] private AudioSource firingSound;
    private bool canShoot = true;
    static Transform PlayerShip;

    public void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(xLeftBorder, pos.y, pos.z), new Vector3(xRightBorder, pos.y, pos.z));

       
    }

    private void Awake()
    {
        PlayerShip = transform;//set space ship for global
    }
    private void Start()
    {
       // myCol = GetComponent<Collider2D>();

    }


    private void FixedUpdate()
    {
    
        if(Input.GetButton("Horizontal"))
        {
            float hAxis = Input.GetAxis("Horizontal");
            Vector3 pos = transform.position;


            if (pos.x > xLeftBorder && pos.x < xRightBorder //in range
                ||(hAxis >0 && pos.x == xLeftBorder)//at left borer and will be move to right
                ||(hAxis < 0 && pos.x == xRightBorder))//at rigt borer and will be move to left
            {

                transform.position += hAxis * transform.right * Time.deltaTime * moveSpeed;
            }
            else if (pos.x < xLeftBorder) //is out of range in left
            {

                transform.position = new Vector3(xLeftBorder, pos.y, pos.z);

            }
            else if (pos.x > xRightBorder)//is out of range in right
            {
                transform.position = new Vector3(xRightBorder, pos.y, pos.z);

            }

        }

        if (Input.GetKey("space"))
        {
            ShootBullet();
        }


    }

    private void ShootBullet()
    {
        if (canShoot)
        {
            firingSound.Play();
            myPool.SpawnFromPool("Bullet", transform.position, Quaternion.identity);
            //cooldown the firing
            canShoot = false;
            Invoke("ReCharge", shootDelay);
        }
    }

    //set can shoot agian
    private void ReCharge()
    {
        canShoot = true;
    }


   protected override void GetHit(GameObject hitObj)
    {
        base.GetHit(hitObj);
        //set to re spawn if have anough live
        if(GameControl.liveCount >0)
        {
            Invoke("ReSpawn", spawnDelay);
        }
        //
        GameControl.gameControl.DecreaseLive();
        //
        myPool.SpawnFromPool("Dead", transform.position, Quaternion.identity);//show dead effect
        gameObject.SetActive(false);
    
    }

    public void ReSpawn()
    {
        gameObject.SetActive(true);
        isDead = false;
    }


    public static Vector3 GetPlayerShipPosition()
    {
        return  PlayerShip.position; 
 
    }
}
