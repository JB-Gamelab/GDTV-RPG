using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {

        private const string ATTACK = "attack";
        private const string STOPATTACK = "stopAttack";

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform handTransform = null;
        [SerializeField] private Weapon weapon = null;
        
        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start() {
            SpawnWeapon();
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            } else {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void SpawnWeapon() {
            if (weapon == null) return;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
        }

        private void AttackBehaviour() {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks) {
                //This will trigger the Hit event
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack() {
            GetComponent<Animator>().ResetTrigger(STOPATTACK);
            GetComponent<Animator>().SetTrigger(ATTACK);
        }

        // Animation Event
        void Hit() {
            if (target == null) return;
            target.TakeDamage(weapon.GetDamage());
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.transform.position) < weapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack() {
            GetComponent<Animator>().ResetTrigger(ATTACK);
            GetComponent<Animator>().SetTrigger(STOPATTACK);
        }
    }
}
