using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeniusCrate.Utility;

public class Obstricle : MonoBehaviour
{
    [SerializeField] ObstricleType m_ObstricleType;
    [SerializeField] float moveSpeed;
    public Transform PlayerPivot;
    Rigidbody rb;
    BoxCollider collider;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if (PlayerPivot == null || !GameManager.Instance.isGameStarted) return;
        if (Vector3.Distance(transform.position, PlayerPivot.position) < 20)
        {
            if (m_ObstricleType == ObstricleType.Moving)
            {
                // transform.Translate(Vector3.forward * moveSpeed*Time.deltaTime);
                rb.MovePosition(transform.position + Vector3.back * moveSpeed * Time.deltaTime);

            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (!collision.gameObject.CompareTag("Ground"))
        {
            m_ObstricleType = ObstricleType.Static;
        }
        if (collision.gameObject.CompareTag("Player") && m_ObstricleType == ObstricleType.Moving)
        {

            if (!collision.gameObject.GetComponent<CharacterController>().CheckInvincibleAndDamage())
            {
                Destroy(this.gameObject);
            }

        }
        if (collision.gameObject.CompareTag("Consumable"))
        {
            collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 1.5f, collision.gameObject.transform.position.z);
        }
    }
}

public enum ObstricleType
{
    Static,
    Moving
}
