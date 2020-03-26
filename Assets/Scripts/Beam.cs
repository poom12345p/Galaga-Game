using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : HitableObject
{
    [SerializeField] private Animator animator;
    [SerializeField] Green_Enemy greenEnemy;
    [SerializeField] AudioSource beamSound;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void GetHit(GameObject hitObj)
    {
        base.GetHit(hitObj);
        Hide();
    }

    public void Show()
    {
        ClearHitObj();
        beamSound.Play();
        animator.ResetTrigger("OUT");
        animator.SetTrigger("IN");
    }

    public void Hide()
    {
        beamSound.Stop();
        animator.ResetTrigger("IN");
        animator.SetTrigger("OUT");
    }

    public void Finished()
    {
        greenEnemy.EndAct();
    }

    public void SetObjActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void SetObjActiveTrue()
    {
        gameObject.SetActive(true);
    }
}
