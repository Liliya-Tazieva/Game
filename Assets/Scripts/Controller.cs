using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Accord.MachineLearning.Structures;

public class Controller : MonoBehaviour {
    public KDTree<Informer> Tree = new KDTree<Informer>(3);
    public Informer From;
    public Informer To;
    public float Radius;

    public void RegisterInformer(Informer informer) {
        var position = informer.transform.position;
        Debug.Log(string.Format("Informer added at: {0}", position));
        Tree.Add(position.ToArray(), informer);
    }

    public void GetNearest() {
        var nearest = Tree.Nearest(From.transform.position.ToArray(), Radius);
        var aggregate = nearest.Aggregate("",
            (s, neibour) => string.Format("{1}\n{0}", s, neibour.Node.Value.gameObject.name));
        Debug.Log(aggregate);
    }

    public List<Informer> A_star(Informer from, Informer to,float radius) {
        if (from == null || to == null) {
            Debug.Log("Can't run A*. Enter proper from and to parameters!");
            return null;
        }
        var current = from;
        current.Distance = current.Metrics(to);
        current.Visited = (NodeState) 1;
        var observed = new List<Informer>();
        observed.Add(current);
        while (current != to && current!=null) {
            List<Informer> query = Tree.Nearest(current.transform.position.ToArray(), radius).ToList();
            query =
                query.Where(informer => informer != current && informer.IsObstacle != true && informer.Visited !=(NodeState) 1)
                    .ToList();
            foreach (var informer in query)
                if (informer.Visited == (NodeState) 2) { 
                    informer.Distance = informer.Metrics(to);
                    informer.Visited = 0;
                    observed.Add(informer);
                }
            observed = observed.OrderBy(arg => arg.Visited).ThenBy(arg => arg.Distance).ToList();
            if (observed[0].Visited != (NodeState) 1) {
                current = observed[0];
                observed[0].Visited = (NodeState) 1;
            }
            else current = null;
        }
        observed = observed.Where(informer => informer.Visited==(NodeState) 1).ToList();
        var finalPath = new List<Informer>();
        if (current != to) Debug.Log("No path was found");
        else {
            var path = new List<Informer>();
            current.Visited = 0;
            path.Add(current);
            for (int i = 1; i < observed.Count; ++i) {
                var temp = current;
                float tempFrom = temp.Metrics(from);
                bool flag = false;
                foreach (var informer in  observed) {
                    if (informer.Metrics(current) < 4.3 && informer.Visited == (NodeState) 1) {
                        float informerFrom = informer.Metrics(from);
                        if (tempFrom > informerFrom
                            || tempFrom <= informerFrom && flag == false) {
                            informer.Visited = 0;
                            temp = informer;
                            tempFrom = temp.Metrics(from);
                            flag = true;
                        }
                    }
                }
                path.Add(temp);
                current = temp;
                if (current == from) break;
            }
            for (int i = path.Count - 1; i >= 0; --i)
                finalPath.Add(path[i]);
            Debug.Log("Final Path:");
            foreach (Informer t in finalPath) {
                Debug.Log(t.transform.name);
            }
        }
        return finalPath;
    } 

    public void OnDrawGizmosSelected() {
        var color = Color.red;
        color.a *= 0.3f;
        Gizmos.color = color;
        Gizmos.DrawSphere(From.transform.position, (float)Radius);
    }
}