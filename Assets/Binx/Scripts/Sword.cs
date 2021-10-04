using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Binx
{
    public class Sword : MonoBehaviour
    {
        private static int isSwingingHash = Animator.StringToHash("isSwinging");
        private static Collider[] buffer = new Collider[3];
        
        [SerializeField] private Animator animator;
        [SerializeField] private float damage;
        
        [Header("Collider")]
        [SerializeField] private LayerMask hitLayerMask;
        [SerializeField] private Vector3 halfExtents;

        public bool IsSwinging => isSwinging;
        
        private Actor actor;
        private HashSet<Damageable> processedHits = new HashSet<Damageable>();
        private bool isSwinging;
        private bool isScanning;
        private new Transform transform;

        private void Awake()
        {
            actor = GetComponentInParent<Actor>();
            transform = GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            HandleSwing();
        }
        
        public void HandleInput(bool swingInputHeld)
        {
            animator.SetBool(isSwingingHash, swingInputHeld);
        }
        
        private void HandleSwing()
        {
            if (!isScanning)
                return;

            int hits = Physics.OverlapBoxNonAlloc(transform.position, halfExtents, buffer, transform.rotation, hitLayerMask, QueryTriggerInteraction.Ignore);
            if (hits == 0)
                return;

            for (int i = 0; i < hits; i++)
                DamageCollider(buffer[i]);
        }

        private void DamageCollider(Collider collider)
        {
            // Ignore player
            if (collider.gameObject == actor.gameObject)
                return;
            
            Damageable damageable = collider.GetComponent<Damageable>();
            
            if (damageable == null)
                return;

            // Ignore already damaged
            if (processedHits.Contains(damageable))
                return;
            processedHits.Add(damageable);
            damageable.InflictDamage(damage, false, actor.gameObject);
        }
        
        /// <summary>
        /// Called from Animator as event
        /// </summary>
        [UsedImplicitly]
        private void StartSwingEvent()
        {
            isSwinging = true;
        }
        
        /// <summary>
        /// Called from Animator as event
        /// </summary>
        [UsedImplicitly]
        private void EndSwingEvent()
        {
            isSwinging = false;
        }

        /// <summary>
        /// Called from Animator as event
        /// </summary>
        [UsedImplicitly]
        private void StartScanEvent()
        {
            processedHits.Clear();
            isScanning = true;
        }
        
        /// <summary>
        /// Called from Animator as event
        /// </summary>
        [UsedImplicitly]
        private void EndScanEvent()
        {
            isScanning = false;
        }
    }
}