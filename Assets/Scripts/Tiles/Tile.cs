using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeniusCrate.Utility;

public class Tile : MonoBehaviour
{
    [SerializeField] List<GameObject> Obstricle;
    [SerializeField] List<GameObject> collectables;
    [SerializeField] List<GameObject> Consumables;
    public Transform PlayerPivot;
    Vector3 mObstricleSpawnedPos;
    public float PowerUpChance;
    [SerializeField] int countOfObstricle = 4;
    float tileLength = -50;
    public float TutSpawntime;
    private void OnEnable()
    {
        GameManager.BackToMainMenu += DestroyTile;
    }
    private void OnDisable()
    {
        GameManager.BackToMainMenu -= DestroyTile;

    }

    private void DestroyTile()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        if (Random.value > .25)
        {
            Vector3 _spawnPos = GetRandomLane();
            float TileEnd = 6f;
            float TileStart = -TileEnd;
            float spawnY = mObstricleSpawnedPos.x == _spawnPos.x ? 2f : 1f;

            if (Random.value < PowerUpChance)
            {
                if (Consumables.Count > 0)
                {
                    GameObject consumable = Instantiate(Consumables[Random.Range(0, Consumables.Count)], this.transform);
                    consumable.transform.localPosition = GetRandomLane() + new Vector3(0, spawnY, Random.Range(TileStart, TileEnd));
                }
            }
            if (collectables.Count > 0)
            {
                StartCoroutine(SpawnCollectables(_spawnPos, TileStart, TileEnd, spawnY));
            }
        }

        if (GameManager.Instance.gameTime < TutSpawntime) return;

        for (int i = 0; i < countOfObstricle; i++)
        {
            Vector3 _spawnPos = GetRandomLane();
            tileLength += 15;
            _spawnPos.z = tileLength;

            if (Obstricle.Count > 0)
            {
                GameObject obstricle;

                    obstricle = Instantiate(Obstricle[Random.Range(0, Obstricle.Count)], this.transform);

                obstricle.GetComponent<Obstricle>().PlayerPivot = PlayerPivot;
                obstricle.transform.localPosition = _spawnPos + new Vector3(0, .2f, 0);
                mObstricleSpawnedPos = _spawnPos + new Vector3(0, .5f, 0);
            }
        }

    }


    IEnumerator SpawnCollectables(Vector3 inSpawnPos, float inTileStart, float inTileEnd, float inSpawnY)
    {
        while (inTileStart <= inTileEnd)
        {
            GameObject collectable = Instantiate(collectables[Random.Range(0, collectables.Count)], this.transform);
            collectable.transform.localPosition = inSpawnPos + new Vector3(0, inSpawnY, inTileStart);
            inTileStart += 2f;
            yield return new WaitForSeconds(.1f);
        }
    }
    Vector3 GetRandomLane()
    {
        float posRandom = Random.value;
        if (posRandom < .5)
            return Vector3.zero;
        else if (posRandom < .75f)
            return Vector3.left - new Vector3(1f, 0, 0);
        else
            return Vector3.right + new Vector3(1f, 0, 0);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerPivot.position) > 50 * 5) Destroy(this.gameObject);
    }
}
