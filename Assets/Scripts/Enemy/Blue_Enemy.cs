using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Enemy : Enemy
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    protected override void Attacking()
    {
        transform.Translate(Vector3.up* Time.deltaTime*moveSpeed);
        base.Attacking();
    }

    protected override void StartAttack()
    {
        base.StartAttack();
        FaceFoward(SpaceShip.GetPlayerShipPosition());
    }

}
