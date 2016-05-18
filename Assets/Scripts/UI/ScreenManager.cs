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

    public void EnableComponents(bool flag) {
        GetComponentInChildren<Button>().interactable = flag;
        var toggles = GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles) {
            toggle.interactable = flag;
        }
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
        FindObjectOfType<Game>().Start();
    }

    public void GameIsOver() {
        Camera.enabled = true;
        GameOver.GetComponent<Canvas>().sortingOrder = 3;
    }
}
