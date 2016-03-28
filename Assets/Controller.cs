using UnityEngine;
using System.Linq;
using Accord.MachineLearning.Structures;

public class Controller : MonoBehaviour {
    private readonly KDTree<Informer> _tree = new KDTree<Informer>(3);

    public void RegisterInformer(Informer informer) {
        var position = informer.transform.position;
        Debug.Log(string.Format("Informer added at: {0}", position));
        _tree.Add(position.ToArray(), informer);
    }

    public Vector3 From;
    public Vector3 To;
    public double Radius;

    public void GetNearest() {
        var nearest = _tree.Nearest(From.ToArray(), Radius);
        var aggregate = nearest.Aggregate("",
            (s, neibour) => string.Format("{1}\n{0}", s, neibour.Node.Value.gameObject.name));
        Debug.Log(aggregate);
    }

    public void OnDrawGizmosSelected() {
        var color = Color.red;
        color.a *= 0.3f;
        Gizmos.color = color;
        Gizmos.DrawSphere(From, (float)Radius);
    }
}