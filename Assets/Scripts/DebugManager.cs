using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PathRenderer : MonoBehaviour {
    public DebugInformationA_Star DebugInformation;

    public IEnumerator Start() {
        yield return StartCoroutine("RenderPath");
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    IEnumerator A_StarDebug(Informer informer, Show show) {
        var component = informer.GetComponent<Renderer>();
        if (show == Show.Observed) {
            component.material.SetColor("_Color", Color.yellow);
        } else if (show == Show.Path) {
            component.material.SetColor("_Color", Color.red);
        } else if (show == Show.From) {
            component.material.SetColor("_Color", Color.cyan);
        } else {
            component.material.SetColor("_Color", Color.magenta);
        }
        yield return new WaitForSeconds(.2f);
    }

    public IEnumerator RenderPath() {
        yield return A_StarDebug(DebugInformation.From, Show.From);
        foreach (var informer in DebugInformation.Observed) {
            if (informer.InformerNode != DebugInformation.From && informer.InformerNode != DebugInformation.To) {
                yield return A_StarDebug(informer.InformerNode, Show.Observed);
            }
        }
        yield return A_StarDebug(DebugInformation.To, Show.To);
        foreach (var informer in DebugInformation.FinalPath) {
            if (informer != DebugInformation.From && informer != DebugInformation.To) {
                yield return A_StarDebug(informer, Show.Path);
            }
        }
        Destroy(this);
    }
}

public class DebugManager : MonoBehaviour {
    private readonly List<DebugInformationA_Star> _paths = new List<DebugInformationA_Star>();  

    public void AddPath(DebugInformationA_Star debugInformation) {
        _paths.Add(debugInformation);
    }

    void Update() {
        if (_paths.Any()) {
            foreach (var path in _paths) {
                var pathRenderer = gameObject.AddComponent<PathRenderer>();
                pathRenderer.DebugInformation = path;
            }
            _paths.Clear();
        }
    }
}
