using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeniusCrate.Utility;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform mPlayerTransform;
    public float trailDistance = 5.0f;
    public float heightOffset = 3.0f;
    public float cameraDelay = 0.02f;
    [SerializeField] private Vector3 mOffSet;

    [SerializeField] ParticleSystem rainParticleSystem;
    [SerializeField] Light sunLight;
    bool isRaining;
    float camSpeed=10;
    private void OnEnable()
    {
        GameManager.OnGameReEnter += ResetValues;
        GameManager.OnGameOver += OnPlayerHit;
        GameManager.BackToMainMenu += ResetValues;
    }
    private void OnDisable()
    {
        GameManager.OnGameReEnter -= ResetValues;
        GameManager.OnGameOver -= OnPlayerHit;
        GameManager.BackToMainMenu -= ResetValues;
    }
    private void LateUpdate()
    {
        transform.position =Vector3.Lerp(transform.position, mPlayerTransform.position + mOffSet,Time.deltaTime* camSpeed);
        /* Vector3 followPos = mPlayerTransform.position - mPlayerTransform.forward * trailDistance;

         followPos.y += heightOffset;
         transform.position += (followPos - transform.position) * cameraDelay;
         transform.LookAt(mPlayerTransform);*/
        if (Mathf.RoundToInt(GameManager.Instance.distance) % 100 == 1 && GameManager.Instance.isGameStarted)
        {
            if (isRaining) MakeSunnyDay();
            else MakeRain();
        }
    }

    void MakeRain()
    {
        rainParticleSystem.Play();
        sunLight.intensity = 0.05f;
        isRaining = true;
    }
    void MakeSunnyDay()
    {
        rainParticleSystem.Stop();
        sunLight.intensity = 0.5f;
        isRaining = false;
    }

    void OnPlayerHit()
    {
        mOffSet.z = -7f;
        camSpeed = 5f;
    }

    void ResetValues()
    {
        camSpeed = 10f;
        mOffSet.z = -5.5f;
    }
}
