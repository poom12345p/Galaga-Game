using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red_Enemy : Enemy
{
    // Start is called before the first frame update
    [Tooltip("redEnemy wil shoot if far from player ship at this distane")]
    [SerializeField] float shootDistance;

    [Tooltip("amount of bullet will shoot at smae time")]
    [SerializeField] private int bulletCount;
    private bool isShoot=false;
    //private Vector3 destination;
    protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }



    protected override void StartAttack()
    {
        base.StartAttack();
        //
        isShoot = false;   //reset shoot
        //set destionation near the player ship
        var pos = transform.position;
        var shipPos = SpaceShip.GetPlayerShipPosition();
        var customPos = new Vector3(pos.x, shipPos.y, 0);
        var destination = Vector3.Lerp(shipPos, customPos, Random.Range(0, 0.8f));
        //
        FaceFoward(destination);
    }

    protected override void Attacking()
    {

        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);//move to straight destination

        if (!isShoot && Vector3.Distance(transform.position, SpaceShip.GetPlayerShipPosition())<shootDistance)//check distance and shoot
        {
            for (int i = 0; i < bulletCount; i++)
            {
                ShootBullet();
            }
            isShoot = true;
        }
        base.Attacking();
    }

    private void ShootBullet()
    {
        GameObject  newBullet = myPool.SpawnFromPool("Bullet", transform.position, Quaternion.Euler(0,0,180));
        Bullet b = newBullet.GetComponent<Bullet>();
        //spread bullet  near player position
        var pos = transform.position;
        var shipPos = SpaceShip.GetPlayerShipPosition();
        var customPos = new Vector3(pos.x, shipPos.y, 0);
        var destination = Vector3.Lerp(shipPos, customPos, Random.Range(0, 0.8f));
        Vector3 dir = (destination- transform.position) / Vector3.Distance(transform.position, SpaceShip.GetPlayerShipPosition());
        b.direction = -dir;
        //
    }
}
