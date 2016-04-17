﻿using System;
using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Structures;
using UnityEngine;

public class Controller : MonoBehaviour {
    public Vector3 From;
    public Vector3 To;
    public float Radius;
    public bool ShowVisited;
    public bool ShowPath;
    public KDTree<Informer> Tree = new KDTree<Informer>(3);

    public void RegisterInformer(Informer informer) {
        var position = informer.transform.position;
        Tree.Add(position.ToArray(), informer);
    }

    public void GetNearest() {
        var nearest = Tree.Nearest(From.ToArray(), Radius);
        var aggregate = nearest.Aggregate("",
            (s, neibour) => string.Format("{1}\n{0}", s, neibour.Node.Value.gameObject.name));
        Debug.Log(aggregate);
    }

    public List<Informer> A_star(Informer from, Informer to, float radius) {
        if (from == null || to == null) {
            Debug.Log("Can't run A*. Enter proper from and to parameters!");
            return null;
        }
        var current = from;
        current.Distance = current.Metrics(to);
        current.Visited = (NodeState) 1;
        var observed = new List<Informer>();
        observed.Add(current);
        while (current != to && current != null) {
            var query = Tree.Nearest(current.transform.position.ToArray(), radius).ToList();
            query =
                query.Where(
                    informer => informer != current && informer.IsObstacle != true && informer.Visited != (NodeState) 1)
                    .ToList();
            foreach (var informer in query) {
                if (informer.Visited == (NodeState) 2) {
                    informer.Distance = informer.Metrics(to);
                    informer.Visited = 0;
                    observed.Add(informer);
                }
            }
            observed = observed.OrderBy(arg => arg.Visited).ThenBy(arg => arg.Distance).ToList();
            if (observed[0].Visited != (NodeState) 1) {
                current = observed[0];
                observed[0].Visited = (NodeState) 1;
            } else {
                current = null;
            }
        }
        observed = observed.Where(informer => informer.Visited == (NodeState) 1).ToList();
        var finalPath = new List<Informer>();
        if (current != to) {
            Debug.Log("No path was found");
        } else {
            var path = new List<Informer>();
            path.Add(current);
            for (var i = 1; i < observed.Count; ++i) {
                var temp = current;
                var tempFrom = temp.Metrics(from);
                var flag = false;
                foreach (var informer in  observed) {
                    if (informer.Metrics(current) < 4.3 && informer.Visited == (NodeState) 1) {
                        var informerFrom = informer.Metrics(from);
                        if (tempFrom > informerFrom
                            || tempFrom <= informerFrom && flag == false) {
                            if (flag) observed.Find(x => x.transform.position == temp.transform.position).Visited = (NodeState) 1;
                            informer.Visited = (NodeState) 2;
                            temp = informer;
                            tempFrom = temp.Metrics(from);
                            flag = true;
                        }
                    }
                }
                if (!flag) {
                    observed.Find(x => x.transform.position==current.transform.position).Visited = (NodeState) 2;
                    path.RemoveAt(path.Count-1);
                    current = path[path.Count - 1];
                } else {
                path.Add(temp);
                current = temp;
                }
                if (current == from) {
                    break;
                }
            }
            for (var i = path.Count - 1; i >= 0; --i) {
                finalPath.Add(path[i]);
            }
            if (ShowVisited) {
                foreach (var t in observed) {
                    var component = t.GetComponent<Renderer>();
                    component.material.SetColor("_Color", Color.yellow);
                }
            }
            if (ShowPath) {
                foreach (var t in finalPath) {
                    var component = t.GetComponent<Renderer>();
                    component.material.SetColor("_Color", Color.red);
                }
            }
            var startRenderer = from.GetComponent<Renderer>();
            startRenderer.material.SetColor("_Color", Color.cyan);
            var endRenderer = to.GetComponent<Renderer>();
            endRenderer.material.SetColor("_Color", Color.magenta);
            Debug.Log("Final Path:");
            foreach (var t in finalPath) {
                Debug.Log(t.transform.name + " " + t.transform.position);
            }
        }
        return finalPath;
    }
}