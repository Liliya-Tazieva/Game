using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Accord.MachineLearning.Structures;

public class Controller : MonoBehaviour {
    public KDTree<Informer> Tree = new KDTree<Informer>(3);
    public Informer From;
    public Informer To;
    public double Radius;

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

    public List<Informer> A_star() {
        var current = From;
        current.Distance = current.Metrics(To);
        current.Visited = 1;
        var observed = new List<Informer>();
        observed.Add(current);
        while (current != To && current!=null) {
            List<Informer> query = Tree.Nearest(current.transform.position.ToArray(), Radius).ToList();
            query =
                query.Where(informer => informer != current && informer.IsObstacle != true && informer.Visited !=1)
                    .ToList();
            foreach (var informer in query)
                if (informer.Visited == 2) { 
                    informer.Distance = informer.Metrics(To);
                    informer.Visited = 0;
                    observed.Add(informer);
                }
            observed = observed.OrderBy(arg => arg.Visited).ThenBy(arg => arg.Distance).ToList();
            if (observed[0].Visited != 1) {
                current = observed[0];
                observed[0].Visited = 1;
            }
            else current = null;
        }
        observed = observed.Where(informer => informer.Visited==1).ToList();
        var finalPath = new List<Informer>();
        if (current != To) Debug.Log("No path was found");
        else {
            var path = new List<Informer>();
            current.Visited = 0;
            path.Add(current);
            for (int i = 1; i < observed.Count; ++i) {
                var temp = current;
                float tempFrom = temp.Metrics(From);
                bool flag = false;
                foreach (var informer in  observed) {
                    if (informer.Metrics(current) < 4.3 && informer.Visited == 1) {
                        float informerFrom = informer.Metrics(From);
                        if (tempFrom > informerFrom
                            || tempFrom <= informerFrom && flag == false) {
                            informer.Visited = 0;
                            temp = informer;
                            tempFrom = temp.Metrics(From);
                            flag = true;
                        }
                    }
                }
                path.Add(temp);
                current = temp;
                if (current == From) break;
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