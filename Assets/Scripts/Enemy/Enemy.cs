using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit,Trainpassenger,PooledObject
{
    [Tooltip("score of that will increas after defeat this enemy.")]
    [SerializeField] protected int score;
    [Tooltip("pool of object that this enemy need to use.")]
    [SerializeField] protected ObjectPooler myPool;
    [Tooltip("The time that enemy will wait for re-entering the screen after it flies out of the screen.")]
    [SerializeField] protected int reEnterDelayTime;
    // [SerializeField] private float ratateSpeed;
    [Tooltip("Time the that enemy will do attack action again after the attacked will be random between min and max")]
    [SerializeField] private float AtKDelayMin, AtkDelayMax;
    private float attackINSeconds;

    //transform for referencing a position in lines
    [HideInInspector] public Transform StanbyPositionRef;
    [HideInInspector] public EnemyManagement enemyManagement;

    //vaiable for checking enemy rotation
    protected bool isRotate=false;
    //enemy rotation speed
    const float TurnSpeed = 8f;
    [Tooltip("threshold of stopping near the destination (use in MoveTo funtion)")]
    [SerializeField]protected float stopRange = 0.1f;
    protected enum State
    {
        SPAWN,STANDBY,ROTATE,ATTACK,REPOS,NONE
    }
    [Tooltip("State of enemy")]
    [SerializeField] protected State state;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        state = State.SPAWN;

    }

    /// <summary>
    /// rotate the enemy with smooth animation
    /// </summary>
    /// <param name="direction">
    /// quaterinon that object will rotate to
    /// </param>
    protected IEnumerator Rotateting(Quaternion direction)
    {
        isRotate = true;
        while (Quaternion.Angle(transform.rotation,direction) != 0)
        {
            yield return new WaitForFixedUpdate();
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * TurnSpeed);
           
        }
        isRotate = false;

    }

    /// <summary>
    /// start actact after finised rotation
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackWaitForRotate()
    {
        yield return new WaitUntil(() => !isRotate);
       
        StartAttack();
       
    }



    protected virtual void  Update()
    {
        switch (state)
        {
            case State.SPAWN:
                
                break;
            case State.REPOS:
                RePosition();
                break;
            case State.STANDBY:
                Standby();
                break;
            case State.ATTACK:
                Attacking();
                break;
        }
    }

    /// <summary>
    /// action after enemy being hit
    /// </summary>
    protected override void GetHit(GameObject hitObj)
    {
        base.GetHit(hitObj);
        StopAllCoroutines();
        enemyManagement.EnemyDead(this);//report to managment
        GameControl.gameControl.IncreaseScore(score);//IncreaseScore

        Transform deadEff = myPool.SpawnFromPool("Dead", transform.position, Quaternion.identity).transform;//show dead effect
        deadEff.SetParent(StanbyPositionRef);//for animation reason
      
        gameObject.SetActive(false);
    }

    protected virtual void EnterStandbyState()
    {
        if (gameObject.active)//prevent bug call coroutine after object is inactive cause this funtion is called by Invoke
        {
            StartCoroutine("Rotateting", Quaternion.identity);
            attackINSeconds = Random.Range(AtKDelayMin, AtkDelayMax);
          
            Invoke("EnterAttackState", attackINSeconds);
            state = State.STANDBY;
        }
   
    }
    
    /// <summary>
    /// if enemy is standby ,enemy will fly after fixed positon
    /// </summary>
    protected virtual void Standby()
    {
        transform.position = StanbyPositionRef.position;
    }

  

    protected virtual void EnterAttackState()
    {
        if (gameObject.active)//prevent bug call coroutine after object is inactive cause this funtion is called by Invoke
        {
            StopCoroutine("Rotateting");//stop rotate for prevent bug form being call new rotate coroutine
            isRotate = false;
            StartCoroutine("Rotateting", Quaternion.Euler(0, 0, 180));
            StartCoroutine("AttackWaitForRotate");//enter ATTACK state after finised rotaion
        }
    }


    protected virtual void StartAttack()
    {
        state = State.ATTACK;
    }


    /// <summary>
    /// check if enemy is out of screen,attacking  will  end;
    /// </summary>
    protected virtual void Attacking()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
      
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //   Debug.Log(screenPoint);
        if (!onScreen)
        {
            EndAttacking();
        }

    }

    /// <summary>
    /// enemy move out of screen and wait for re Enter thescreen 
    /// </summary>
    protected virtual void EndAttacking()
    {
        if(StanbyPositionRef != null)
        {
            var edgePos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            transform.position = new Vector3(StanbyPositionRef.position.x, edgePos.y*2, 0);
        }
    
        Invoke("ReEnter", reEnterDelayTime);
        state = State.NONE;
      
    }

    /// <summary>
    /// re enter the screenn from above
    /// </summary>
    protected virtual void ReEnter()
    {
        
        if (StanbyPositionRef != null)
        {
            var edgePos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            transform.position = new Vector3(StanbyPositionRef.position.x, edgePos.y, 0);
        }
        state = State.REPOS;
    }


    /// <summary>
    /// rotate enemy to target
    /// </summary>
    /// <param name="targetPos">
    /// position taht enemy will rotate to.
    /// </param>
    protected void FaceFoward(Vector3 targetPos)
    {
        var dir = transform.position - targetPos;
        //var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90;
        transform.up = -dir;
    }


    /// <summary>
    /// move Enemy to in line positon
    /// </summary>
    protected virtual void RePosition()
    {
        if(Vector2.Distance( transform.position , StanbyPositionRef.position)>stopRange)
        {

            MoveTo(StanbyPositionRef.position);

        }
        else
        {
         
            EnterStandbyState();
        }
    }

    /// <summary>
    /// move enemy to destination
    /// </summary>
    /// <param name="des">
    /// destination
    /// </param>
    protected void MoveTo(Vector3 des)
    {
        if (Vector2.Distance(transform.position, des) > stopRange)
        {

            transform.position = Vector2.MoveTowards(transform.position, des, moveSpeed * Time.deltaTime);
            FaceFoward(des);



        }
       
    }

    /// <summary>
    /// call when  this enemy begin spawn from pool
    /// </summary>
    public virtual void OnSpawn()
    {
        state = State.SPAWN;
        isDead = true;//prevent dead until Enemy will be in place
    }
    
    /// <summary>
    /// will be call after spawning animation end
    /// </summary>
    public void EndRide()
    {
        state = State.REPOS;
        Reborn();
    }


  
}
