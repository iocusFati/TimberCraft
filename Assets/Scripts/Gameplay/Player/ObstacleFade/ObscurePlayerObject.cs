using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.ObstacleFade
{
    public class ObscurePlayerObject : MonoBehaviour
    {
        public List<Renderer> Renderers = new List<Renderer>();
        public Vector3 Position;
        public List<Material> Materials = new List<Material>();
        [HideInInspector]
        public float InitialAlpha;

        private void Awake()
        {
            Position = transform.position;
            if (Renderers.Count == 0)
            {
                Renderers.AddRange(GetComponentsInChildren<Renderer>());
            }
            for (int i = 0; i < Renderers.Count; i++)
            {
                Materials.AddRange(Renderers[i].materials);
            }

            InitialAlpha = Materials[0].color.a;
        }

        public bool Equals(ObscurePlayerObject other)
        {
            return Position.Equals(other.Position);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
