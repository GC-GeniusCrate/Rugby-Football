using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using GeniusCrate.Utility;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] AssetReference character;
    [SerializeField] Transform characterSlot;
    Transform playerTransform;

    [Header("Enemy Reference")]
    [SerializeField] AssetReference enemy;
    AssetReference[] assetReferences;
    List<Enemy> enemies = new List<Enemy>();
    private void OnEnable()
    {
        GameManager.OnCharacterChange += OnCharacterChange;
    }
    private void OnDisable()
    {
        GameManager.OnCharacterChange -= OnCharacterChange;

    }
    private void Start()
    {

        /* character.InstantiateAsync(characterSlot).Completed += (newCharacter) =>
         {
             newCharacter.Result.gameObject.GetComponent<CharacterController>().PlayerPivot = this.transform;
             playerTransform = newCharacter.Result.transform;
         };*/
        OnCharacterChange(GameManager.Instance.mSelectedCharacterKey);
        /*  for (int i = 0; i < 3; i++)
          {
              float posX = i - 1;
              float posZ = Random.Range(-2.75f, -1.75f);
              enemy.InstantiateAsync(new Vector3(posX, 0, posZ), Quaternion.identity).Completed += (newEnemy) =>
              {

                  Enemy _enemy = newEnemy.Result.GetComponent<Enemy>();
                  _enemy.xPosValue = posX;
                  _enemy.mPlayerTransform = playerTransform;
                  _enemy.keepDiatanceValue = Vector3.Distance(_enemy.transform.position, playerTransform.position) + 1.5f;
                  enemies.Add(_enemy);
              };
          }*/
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        int i = 0;
        while (i < 3)
        {
            float posX = i - 1;
            float posZ = Random.Range(-2f, -1.5f);
            enemy.InstantiateAsync(new Vector3(posX, 0, posZ), Quaternion.identity).Completed += (newEnemy) =>
            {

                Enemy _enemy = newEnemy.Result.GetComponent<Enemy>();

                _enemy.transform.localScale = new Vector3(.75f, .75f, .75f);

                _enemy.xPosValue = posX;
                _enemy.mPlayerTransform = playerTransform;
              //  _enemy.keepDiatanceValue = Vector3.Distance(_enemy.transform.position, new Vector3(_enemy.transform.position.x, _enemy.transform.position.y, playerTransform.position.z)) +1f;
                enemies.Add(_enemy);
            };
            yield return new WaitForSeconds(0.25f);
            i++;
        }
    }
    private void OnCharacterChange(string key)
    {
        if (playerTransform) Destroy(playerTransform.gameObject);
        Addressables.InstantiateAsync(key, characterSlot).Completed += (newChar) =>
        {
            newChar.Result.gameObject.GetComponent<CharacterController>().PlayerPivot = this.transform;

            newChar.Result.transform.localScale = new Vector3(.75f, .75f, .75f);

            playerTransform = newChar.Result.transform;
            foreach (var enemy in enemies)
            {
                enemy.mPlayerTransform = playerTransform;
            }
        };

    }
}
