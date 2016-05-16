using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour {
    public GameObject Win;
    public GameObject LevelCompl;
    public GameObject GameOver;

    void Start() {
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LevelCompleted() {
        LevelCompl.GetComponent<Canvas>().sortingOrder = 3;
    }

    public void GameCompleted() {
        Win.GetComponent<Canvas>().sortingOrder = 3;
    }

    public void GameIsOver() {
        GameOver.GetComponent<Canvas>().sortingOrder = 3;
    }
}
