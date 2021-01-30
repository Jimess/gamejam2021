using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void OnLevelStartDelegate();
    public static OnLevelStartDelegate OnGameStart;

    private void Awake() {
        Instance = this;
        GameContent.OnContentLoad += ContentLoaded;
    }

    private void ContentLoaded() {
        OnGameStart?.Invoke();
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
}
