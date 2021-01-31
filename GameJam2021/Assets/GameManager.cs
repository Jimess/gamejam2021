using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void OnLevelStartDelegate();
    public static OnLevelStartDelegate OnGameStart;
    public delegate void OnTradeLevelStartDelegate();
    public static OnTradeLevelStartDelegate OnTradeLevelStart;

    private ContentItem startItem;
    private ContentItem goalItem;

    private void Awake() {
        Instance = this;
        GameContent.OnContentLoad += ContentLoaded;
    }

    private void ContentLoaded() {
        OnGameStart?.Invoke();
    }

    public void MenuLevelComplete() {
        MenuSystem.Instance.HideMenu().OnComplete(() => {
            OnTradeLevelStart?.Invoke();
        });
        
    }



    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("space")) {
            TogglePause();
        }
        //timeCounter.value = Time.time - startTime;
    }

    void TogglePause() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            Debug.Log("PAUSED");
        } else {
            Time.timeScale = 1;
            Debug.Log("UNPAUSED");
        }
    }

    public void SetStartItem(ContentItem item)
    {
        startItem = item;
    }

    public ContentItem GetStartItem()
    {
        return startItem;
    }

    public void SetGoalItem(ContentItem item)
    {
        goalItem = item;
    }

    public ContentItem GetGoalItem()
    {
        return goalItem;
    }
}
