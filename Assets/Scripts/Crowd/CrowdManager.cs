using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;
using Assets.Scripts.Agents;
using Assets.Scripts.Map;
using Assets.Scripts.PathFinding;
using Random = System.Random;


public class CrowdManager : MonoBehaviour {

    public MapManager Map;
    public int AgentsAmount;
    public GameObject Agent;
    private bool _instantiateFlag;
    public int LevelComplexity;
    public int LevelCollisionAmount;
    public bool GameOver;
    public bool LevelComplete;
    private int _startingAmount;
    private int _amountOfCollisions;

    public Informer GetRandomInformer() {
        var controller = Map.GetComponent<Controller>();

        var nodes = controller.NodesTree
            .Select(kdTreeNode => kdTreeNode.Value).Where(arg => arg.IsObstacle != true)
            .ToList();

        var rnd = new Random();
        var index = rnd.Next(0, nodes.Count - 1);
        return nodes[index];
    }

    public List<Informer> FindPath(Informer from, Informer to) {
        var controller = Map.GetComponent<Controller>();
        var path = controller.AStar(from, to, controller.Radius);
        return path;
    }

    public void StopAgents() {
        foreach (var agent in GetComponentsInChildren<AgentLogic>()) {
            agent.StopFlag = true;
        }
    }

    public void InstantiateAgentsCollision() {
        ++_amountOfCollisions;
        if (_instantiateFlag) {
            InstantiateAgents(1);
            _instantiateFlag = false;
            ++AgentsAmount;
        } else {
            _instantiateFlag = true;
        }
    }

    public void InstantiateAgents() {
        InstantiateAgents(AgentsAmount);
    }

    public void InstantiateAgents(int amount) {
        for (var i = 0; i < amount; ++i) {
            Informer from = GetRandomInformer(), to = GetRandomInformer();
            while (from == to) {
                to = GetRandomInformer();
            }

            var temp =Instantiate(Agent, from.transform.position, Quaternion.identity) as GameObject;
            if (temp != null) {
                temp.transform.SetParent(gameObject.transform);
                temp.AddComponent<Rigidbody>();
                var agent = temp.GetComponent<AgentLogic>();
                agent.From = from;
                agent.To = to;
                agent.CrowdM = this;
                agent.Speed = FindObjectOfType<Game>().AgentsSpeed;
                agent.transform.position = new Vector3(agent.transform.position.x, agent.Height, agent.transform.position.z);
            }
        }
    }

    public void DestroyAgents() {
        foreach (var agent in GetComponentsInChildren<AgentLogic>()) {
            DestroyImmediate(agent.gameObject);
        }
        AgentsAmount = _startingAmount;
    }

	// Use this for initialization
	void Start () {
	    _instantiateFlag = true;
	    GameOver = false;
	    LevelComplete = false;
	    _startingAmount = AgentsAmount;
	    _amountOfCollisions = 0;
	}
	
	// Update is called once per frame
    void Update() {
        if (AgentsAmount >= LevelComplexity || _amountOfCollisions==LevelCollisionAmount) {
            GameOver = true;
        }
        if (AgentsAmount == 0) {
            LevelComplete = true;
        }
	}
}
