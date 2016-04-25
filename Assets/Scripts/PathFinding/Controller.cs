using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Structures;
using Assets.Scripts.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.PathFinding {
    public class Controller : MonoBehaviour {
        public DebugManager DebugManagerAStar;

        [UsedImplicitly]
        public Vector3 From;

        public KDTree<Informer> NodesTree = new KDTree<Informer>(3);

        [UsedImplicitly]
        public float Radius;

        [UsedImplicitly]
        public Vector3 To;

        public void RegisterInformer(Informer informer) {
            var position = informer.transform.position;
            NodesTree.Add(position.ToArray(), informer);
        }

        //Avoid at all costs!
        public void RemoveEmpty() {
            var newNodesTree = new KDTree<Informer>(3);
            foreach (var kdTreeNode in NodesTree) {
                if (kdTreeNode.Value != null && kdTreeNode.Value) {
                    newNodesTree.Add(kdTreeNode.Position, kdTreeNode.Value);
                }
            }
            NodesTree = newNodesTree;
        }

        public void GetNearest() {
            var nearest = NodesTree.Nearest(From.ToArray(), Radius);
            var aggregate = nearest.Aggregate("",
                (s, neibour) => string.Format("{1}\n{0}", s, neibour.Node.Value.gameObject.name));
            //Debug.Log(aggregate);
        }

        public void InitializeDebugInfo() {
            if (DebugManagerAStar == null) {
                DebugManagerAStar = GetComponent<DebugManager>();
            }
        }

        [UsedImplicitly]
        public List<Informer> AStar(Informer from, Informer to, float radius) {
            DebugInformationAStar debugInformation;
            var finalPath = AStar(from, to, radius, false, out debugInformation);
            return finalPath;
        }

        public List<Informer> AStar(Informer from, Informer to, float radius, bool debugFlag,
            out DebugInformationAStar debugInformation) {
            if (from == null || to == null) {
                Debug.LogError("Can't run A*. Enter proper from and to parameters!");
                debugInformation = null;
                return null;
            }
            if (debugFlag) {
                debugInformation = new DebugInformationAStar {
                    From = from,
                    To = to,
                    Observed = new List<Node>(),
                    FinalPath = new List<Informer>()
                };
            } else {
                debugInformation = null;
            }
            var current = new Node(from, NodeState.Processed);
            current.Distance = current.InformerNode.Metrics(to);
            var observed = new List<Node> {current};
            // ReSharper disable once PossibleNullReferenceException

            while (current.InformerNode != null && current.InformerNode != to) {
                var query = NodesTree.Nearest(current.InformerNode.transform.position.ToArray(), radius).ToList();
                query =
                    query.Where(
                        informer => informer.InformerNode != current.InformerNode
                                    && informer.InformerNode.IsObstacle != true && informer.Visited != NodeState.Processed )
                        .ToList();
                foreach (var informer in query) {
                    if (informer.Visited == (NodeState) 2) {
                        informer.Distance = informer.InformerNode.Metrics(to);
                        informer.Visited = 0;
                        observed.Add(informer);
                    }
                }
                observed = observed.OrderBy(arg => arg.Visited).ThenBy(arg => arg.Distance).ToList();
                if ( observed[0].Visited != NodeState.Processed ) {
                    current = observed[0];
                    observed[0].Visited = NodeState.Processed;
                    if (debugInformation != null) {
                        debugInformation.Observed.Add(observed[0]);
                    }
                } else {
                    current = null;
                }
            }
            observed = observed.Where( informer => informer.Visited == NodeState.Processed ).ToList();
            var finalPath = new List<Informer>();
            if (current.InformerNode != to) {
                Debug.LogError("No path was found");
            } else {
                var path = new List<Node> {current};
                while (current.InformerNode != from) {
                    var temp = current;
                    var tempFrom = temp.InformerNode.Metrics(from);
                    var flag = false;
                    foreach (var informer in  observed) {
                        if (informer.InformerNode.Metrics(current.InformerNode) < 18.1 &&
                            informer.Visited == (NodeState) 1) {
                            var informerFrom = informer.InformerNode.Metrics(from);
                            if (tempFrom > informerFrom
                                || tempFrom <= informerFrom && flag == false) {
                                if (flag) {
                                    observed.Find(arg => arg.InformerNode.transform.position
                                                         == temp.InformerNode.transform.position).Visited =
                                        (NodeState) 1;
                                }
                                informer.Visited = (NodeState) 2;
                                temp = informer;
                                tempFrom = temp.InformerNode.Metrics(from);
                                flag = true;
                            }
                        }
                    }
                    if (!flag) {
                        path.RemoveAt(path.Count - 1);
                        current = path[path.Count - 1];
                    } else {
                        path.Add(temp);
                        current = temp;
                    }
                }
                var loopflag = false;
                Node loopstart = null;
                for (var i = path.Count - 1; i >= 0; --i) {
                    var intersection = NodesTree.Nearest(path[i].InformerNode.transform.position.ToArray(), radius)
                        .ToList().Intersect(path).ToList().Count;
                    if (intersection > 3) {
                        if (!loopflag) {
                            loopflag = true;
                            int index;
                            if (i < path.Count - 1) {
                                index = i + 1;
                            } else {
                                index = i;
                            }
                            loopstart = path[index];
                            finalPath.Remove( loopstart.InformerNode );
                            //Debug.Log("Loopstart: " + loopstart.transform.position);
                        }
                    } else {
                        int index;
                        if (i > 0) {
                            index = i - 1;
                        } else {
                            index = i;
                        }
                        if (NodesTree.Nearest(path[index].InformerNode.transform.position.ToArray(), radius)
                            .ToList().Intersect(path).ToList().Count <= 3) {
                            if (loopflag) {
                                loopflag = false;
                                var loopend = path[i];
                                //Debug.Log("Loopend: " + loopend.transform.position);
                                var loopescape = Extensions.LoopEscape( loopstart, loopend, NodesTree, radius);
                                finalPath.AddRange(loopescape);
                                loopstart = null;
                            } else {
                                finalPath.Add(path[i].InformerNode);
                            }
                        }
                    }
                }
                debugInformation.FinalPath = finalPath;
                //Debug.Log("Final Path:");
                foreach (var informer in finalPath) {
                    //Debug.Log(informer.transform.name + " " + informer.transform.position);
                }
            }
            return finalPath;
        }
    }
}