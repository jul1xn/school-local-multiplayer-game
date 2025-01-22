using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public Sprite triggeredSprite;
    public string triggerTag;
    public bool specificTrigger = false;
    public string triggerableBy;
    private bool hasbeentriggered = false;
    private SpriteRenderer spriteRenderer;
    public AudioSource triggerAudioSource;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasbeentriggered)
        {
            return;
        }

        if (specificTrigger)
        {
            if (collision.CompareTag(triggerableBy))
            {
                hasbeentriggered = true;
                spriteRenderer.sprite = triggeredSprite;
                GameObject obj = GameObject.FindWithTag(triggerTag);
                if (obj != null)
                {
                    obj.GetComponent<ButtonTriggerDoor>().TriggerDoor();
                    triggerAudioSource.Play();
                }
                else
                {
                    Debug.LogWarning("Could not find door with tag " + triggerTag);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            hasbeentriggered = true;
            spriteRenderer.sprite = triggeredSprite;
            GameObject obj = GameObject.FindWithTag(triggerTag);
            if (obj != null)
            {
                obj.GetComponent<ButtonTriggerDoor>().TriggerDoor();
                triggerAudioSource.Play();
            }
            else
            {
                Debug.LogWarning("Could not find door with tag " + triggerTag);
            }
        }
    }
}
