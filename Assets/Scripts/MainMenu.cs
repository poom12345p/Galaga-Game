using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class MainMenu : MonoBehaviour
{
   
    [System.Serializable]
    public struct Menu
    {
        [Tooltip("Menu RectTransFrom")]
        public RectTransform MenuRect;
        [Tooltip("Event will be call when select this menu (will be call after press space)")]
        public UnityEvent menuEvent;
    }
    [Tooltip("Icon that display near selected menu")]
    [SerializeField] RectTransform selectIcon;
    [SerializeField] Menu[] MenuList;
    private int menuIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        menuIndex = 0;
        chageMenuIndex(0);//select first menu
    }

   
    /// <summary>
    /// Invoke all Event that matched with selected menu
    /// </summary>
    public void InvokeSelectedEvent()
    {
        MenuList[menuIndex].menuEvent.Invoke();
    }


    /// <summary>
    /// chagne select menu by change menuIndex that represent element of MenuList
    /// </summary>
    /// <param name="i">
    /// Index will be change by i
    /// </param>
    public void chageMenuIndex(int i)
    {
        //set curent menu selected color to normal color(red)
        RectTransform target = MenuList[menuIndex].MenuRect;
        Text menuText = target.gameObject.GetComponent<Text>();
        menuText.color = Color.red;
        //
        menuIndex = (MenuList.Length + menuIndex + i) % MenuList.Length;//cahge index and doing circular array
        //set curent new menu selected color to green c
        target = MenuList[menuIndex].MenuRect;
        menuText = target.gameObject.GetComponent<Text>();
        menuText.color = Color.green;
        selectIcon.position = new Vector3(selectIcon.position.x, target.position.y, 0);
        selectIcon.sizeDelta = new Vector2(target.sizeDelta.x+40, selectIcon.sizeDelta.y);
        //

    }


    public void StartGame()
    {
        GameControl.gameControl.StartGame();
    }
    public void Exit()
    {
        Application.Quit();
    }

}
