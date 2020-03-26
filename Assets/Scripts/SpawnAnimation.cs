using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// unit int parade
/// </summary>
public class SpawnAnimation : MonoBehaviour
{
   [Tooltip("body of train(passenger will be animation at same point of this transfrom) ")]
    public Transform animPivot;

    [Tooltip("passenger taht will be animated  ")]
    public Transform myPassenger;

    [Tooltip("parent after finised animation")]
    public Transform landAt;

    [Tooltip("director that control animation")]
    public PlayableDirector director;

    [Tooltip("list of next passenger in train")]
    public List<SpawnAnimation> crews;
   

    public void  AssignRider(Transform rider)
    {
        myPassenger = rider;
        rider.SetParent(animPivot);
        rider.position = animPivot.position;
        rider.rotation = animPivot.rotation;
    }

    public void StartTrain()
    {
        director.Play();
        
    }

    public void FinishSpawnAnimation()
    {
        try// check passenger is active and not missign
        {
            director.Stop();
            myPassenger.SetParent(landAt);
            var tr = myPassenger.GetComponent<Trainpassenger>();
            if (tr != null)
            {
                tr.EndRide();
            }
         
        }
        catch
        {
            Debug.LogWarning("Rider is unactive");
        }
        foreach (var crew in crews)//if head is finised animation all crew will be end at same time
        {
            
            crew.FinishSpawnAnimation();
        }
        crews.Clear();
        gameObject.SetActive(false);
    }
}
