using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableObject : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("tag of object that can interact with this object")]
    public List<string> HitableTag;
    //variable use for detect object that this object already hit for preventing repeat-interacting
    [HideInInspector] protected List<GameObject> alreadyHitObj = new List<GameObject>();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (HitableTag.Contains(collision.tag) && !alreadyHitObj.Contains(collision.gameObject))//check if collision object can interact with
        {
            alreadyHitObj.Add(collision.gameObject);//notify that this obj is alreay hited
            GetHit(collision.gameObject);

        }
    }

    /// <summary>
    /// action will done after get hit
    /// </summary>
    /// <param name="hitObj">
    /// object that interacted with this object
    /// </param>
    protected virtual void GetHit(GameObject hitObj)
    {
        Debug.Log(gameObject + " is hit");
    }

    /// <summary>
    ///reset this object for being reused
    /// </summary>
    protected void ClearHitObj()
    {
      
        //anothers object  that this object used to interact with this can interact with this again
        foreach (var obj in alreadyHitObj)
        {
            var HabObj = obj.GetComponent<HitableObject>();
            if (HabObj) HabObj.alreadyHitObj.Remove(gameObject);
        }
        // this object can interact with  another  that this objectused to interact with again
        alreadyHitObj.Clear();
    }
}
