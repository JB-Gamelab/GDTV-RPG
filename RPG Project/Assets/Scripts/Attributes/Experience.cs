﻿using UnityEngine;
using RPG.Saving;

namespace RPG.Attributes {
    public class Experience : MonoBehaviour, ISaveable {
        [SerializeField] private float experiencePoints = 0;

        public void GainExperience(float experience) {
            experiencePoints += experience;
        }

        public float GetPoints() {
            return experiencePoints; 
        }

        public object CaptureState() {
            return experiencePoints;
        }

        public void RestoreState(object state) {
            experiencePoints = (float)state;
        }
    }
}