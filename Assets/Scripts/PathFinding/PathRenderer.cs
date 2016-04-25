using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.PathFinding {
    public enum Show {
        Observed = 0,
        Path = 1,
        From = 2,
        To = 3
    }

    public class PathRenderer : MonoBehaviour {
        private static Dictionary<Renderer, Color> DefaultColors = new Dictionary<Renderer, Color>();
        public DebugInformationAStar DebugInformation;

        public static void MapRebuild() {
            DefaultColors = new Dictionary<Renderer, Color>();
        }

        [UsedImplicitly]
        private IEnumerator Start() {
            yield return StartCoroutine("RendererPath");
        }

        [UsedImplicitly]
        public void OnDestroy() {
            foreach (var pair in DefaultColors) {
                pair.Key.material.SetColor("_Color", pair.Value);
            }
        }

        [UsedImplicitly]
        public IEnumerator RendererPath() {
            yield return AStarDebug(DebugInformation.From, Show.From);
            foreach (var informer in DebugInformation.Observed) {
                if (informer.InformerNode != DebugInformation.From && informer.InformerNode != DebugInformation.To) {
                    yield return AStarDebug(informer.InformerNode, Show.Observed);
                }
            }
            yield return AStarDebug(DebugInformation.To, Show.To);
            foreach (var informer in DebugInformation.FinalPath) {
                if (informer != DebugInformation.From && informer != DebugInformation.To) {
                    yield return AStarDebug(informer, Show.Path);
                }
            }
            Destroy(this);
        }

        private static IEnumerator AStarDebug(Component informer, Show show) {
            var component = informer.GetComponent<Renderer>();

            var color = component.material.GetColor("_Color");
            if (!DefaultColors.ContainsKey(component)) {
                DefaultColors.Add(component, color);
            }

            if (show == Show.Observed) {
                component.material.SetColor("_Color", Color.yellow);
            } else if (show == Show.Path) {
                component.material.SetColor("_Color", Color.red);
            } else if (show == Show.From) {
                component.material.SetColor("_Color", Color.cyan);
            } else {
                component.material.SetColor("_Color", Color.magenta);
            }
            yield return new WaitForSeconds(.01f);
        }
    }
}