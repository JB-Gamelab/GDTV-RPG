using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {

        private const string ATTACK = "attack";

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        
        private Transform target;
        private float timeSinceLastAttack = 0f;

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.position);
            } else {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour() {
            if (timeSinceLastAttack > timeBetweenAttacks) {
                GetComponent<Animator>().SetTrigger(ATTACK);
                timeSinceLastAttack = 0f;
            }
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }


        // Animation Event
        void Hit() {

        }
    }
}
