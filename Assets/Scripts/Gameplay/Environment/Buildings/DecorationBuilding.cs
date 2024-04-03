using UnityEngine;

namespace Gameplay.Environment.Buildings
{
    public class DecorationBuilding : Building
    {
        [Header("Decoration")]
        [SerializeField] private Collider _constructionTrigger;
        public override void InteractWithPlayer() { }
        public override void StopInteractingWithPlayer() { }

        protected override void OnBuilt()
        {
            base.OnBuilt();

            _constructionTrigger.enabled = false;
        }
    }
}