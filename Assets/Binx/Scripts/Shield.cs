using JetBrains.Annotations;
using Unity.FPS.Game;
using UnityEngine;

namespace Binx
{
    public class Shield : MonoBehaviour
    {
        private static int isBlockingHash = Animator.StringToHash("isBlocking");

        [SerializeField] private Animator animator;

        public bool IsBlocking => isBlocking;

        private Actor actor;
        private bool isBlocking;

        private void Awake()
        {
            actor = GetComponentInParent<Actor>();
            Damageable damageable = actor.GetComponent<Damageable>();
            damageable.DamageModifiers += HandleDamage;
        }

        private float HandleDamage(float damage)
        {
            return isBlocking ? 0f : damage;
        }
        
        public void HandleInput(bool blockInputHeld)
        {
            animator.SetBool(isBlockingHash, blockInputHeld);
        }
        
        /// <summary>
        /// Called from Animator as event
        /// </summary>
        [UsedImplicitly]
        private void StartBlockEvent()
        {
            isBlocking = true;
        }
        
        /// <summary>
        /// Called from Animator as event
        /// </summary>
        [UsedImplicitly]
        private void EndBlockEvent()
        {
            isBlocking = false;
        }
    }
}