using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

public class LightAttack : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private Vector3 halfExtents;
    [SerializeField] float damage = 10f;
    private HashSet<Damageable> processedHits = new HashSet<Damageable>();
    private static Collider[] buffer = new Collider[3];
    private LightAttack actor;
    private EnemyAI enemyAI;
    public Health Health { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        actor = GetComponent<LightAttack>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    
    public void HandleSwing()
    {

        int hits = Physics.OverlapBoxNonAlloc(transform.position, halfExtents, buffer, transform.rotation, hitLayerMask, QueryTriggerInteraction.Ignore);
        if (hits == 0)
            return;

        for (int i = 0; i < hits; i++)
            DamageCollider(buffer[i]);
        processedHits.Clear();
    }
    private void DamageCollider(Collider collider)
    {
        // Ignore player
        if (collider.gameObject == actor.gameObject || collider.gameObject == enemyAI.gameObject)
            return;

        Damageable damageable = collider.GetComponent<Damageable>();

        if (damageable == null)
            return;

        // Ignore already damaged
        if (processedHits.Contains(damageable))
            return;
        processedHits.Add(damageable);
        damageable.InflictDamage(damage, false, enemyAI.gameObject);
    }
}
