using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Structures;
using Assets.Scripts.PathFinding;
using UnityEngine;

namespace Assets.Scripts.Core {
    public static class Extensions {
        public static double[] ToArray(this Vector3 vec) {
            return new double[] {vec.x, vec.y, vec.z};
        }

        public static float Metrics(this Informer from, Informer to) {
            var metric = (to.transform.position - from.transform.position).sqrMagnitude;
            return metric;
        }

        public static List<Node> ToList(this KDTreeNodeList<Informer> tree) {
            return tree
                .Select(node => new Node(node.Node.Value, NodeState.Undiscovered))
                .ToList();
        }

        public static List<Informer> LoopEscape(Informer from, Informer to, KDTree<Informer> loop) {
            var current = from;
            current.Distance = current.Metrics(to);
            var path = new List<Informer> {current};
            while (current != to) {
                var query = loop.Nearest(current.transform.position.ToArray(), 5)
                    .Select(informer => informer.Node.Value).ToList()
                    .Where(informer => informer.IsObstacle != true).ToList();
                foreach (var informer in query) {
                    informer.Distance = informer.Metrics(to);
                }
                query = query.Where(informer => informer.Distance < current.Distance)
                    .ToList().OrderBy(informer => informer.Distance).ToList();
                current = query[0];
                path.Add(current);
            }
            return path;
        }
    }
}