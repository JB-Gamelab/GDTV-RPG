using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass charaterClass;
        [SerializeField] private Progression progression = null;

        public float GetHealth() {
            return progression.GetHealth(charaterClass, startingLevel);
        }

        public float GetExperienceReward() {
            return 10;
        }
    }
}
