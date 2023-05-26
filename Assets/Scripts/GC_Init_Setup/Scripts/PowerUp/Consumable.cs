using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeniusCrate.Utility
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class Consumable : MonoBehaviour
    {

        public string ConsumableName;
        public Sprite ConsumabeIcon;
        public ConsumableType ConsumableType;
        public ParticleSystem ConsumableParticle;
        public int ConsumableCost;
        public float ConsumableDuration;

        public int ConsumableLevel;

        public float m_SinceTime;
        public bool IsActive;

        public static System.Action<Sprite, float> OnConsumablePicked;



        public abstract ConsumableType GetConsumableType();

        public abstract string GetConsumableName();

        public abstract int GetConsumableCost();

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySoundOfType(SoundEffectType.PowerUp);

            }
        }

    }
}