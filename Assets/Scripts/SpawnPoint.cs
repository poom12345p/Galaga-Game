using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// point that start spawn set of object with animation and arrange it in train formation
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    [Tooltip("pool of SpawnAnimation")]
    [SerializeField] private ObjectPooler SpawnAnimationPool;
    [Tooltip("Time delay before spawn next on in parade")]
    [SerializeField] private float repeatTime=0.05f;
  
    [SerializeField] private Transform startpoint;

    [Tooltip("parent of object being spawn after finised spawnanimation")]
    public Transform landStation;



    /// <summary>
    /// spawing one by one
    /// </summary>
    /// <param name="spawnObjs">
    ///  set of object spawn in same procession
    /// </param>

    IEnumerator Spawning(Queue<Transform> spawnObjs)
    {
        SpawnAnimation beforeObj = null;
        while(spawnObjs.Count >0)
        {
            //create SpawnAnimation
            var newtrain = SpawnAnimationPool.SpawnFromPool("SpawnAnim", startpoint.position, transform.rotation);
            var newSpawnAim = newtrain.GetComponent<SpawnAnimation>();
            //

            newSpawnAim.AssignRider(spawnObjs.Dequeue());//add spawned object to SpawnAnimation for doing spawn animation

            if (beforeObj != null)//check if is not the head of train,
            {

                beforeObj.crews.Add(newSpawnAim);//Append to the last of the row

            }
            beforeObj = newSpawnAim;//set object(passenger) to last of row
            newSpawnAim.landAt = landStation;//set parent of object(passenger)  after finised spawnanimations
            newSpawnAim.StartTrain();//start animating
            yield return new WaitForSeconds(repeatTime);

        }
    }

    /// <summary>
    /// start spawning
    /// </summary>
    /// <param name="spawnObjs">
    /// set of object(passenger) will be spawn in same procession
    /// </param>
    public void StartSpawn(Queue<Transform> spawnObjs)
    {
        StartCoroutine("Spawning", spawnObjs);
    }
}
