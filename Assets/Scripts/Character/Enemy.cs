using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeniusCrate.Utility;

public class Enemy : MonoBehaviour
{
    [SerializeField] float Speed;
    public Transform mPlayerTransform;
    [SerializeField] Animator mEnemyAnimator;
    public float xPosValue;
   // public ParticleSystem destroyParticle;

    float keepDiatanceValue = 10;

    private void OnEnable()
    {
        GameManager.BackToMainMenu += ResetEnemy;
        GameManager.OnGameReEnter += ReEnterGame;
        GameManager.OnGameStart += StartGame;
        keepDiatanceValue = 3;

    }

    private void StartGame()
    {
        mEnemyAnimator.SetBool("Run", true);
    }

    private void ReEnterGame()
    {
        transform.position = new Vector3(xPosValue, 0,mPlayerTransform.position.z+ Random.Range(-2f, -1.5f));
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameStarted) return;
        if (GameManager.Instance.PauseGame) return;
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.player.m_IsPlayeralive)
        {
            transform.Translate(new Vector3(0,0,1) * (Speed )*Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(transform.position, new Vector3(0, mPlayerTransform.position.y, mPlayerTransform.position.z)) < keepDiatanceValue)
            {
                if (Speed > GameManager.Instance.player.mPlayerspeed)
                {
                    Speed = GameManager.Instance.player.mPlayerspeed;
                }
                Speed -= Random.Range(0.3f, 0.5f) * Time.deltaTime * 2.5f;
                Vector3 movePos = new Vector3(transform.position.x, transform.position.y, mPlayerTransform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, movePos, Time.deltaTime * Speed);
            }
            else
            {
                Speed = GameManager.Instance.player.mPlayerspeed + Random.Range(0f, .2f);
                Vector3 movePos = new Vector3(transform.position.x, transform.position.y, mPlayerTransform.position.z - 1);
                transform.position = Vector3.MoveTowards(transform.position, movePos, Time.deltaTime * Speed);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mEnemyAnimator.SetTrigger("Hit");
            other.GetComponent<CharacterController>().Die(true);
        }
        else if (!other.CompareTag("Ground"))
        {
            mEnemyAnimator.SetTrigger("Hit");
            //destroyParticle.Play();
            Destroy(other.gameObject);
        }
    }

    private void OnDisable()
    {
        GameManager.BackToMainMenu -= ResetEnemy;
        GameManager.OnGameReEnter -= ReEnterGame;
        GameManager.OnGameStart -= StartGame;


    }

    private void ResetEnemy()
    {
        transform.position = new Vector3(xPosValue, 0, Random.Range(-2f, -1.5f));
        Speed = 9;
        mEnemyAnimator.SetBool("Run", false);

    }
}
