using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlocking : MonoBehaviour
{

    [SerializeField] private Animator columAnimator;
    [SerializeField] private float DelayMin=20.0f, DelayMax=50.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("deleyForAnimateColum");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator deleyForAnimateColum()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(DelayMin, DelayMax));
            columAnimator.SetTrigger("START");
        }
    }
}
