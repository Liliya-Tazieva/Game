using UnityEngine;
using System.Collections;

public class OnGameGUI : MonoBehaviour {

    void OnGUI() {
        if (GUI.Button(new Rect(30, 40, 100, 50), "Start")) {
            var GameArea = FindObjectOfType<Game>();
            GameArea.StartGame();
        }
        if (GUI.Button(new Rect(30, 95, 100, 50), "Menu")) {
            var GameArea = FindObjectOfType<Game>();
            gameObject.GetComponentInChildren<Camera>().enabled = false;
            FindObjectOfType<ScreenManager>().EnableComponents(true);
            FindObjectOfType<ScreenManager>().Camera.enabled = true;
            GameArea.DestroyCurrentLevel();
        }
    }

    void Start() {
    }
}
