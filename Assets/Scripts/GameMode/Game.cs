using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.Scripts.Agents;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public List<GameObject> Levels;
    private int _currentLevel;
    public GameObject LevelInstanse;
    public LevelOptions Level;
    public float AgentsSpeed;

    public void InstantiateMap(Transform parent) {
        if (LevelInstanse == null) {
            LevelInstanse = Instantiate(Levels[_currentLevel], Vector3.zero, Quaternion.identity) as GameObject;
            if (LevelInstanse != null) {
                LevelInstanse.transform.SetParent(parent);
                Level = LevelInstanse.GetComponent<LevelOptions>();
            }
        }
    }

    public void SlowSpeed() {
        AgentsSpeed = 0.5f;
    }

    public void MediumSpeed() {
        AgentsSpeed = 0.85f;
    }

    public void FastSpeed() {
        AgentsSpeed = 1.2f;
    }

    public void StartGame() {
        if (Level != null) {
            Level.CrowdManager.DestroyAgents();
            Level.CrowdManager.InstantiateAgents();
        }
    }

    public void DestroyCurrentLevel() {
        if (Level.CrowdManager != null) {
            Level.CrowdManager.DestroyAgents();

        }
        DestroyImmediate(LevelInstanse.gameObject);
    }

    public void PreviousLevel(bool flag) {
        if (Level != null) {
            DestroyCurrentLevel();
        }
        if (flag) {
            if (_currentLevel != 0) _currentLevel--;
        } else {
            if (_currentLevel != Levels.Count - 1) _currentLevel++;
        }
        InstantiateMap(gameObject.transform);
    }
	// Use this for initialization
    public void Start() {
        _currentLevel = 0;
        AgentsSpeed = 0.85f;
        //InstantiateMap(gameObject.transform);
    }
	
	// Update is called once per frame
	void Update () {
	    if (LevelInstanse != null) {
            if (Input.GetButtonDown("Fire1")) {
                RaycastHit hit;
                var cam = FindObjectOfType<Camera>();
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    var agent = hit.collider.gameObject;
                    if (agent.GetComponent<AgentLogic>() != null) {
                        Destroy(agent);
                        Level.CrowdManager.AgentsAmount--;
                    }
                }
            }
            if (Level.CrowdManager.GameOver) {
                Level.CrowdManager.GameOver = false;
                FindObjectOfType<ScreenManager>().GameIsOver();
                DestroyCurrentLevel();
            } else if (Level.CrowdManager.LevelComplete) {
                Level.CrowdManager.LevelComplete = false;
                DestroyCurrentLevel();
	            if (_currentLevel != Levels.Count - 1) {
                    FindObjectOfType<ScreenManager>().LevelCompleted();
	            } else {
                    FindObjectOfType<ScreenManager>().GameCompleted();
	            }
	        }
	    }
	}
}
