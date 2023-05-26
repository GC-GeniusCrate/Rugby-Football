using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeniusCrate.Utility;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float laneChangeSpeed;
    public float mPlayerspeed;
    [SerializeField] float laneOffset;
    [SerializeField] float mJumpForce;

    Animator mCharacterAnimator;
    Rigidbody mCharaterRigidbody;

    Vector2 m_StartingTouch;
    Vector3 m_TargetPosition;

    bool m_Sliding;
    bool m_IsSwiping;
    public bool m_IsJumping;

    public bool m_IsPlayeralive;
    bool m_IsMagnetActive;
    bool m_IsInvincible;

    [SerializeField] LayerMask CoinLayerMask;
    [SerializeField] LayerMask ObstricleLayerMask;

    int m_CurrentLane = 1;

    public Transform PlayerPivot;

    IEnumerator magnetCorotine;
    IEnumerator invincibleCorotine;
    IEnumerator danceCorotine;

    public ParticleSystem onPowerUpParticle;
    public ParticleSystem destroyParticle;


    public static Action ChangeInTutorialState;
    public static Action<bool, string> ShowTutorialUI;
    int inputAccessState;

    private void Start()
    {
        mCharacterAnimator = GetComponent<Animator>();
        mCharaterRigidbody = GetComponent<Rigidbody>();

        GameManager.Instance.player = this;

    }
    void Update()
    {
       // Debug.Log("Test");
        transform.localPosition = Vector3.zero;
        if (GameManager.Instance.PauseGame) return;
        Debug.Log("Test");

        Vector3 verticalTargetPosition = m_TargetPosition;

        transform.parent.localPosition = Vector3.MoveTowards(transform.parent.localPosition, verticalTargetPosition, laneChangeSpeed * Time.deltaTime);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, 0), 5 * Time.deltaTime);

        GameManager.Instance._Score = (int)PlayerPivot.position.z * 10;
        GameManager.Instance.distance = PlayerPivot.position.z;
        if (!m_IsPlayeralive) return;
        if (GameManager.Instance.isGameStarted)
            PlayerPivot.Translate(Vector3.forward * mPlayerspeed * Time.deltaTime);


        CheckInput();

        if (m_IsMagnetActive) MagnetAttract();
    }

    void CheckInput()
    {
        #region Game Input
#if UNITY_EDITOR || UNITY_STANDALONE

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GameManager.Instance._GameWorldType == WorldType.StrightWorld)
                ChangeLane(1);
            else
                transform.Rotate(new Vector3(0f, -90f, 0f));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (GameManager.Instance._GameWorldType == WorldType.StrightWorld)
                ChangeLane(-1);
            else
                transform.Rotate(new Vector3(0f, 90f, 0f));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!m_Sliding)
                Slide();
        }
#endif

#if UNITY_IOS || UNITY_ANDROID
        // Use touch input on mobile
        if (Input.touchCount == 1)
        {
            if (m_IsSwiping)
            {
                Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;
                diff = new Vector2(diff.x / UnityEngine.Screen.width, diff.y / UnityEngine.Screen.height);

                if (diff.magnitude < 0.05f) return;

                if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                {
                    if (diff.y < 0)
                        Slide();
                    else
                        Jump();
                }
                else
                {

                    if (diff.x < 0)
                        ChangeLane(1);
                    else
                        ChangeLane(-1);


                }
                m_IsSwiping = false;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                m_StartingTouch = Input.GetTouch(0).position;
                m_IsSwiping = true;

            }

            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                m_IsSwiping = false;
            }
        }

#endif
        #endregion
    }
    private void Jump()
    {
        if (m_IsJumping) return;
        mCharacterAnimator.SetTrigger("Jump");
       // mCharaterRigidbody.AddForce(Vector3.up * mJumpForce);
        m_IsJumping = true;
        MissionManager.OnMissionTrigger?.Invoke(2, 1);
        AchievementManager.OnAchevement?.Invoke(0, 1);


    }

    private void Slide()
    {
        mCharacterAnimator.SetTrigger("Slide");
        MissionManager.OnMissionTrigger?.Invoke(4, 1);
        AchievementManager.OnAchevement?.Invoke(6, 1);



    }
    void Turn(float angle)
    {
        PlayerPivot.transform.Rotate(new Vector3(0f, angle, 0f));

    }

    private void ChangeLane(int direction)
    {
        int targetLane = m_CurrentLane + direction;

        if (targetLane < 0 || targetLane > 2)
            return;
        m_CurrentLane = targetLane;
        m_TargetPosition = new Vector3((m_CurrentLane - 1) * laneOffset, 0, 0);


    }

    public void StartGame()
    {
        m_IsPlayeralive = true;
        mCharacterAnimator.SetBool("Run", true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        m_IsJumping = false;

        if (collision.gameObject.CompareTag("DangerCollider"))
        {
            if (m_IsInvincible)
            {
                Destroy(collision.gameObject);
                // destroyParticle.Play();
            }
            else
                Die(false, collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {

            if (m_IsInvincible)
            {
                Destroy(collision.gameObject);
                //destroyParticle.Play();
            }
            else
                Die();
        }
    }

    public void MakeInvincible(float duration)
    {
        if (invincibleCorotine != null)
            StopCoroutine(invincibleCorotine);
        invincibleCorotine = Invincible(duration);
        StartCoroutine(invincibleCorotine);
    }

    IEnumerator Invincible(float time)
    {

        m_IsInvincible = true;
        yield return new WaitForSeconds(time);
        m_IsInvincible = false;

    }

    public void ActivateFlamencoDancer(float inDuration)
    {
        if (danceCorotine != null)
        {
            mPlayerspeed -= 2;
            StopCoroutine(danceCorotine);
            danceCorotine = null;
        }
        danceCorotine = FlamencoDancer(inDuration);
        StartCoroutine(danceCorotine);
    }
    IEnumerator FlamencoDancer(float duration)
    {
        mPlayerspeed += 4;
        yield return new WaitForSeconds(duration);
        mPlayerspeed -= 4;
        danceCorotine = null;

    }

    public void ActivateMagnet(float inDuration)
    {
        if (magnetCorotine != null)
            StopCoroutine(magnetCorotine);
        magnetCorotine = Magnet(inDuration);
        StartCoroutine(magnetCorotine);
    }

    IEnumerator Magnet(float duration)
    {

        m_IsMagnetActive = true;
        yield return new WaitForSeconds(duration);
        m_IsMagnetActive = false;
    }
    void MagnetAttract()
    {
        Collider[] returnColls = Physics.OverlapBox(transform.position, new Vector3(6, 3, 6), transform.rotation, CoinLayerMask);

        foreach (var item in returnColls)
        {
            item.GetComponent<Coin>().mTarget = this.transform;
        }
    }
    public void ReEnterGame()
    {
        DestroySurrounding();
        StartGame();
    }
    void DestroySurrounding()
    {
        Collider[] returnColls = Physics.OverlapBox(transform.position, new Vector3(6, 3, 20), transform.rotation, ObstricleLayerMask);

        foreach (var item in returnColls)
        {
            Destroy(item.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(6, 3, 6));
        Debug.DrawRay(transform.position - new Vector3(0, -.1f, 0), transform.TransformDirection(Vector3.forward) * 10, Color.red);

    }
    public void Die(bool hit = false, GameObject other = null)
    {
        if (hit)
        {
            mCharacterAnimator.SetTrigger("Die");
            GameManager.Instance.GameOver();
        }
        if (!m_IsPlayeralive) return;
        mCharaterRigidbody.AddForce(Vector3.back * 50);

        mCharacterAnimator.SetBool("Run", false);
        m_IsPlayeralive = false;
        AudioManager.Instance.PlaySoundOfType(SoundEffectType.Hit);

    }

    public bool CheckInvincibleAndDamage()
    {
        if (m_IsInvincible) return false;
        else Die();
        return true;
    }
    public void IncreasePlayerSpeed(float inPercent)
    {
        mPlayerspeed += mPlayerspeed * inPercent;
    }
    private void OnEnable()
    {
        GameManager.OnGameStart += StartGame;
        GameManager.OnGameReEnter += ReEnterGame;
        GameManager.BackToMainMenu += ResetPlayer;
    }
    private void OnDisable()
    {
        GameManager.OnGameStart -= StartGame;
        GameManager.OnGameReEnter -= ReEnterGame;
        GameManager.BackToMainMenu -= ResetPlayer;

    }

    private void ResetPlayer()
    {
        PlayerPivot.transform.position = Vector3.zero;
        mCharacterAnimator.Play("Idle");
        transform.parent.localPosition = Vector3.zero;
        m_TargetPosition = Vector3.zero;
    }
}
