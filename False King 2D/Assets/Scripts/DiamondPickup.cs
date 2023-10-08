using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickup : MonoBehaviour
{
    [SerializeField] AudioClip diamondPickupSFX;
    [SerializeField] int diamondValue = 100;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(diamondPickupSFX, collision.transform.position);
        FindAnyObjectByType<GameSession>().AddToScore(diamondValue);
        Destroy(gameObject);
    }
}
