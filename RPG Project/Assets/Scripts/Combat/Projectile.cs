using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {

    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed = 1;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifetime = 10;
        [SerializeField] private GameObject[] destoryOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2;

        Health target = null;
        GameObject instigator = null;
        float damage = 0f;

        private void Start() {
            transform.LookAt(GetAimLocation());
        }

        void Update() {
            if (target == null) return;
            if (isHoming && !target.IsDead()) {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage) {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifetime);
        }

        private Vector3 GetAimLocation() {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            speed = 0;

            if (hitEffect != null) {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destoryOnHit) {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
