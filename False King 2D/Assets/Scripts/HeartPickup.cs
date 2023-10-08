using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] AudioClip heartPickupSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(heartPickupSFX, collision.transform.position);
        FindAnyObjectByType<GameSession>().AddToLife();
        Destroy(gameObject);
    }
}
