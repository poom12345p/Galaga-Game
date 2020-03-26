using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesDisplay : MonoBehaviour
{
    [Tooltip("live Icon that will be show")]
    [SerializeField] private GameObject livesIconPrefab;
    //variable contains lives icon that is child of this object
    private List<GameObject> lives = new List<GameObject>();

    //live count and index of lives that divide active and inactive  iconin lives(index < livesCount  is active / index >= livesCount  is inactive)
    public int livesCount = 0;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// display and manage list of live icon lives of player in case  live is increase
    /// </summary>
    /// <param name="num">
    /// amount of live increse
    /// </param>
   public void IncreasLive(int num)
    {

        for (int i = 0; i < num; i++)
        {
            if (livesCount < lives.Count)//if have liveIcon in list to display
            {
                lives[livesCount].SetActive(true);

            }
            else//not enough liveIcon in list then spawn the new one
            {
                GameObject newIcon = Instantiate(livesIconPrefab, transform);
                lives.Add(newIcon);

            }
            livesCount++;
        }

    }

    /// <summary>
    /// display lives of player  in case  live is decrease
    /// </summary>
    /// <param name="num">
    /// amount of live decreas
    /// </param>
    public void DecreasLive(int num)
    {

        for (int i = 0; i < num; i++)
        {

            if (livesCount > 0)
            {
                livesCount--;
                lives[livesCount].SetActive(false);

            }
        }

    }
}
