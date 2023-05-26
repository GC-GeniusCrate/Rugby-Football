using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeniusCrate.Utility;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] Transform mPreTile;
    [SerializeField] Transform mPlayerTransform;
    [SerializeField] float mDistanceToSpawn;

    [SerializeField] float SpawnZPosition = 6;
    private ThemeData _CurrentThemeData;
    [SerializeField] int _CurrentPlace;
    int m_PreviousSegment;
    [SerializeField] Transform initialTile;

    [Space(10)]
    [Header("Theme:- ")]
    public ThemeData mainTheme;




    private void OnEnable()
    {
        GameManager.BackToMainMenu += ResetGame;
        CharacterController.ChangeInTutorialState += SwitchPlace;
    }

    private void ResetGame()
    {
        mPreTile = initialTile;
    }

    private void OnDisable()
    {
        GameManager.BackToMainMenu -= ResetGame;
        CharacterController.ChangeInTutorialState -= SwitchPlace;


    }
    private void Start()
    {
        _CurrentThemeData = mainTheme;
    }
    private void Update()
    {
        if (Vector3.Distance(mPlayerTransform.position, mPreTile.position) < mDistanceToSpawn) SpawnTile();
    }
    void SpawnTile()
    {
        Vector3 _spawnLocation;
        if (GameManager.Instance._GameWorldType == WorldType.StrightWorld)
        {
            _spawnLocation = mPreTile.position + new Vector3(0, 0, SpawnZPosition);
        }
        else
        {
            if (Random.value < .75)
                _spawnLocation = mPreTile.position + new Vector3(0, 0, 3);
            else
                _spawnLocation = mPreTile.position + new Vector3(3, 0, 0);
        }

        int _tileSegmentIndex = Random.Range(1, _CurrentThemeData._Places[_CurrentPlace]._BuildingPrefabs.Length);
        if (_tileSegmentIndex == m_PreviousSegment) _tileSegmentIndex = (_tileSegmentIndex + 1) % _CurrentThemeData._Places[_CurrentPlace]._BuildingPrefabs.Length;
        GameObject _tileSegmentToSpawn = _CurrentThemeData._Places[_CurrentPlace]._BuildingPrefabs[_tileSegmentIndex];

        Tile spawnedTile = Instantiate(_tileSegmentToSpawn, _spawnLocation, _tileSegmentToSpawn.transform.rotation).GetComponent<Tile>();

        spawnedTile.PlayerPivot = mPlayerTransform;
        mPreTile = spawnedTile.transform;
        m_PreviousSegment = _tileSegmentIndex;
    }


    void SwitchPlace()
    {
        if (_CurrentPlace < _CurrentThemeData._Places.Length - 1)
            // _CurrentPlace++;
            return;
        else
        {
            _CurrentPlace = 0;
            _CurrentThemeData = mainTheme;
        }
    }
}
