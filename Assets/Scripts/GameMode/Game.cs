using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Agents;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public List<GameObject> Levels;
    private int _currentLevel;
    private GameObject _levelInstanse;
    private LevelOptions _level;
    private Button Map;

    void OnGUI() {
        if (GUI.Button(new Rect(10, 70, 100, 30), "Show map")) {
            if (_levelInstanse == null) {
                _levelInstanse = Instantiate(Levels[_currentLevel], Vector3.zero, Quaternion.identity) as GameObject;
            }
            if (_levelInstanse != null) {
                _level = _levelInstanse.GetComponent<LevelOptions>();
            }
        }
        if (GUI.Button(new Rect(10, 100, 100, 30), "Start")) {
            if (_level != null) {
                _level.CrowdManager.InstantiateAgents();
            }
        }
        if (GUI.Button(new Rect(10, 130, 100, 30), "Restart")) {
            if (_level != null) {
                _level.CrowdManager.DestroyAgents();
            }
        }
        if (GUI.Button(new Rect(10, 160, 100, 30), "Next Level")) {
            if(_currentLevel!=Levels.Count-1)_currentLevel++;
            Destroy(_levelInstanse);
        }
        if (GUI.Button(new Rect(10, 190, 100, 30), "Previous Level")) {
            if (_currentLevel != 0) _currentLevel--;
            Destroy(_levelInstanse);
        }
    }

	// Use this for initialization
	void Start () {
	    _currentLevel = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if (_levelInstanse != null) {
            if (Input.GetButtonDown("Fire1")) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray,out hit)) {
                    var agent = hit.collider.gameObject;
                    if (agent.GetComponent<AgentLogic>() != null) {
                        Destroy(agent);
                        _level.CrowdManager.AgentsAmount--;
                    }
                }
            }
	    }
	
	}
}
