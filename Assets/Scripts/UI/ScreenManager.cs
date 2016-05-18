using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour {
    public GameObject Win;
    public GameObject LevelCompl;
    public GameObject GameOver;
    public Camera Camera;

    void Start() {
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LevelCompleted() {
        Camera.enabled = true;
        LevelCompl.GetComponent<Canvas>().sortingOrder = 3;
    }

    public void GameCompleted() {
        Camera.enabled = true;
        Win.GetComponent<Canvas>().sortingOrder = 3;
    }

    public void GameIsOver() {
        Camera.enabled = true;
        GameOver.GetComponent<Canvas>().sortingOrder = 3;
    }
}
