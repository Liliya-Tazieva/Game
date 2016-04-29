using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Agents;
using Assets.Scripts.Map;
using Assets.Scripts.PathFinding;
using Random = System.Random;


public class CrowdManager : MonoBehaviour {

    public MapManager Map;
    public int AgentsAmount;
    public GameObject Agent;

    public Informer GetRandomInformer() {
        var controller = Map.GetComponent<Controller>();

        var nodes = controller.NodesTree
            .Select(kdTreeNode => kdTreeNode.Value)
            .ToList();

        nodes = nodes
            .Where(arg => arg.IsObstacle != true)
            .ToList();

        var rnd = new Random();
        int index = 0;
        index = rnd.Next(0, nodes.Count - 1);
        return nodes[index];
    }

    public List<Informer> FindPath(Informer from, Informer to) {
        var controller = Map.GetComponent<Controller>();
        var path = controller.AStar(from, to, controller.Radius);
        return path;
    }

    public void InstantiateAgents() {
        for (var i = 0; i < AgentsAmount; ++i) {
            Informer from = GetRandomInformer(), to = GetRandomInformer();
            while (from == to) {
                to = GetRandomInformer();
            }

            var temp =Instantiate(Agent, from.transform.position, Quaternion.identity) as GameObject;
            if (temp != null) {
                var agent = temp.GetComponent<AgentLogic>();
                agent.From = from;
                agent.To = to;
                agent.CrowdM = this;
                agent.Speed = 1.0f;
                agent.transform.position = new Vector3(agent.transform.position.x, agent.Height, agent.transform.position.z);
            }
        }
    }    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
