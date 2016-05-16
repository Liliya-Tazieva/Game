using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.Scripts.Agents;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public List<GameObject> Levels;
    private int _currentLevel;
    private GameObject _levelInstanse;
    private LevelOptions _level;

    public void InstantiateMap(Transform parent) {
        if (_levelInstanse == null) {
            _levelInstanse = Instantiate(Levels[_currentLevel], Vector3.zero, Quaternion.identity) as GameObject;
            if (_levelInstanse != null) {
                _levelInstanse.transform.SetParent(parent);
                _level = _levelInstanse.GetComponent<LevelOptions>();
            }
        }
    }

    public void StartGame() {
        _level.CrowdManager.InstantiateAgents();
    }

    public void Restart() {
        if (_level != null) {
                _level.CrowdManager.DestroyAgents();
                _level.CrowdManager.InstantiateAgents();
            }
    }

    public void DestroyCurrentLevel() {
        if (_level.CrowdManager != null) {
            _level.CrowdManager.DestroyAgents();

        }
        DestroyImmediate(_levelInstanse.gameObject);
    }

    public void PreviousLevel(bool flag) {
        DestroyCurrentLevel();
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
	}
	
	// Update is called once per frame
	void Update () {
	    if (_levelInstanse != null) {
            if (Input.GetButtonDown("Fire1")) {
                RaycastHit hit;
                var cam = FindObjectOfType<Camera>();
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    var agent = hit.collider.gameObject;
                    if (agent.GetComponent<AgentLogic>() != null) {
                        Destroy(agent);
                        _level.CrowdManager.AgentsAmount--;
                    }
                }
            }
            if (_level.CrowdManager.GameOver) {
                _level.CrowdManager.GameOver = false;
                GetComponentInParent<ScreenManager>().GameIsOver();
                DestroyCurrentLevel();
            } else if (_level.CrowdManager.LevelComplete) {
                _level.CrowdManager.LevelComplete = false;
                DestroyCurrentLevel();
	            if (_currentLevel != Levels.Count - 1) {
                    GetComponentInParent<ScreenManager>().LevelCompleted();
	            } else {
                    GetComponentInParent<ScreenManager>().GameCompleted();
	            }
	        }
	    }
	}
}
