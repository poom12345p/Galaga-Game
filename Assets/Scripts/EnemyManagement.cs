using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyPositionSet
    {
        public string enemyTag;
        public List<Transform> points;
    }
    [System.Serializable]
    public struct SpawnSet
    {
        public string[] enemyTag;
        [Tooltip("spawnPoint using to spawn set of enemy")]
        public SpawnPoint spawnPoint;
        [Tooltip("delay before enemy set (count after latest spawn)")]
        public float delayTime;
    }
    [Tooltip("pool of enemy")]
    [SerializeField] private ObjectPooler enemyPool;

    [Tooltip("list of matched enemy tag and with inline position of enemy")]
    [SerializeField] private EnemyPositionSet[] enemyPositionSets;

    [Tooltip("order of set enemy that will be spawn")]
    [SerializeField] private SpawnSet[] spawnSets;

    //a copy of spawnset in queue for doing repeat spawn all enemy in each level
    private Dictionary<string, Queue<Transform>> LineData= new Dictionary<string, Queue<Transform>>();
    //for checking is all set spawn
    private int setCount=0;
    //list of enemy that don't defeat yet
    [SerializeField]private List<Enemy> liveEnemy;

    [Tooltip("green enemy will stop at this Y-Axis before beaming")]
    [SerializeField] public float GreenEnemyStopPosY;

    //only one green enemy can attack.Another green can't attack if one of green enemy is attacking
    [HideInInspector]public GameObject GreenAtking = null;
    // Start is called before the first frame update

    public void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-5, GreenEnemyStopPosY,0), new Vector3(5, GreenEnemyStopPosY, 0));


    }


    void Start()
    {
        //StartEnemyManagement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //start spawn enemy and loop untill end game
    public void StartEnemyManagement()
    {
        ResetEnemy();
        StartSpawn();
    }

    /// <summary>
    /// spawn all set by order
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawning()
    {
        setCount = 0;
        foreach (var set in spawnSets)
        {
            yield return new WaitForSeconds(set.delayTime);
            SetSpawn(set);
            setCount++;
            
        }
    }


    public void StartSpawn()
    {
        StartCoroutine("Spawning");
    }

    /// <summary>
    /// spawn set of enemy
    /// </summary>
    /// <param name="set">
    /// set of enemy
    /// </param>
    public void SetSpawn(SpawnSet set)
    {
      
        Queue<Transform> spawnQueue = new Queue<Transform>();
        foreach (var tag in set.enemyTag)
        {
            var pointQueue = LineData[tag];

            if (pointQueue.Count > 0)
            {
                GameObject newEnemy = enemyPool.SpawnFromPool(tag, set.spawnPoint.transform.position, Quaternion.identity);
                Enemy enemy = newEnemy.GetComponent<Enemy>();

                enemy.enemyManagement = this;
                liveEnemy.Add(enemy);
                
                enemy.StanbyPositionRef = pointQueue.Dequeue();
               // Debug.Log(enemy.linePositionRef);
                spawnQueue.Enqueue(newEnemy.transform);
            }
        }
        set.spawnPoint.StartSpawn(spawnQueue);
    }

 
    /// <summary>
    /// resert level
    /// </summary>
    public void ResetEnemy()
    {
        LineData = new Dictionary<string, Queue<Transform>>();
      
        foreach (var line in enemyPositionSets)
        {
            LineData.Add(line.enemyTag, new Queue<Transform>(line.points));
        }

      
    }

    /// <summary>
    /// call when a enemy is dead
    /// </summary>
    /// <param name="deadEnemy">
    /// data of a dead enemy
    /// </param>
    public void EnemyDead(Enemy deadEnemy)
    {
        liveEnemy.Remove(deadEnemy);
        if(liveEnemy.Count ==0 && setCount == spawnSets.Length)//check if all enemy dead,then reset level and respawn all
        {
            ResetEnemy();
            StartSpawn();
        }
    }

    /// <summary>
    /// clear all enemy and end spawning
    /// </summary>
    public void ClearEnemy()
    {
        foreach (var enemy in liveEnemy)
        {
            enemy.gameObject.SetActive(false);
        }
        liveEnemy.Clear();
    }
}
