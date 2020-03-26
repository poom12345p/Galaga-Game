using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green_Enemy : Enemy
{
    [SerializeField] private Beam beam;
    [SerializeField] private Animator animator;
    [SerializeField] private SubState subState;
    [SerializeField] float atkTime;
    private Vector3 atkStanbyPos;
   // private bool isAtk=false;
    public enum SubState
    {
        SETPOS,BEAMING,END
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
        beam.gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    IEnumerator BeamingWaitForRotate()
    {
        yield return new WaitUntil(() => !isRotate);
        StartAttack();
        StartBeam();
        
    }

    protected override void Attacking()
    {
      switch(subState)
        {
            case SubState.SETPOS:
                SetPos();
                break;
            case SubState.END:
                Ending();
                break;
        }
        //base.Attacking();
    }

    void SetPos()
    {
        if (Vector2.Distance(transform.position, atkStanbyPos) > stopRange)
        {
            MoveTo(atkStanbyPos);
        }
        else
        {
            StopCoroutine("Rotateting");
            isRotate = false;
            StartCoroutine("Rotateting", Quaternion.Euler(0, 0, 180));
            StartCoroutine("BeamingWaitForRotate");
           
            // subState = SubState.BEAMING;
        }
    }

    void StartBeam()
    {
        subState = SubState.BEAMING;
        beam.gameObject.SetActive(true);
        beam.Show();
        Invoke("EndBeam", atkTime);
    }

    public void Ending()
    {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        base.Attacking();
    }

    public void EndBeam()
    {
        beam.Hide();
    }

    public void EndAct()
    {
        if(subState != SubState.END)
        {
            subState = SubState.END;
            if (enemyManagement.GreenAtking == this.gameObject)
            {
                enemyManagement.GreenAtking = null;
            }
           // isAtk = false;
        }
    }

    protected override void StartAttack()
    {

            base.StartAttack();
         
            var pos = transform.position;
            var shipPos = SpaceShip.GetPlayerShipPosition();
            var customPos = new Vector3(shipPos.x, enemyManagement.GreenEnemyStopPosY, 0);
            atkStanbyPos = customPos;
            subState = SubState.SETPOS;

    }
    protected override void EnterAttackState()
    {
        if (!enemyManagement.GreenAtking && gameObject.active)
        {
            base.EnterAttackState();
            enemyManagement.GreenAtking = this.gameObject;
        }
        else
        {
            EnterStandbyState();
        }
    }

    protected override void GetHit(GameObject hitObj)
    {
        int hp = animator.GetInteger("hp");
        hp--;
         Debug.Log(hp);
        if (hp==0)
        {
           if(enemyManagement.GreenAtking == this.gameObject)
            {
                enemyManagement.GreenAtking =null;
            }
            base.GetHit(hitObj);

        }
        else
        {
            animator.SetInteger("hp", hp);
        }
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        animator.SetInteger("hp",2);
    }
}
