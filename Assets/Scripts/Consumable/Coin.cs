using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeniusCrate.Utility;
using System;

public class Coin : MonoBehaviour
{
   [HideInInspector] public Transform mTarget;
    [SerializeField] float moveSpeed;
    public static Action OnCollectingCoin;
    private void Update()
    {
        if (mTarget != null)
        {
            transform.parent = null;
           transform.position=(Vector3.MoveTowards(transform.position, mTarget.transform.position+new Vector3(0,0,0), moveSpeed * Time.deltaTime));

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.UpdateCoin(1);
            OnCollectingCoin?.Invoke();
            MissionManager.OnMissionTrigger?.Invoke(0,1);
            AchievementManager.OnAchevement?.Invoke(4, 1);
            AudioManager.Instance.PlaySoundOfType(SoundEffectType.Collect);

            Destroy(this.gameObject);
        }
    }
}
