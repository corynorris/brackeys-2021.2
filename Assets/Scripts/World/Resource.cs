using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public Item item;
    private Health health;
    private SpriteRenderer sprite;
    private Color originalColor;


    private void Start()
    {
        sprite = transform.Find("sprite").GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        health = GetComponent<Health>();
        health.OnDied += Health_OnDied;
        health.OnTookDamage += Health_OnTookDamage;
    }

    private void Health_OnTookDamage(object sender, Health.DamageInfoEventArgs e)
    {
        StartCoroutine(Flash(0.1f));
    }

    private void Health_OnDied(object sender, Health.DamageInfoEventArgs eventArgs)
    {
        ItemWorld.DropItemInDirection(transform.position, item, eventArgs.direction);
        Destroy(this.gameObject);
    }

    IEnumerator Flash(float intervalTime)
    {

        for (int n = 0; n < 2; n++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(intervalTime);
            sprite.color = originalColor;
            yield return new WaitForSeconds(intervalTime);
        }
    }

    private void OnDestroy()
    {
        health.OnDied -= Health_OnDied;
        health.OnTookDamage -= Health_OnTookDamage;
    }
}
