using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// for attach with animation event of object need to set self inactive after end animation
/// </summary>
public class AnimationEffect : MonoBehaviour
{
   
    
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

}
