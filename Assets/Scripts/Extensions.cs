using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Structures;
using UnityEngine;

public static class Extensions {
    public static double[] ToArray(this Vector3 vec) {
        return new double[] {vec.x, vec.y, vec.z};
    }

    public static float Metrics(this Informer from, Informer to) {

        float metric = Mathf.Pow(to.transform.position.x - from.transform.position.x, 2);
        metric += Mathf.Pow(to.transform.position.y - from.transform.position.y, 2);
        metric += Mathf.Pow(to.transform.position.z - from.transform.position.z, 2);
        metric = Mathf.Sqrt(metric);
        return metric;
    }

    public static List<Informer> ToList(this KDTreeNodeList<Informer> l) {
        return l.Select(informer => informer.Node.Value).ToList();
    }
}