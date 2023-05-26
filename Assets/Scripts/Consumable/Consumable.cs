using GeniusCrate.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    public string ConsumableName;
    public Sprite ConsumabeIcon;
    public float ConsumableDuration;
    public float m_SinceTime;
    public ConsumableType ConsumableType;
    public int ConsumableCost;
    public bool IsActive;
    public ParticleSystem ConsumableParticle;
    public static System.Action<Sprite, float,ConsumableType> OnConsumablePicked;



    public abstract ConsumableType GetConsumableType();

    public abstract string GetConsumableName();

    public abstract int GetConsumableCost();

    public virtual void StartIt(CharacterController c)
    {



    }


    public virtual void TickIt(CharacterController c)
    {


    }

    public virtual void EndIt(CharacterController c)
    {



    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartIt(other.GetComponent<CharacterController>());
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;
            OnConsumablePicked?.Invoke(ConsumabeIcon,ConsumableDuration,ConsumableType);
            this.transform.GetChild(0).gameObject.SetActive(false);
            other.GetComponent<CharacterController>().onPowerUpParticle.Play();

            AudioManager.Instance.PlaySoundOfType(SoundEffectType.PowerUp);

        }
    }


}

public enum ConsumableType
{
    Invincible,
    Magnet,
    FlamencoDancer
}
