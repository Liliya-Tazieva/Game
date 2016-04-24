using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Structures;
using UnityEngine;

public static class Extensions {
    public static double[] ToArray(this Vector3 vec) {
        return new double[] {vec.x, vec.y, vec.z};
    }

    public static float Metrics(this Informer from, Informer to) {
        var metric = (to.transform.position - from.transform.position).sqrMagnitude;
        return metric;
    }

    public static List<Node> ToList(this KDTreeNodeList<Informer> tree) {
        var list = new List<Node>();
        foreach (var node in tree) {
            var value = new Node(node.Node.Value, NodeState.Undiscovered);
            list.Add(value);
        }
        return list;
    }

    public static List<Informer> LoopEscape(Node from, Node to, KDTree <Informer> nodesTree, float radius) {
        var current = from;
        current.Distance = current.InformerNode.Metrics(to.InformerNode);
        var path = new List<Informer> {current.InformerNode};
        while (current.InformerNode != to.InformerNode) {
            var query = nodesTree.Nearest(current.InformerNode.transform.position.ToArray(), radius).ToList();
            query =
                query.Where(
                    informer => informer.InformerNode != current.InformerNode
                        && informer.InformerNode.IsObstacle != true)
                    .ToList();
            foreach (var informer in query) {
                informer.Distance = informer.InformerNode.Metrics(to.InformerNode);
            }
            query = query.Where(informer => informer.Distance < current.Distance)
            .ToList().OrderBy(informer => informer.Distance).ToList();
            current = query[0];
            path.Add(current.InformerNode);
        }
        return path;
    }
}