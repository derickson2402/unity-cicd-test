using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealsDamageExploder : DealsDamage
{
    private GameObject[] allEnemies;
    private List<GameObject> enemiesInRange;
    public float distanceThreshold;
    public float timePerSprite;
    public GameObject ExplosionSpritePrefab;
    public List<Sprite> dustSprites;

    protected override void ProcessInteraction(Collider collider)
    {
        Detonate();
    }

    void Detonate()
    {
        Debug.Log("Fireball detonating");
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesInRange = new List<GameObject>();
        foreach (GameObject enemy in allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < distanceThreshold)
            {
                enemiesInRange.Add(enemy);
            }
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        foreach (GameObject enemy in enemiesInRange)
        {
            Debug.Log("Fireball damaging " + enemy.name + " for " + damageHP);
            enemy.GetComponent<TakesDamage>().Damage(damageHP);
        }

        float spriteSize = 1f;
        int radialSteps = Mathf.RoundToInt(distanceThreshold / spriteSize);
        for (int r = 0; r <= radialSteps; r++)
        {
            float radius = r * spriteSize;
            //minimum of 1 so that center dust is generated
            int numSprites = Mathf.Max(1, Mathf.RoundToInt(2 * Mathf.PI * radius));

            for (int i = 0; i < numSprites; i++)
            {
                float angle = i * (360f / numSprites);
                Vector3 spawnPosition = transform.position +
                                        Quaternion.Euler(0, 0, angle) * Vector3.right * radius;
                GameObject dustObject = Instantiate(ExplosionSpritePrefab, spawnPosition, Quaternion.identity);
                StartCoroutine(SwapSprites(dustObject));
            }
        }
    }

    IEnumerator SwapSprites(GameObject dustObject)
    {
        SpriteRenderer r = dustObject.GetComponent<SpriteRenderer>();
        // time interval between sprite switches is timePerSprite
        // total time is 3 * timePerSprite
        float tempTime = timePerSprite;
        foreach (var t in dustSprites)
        {
            r.sprite = t;
            yield return new WaitForSeconds(tempTime);
            tempTime /= 1.5f;
            dustObject.transform.localScale /= 2f;
        }
        Destroy(dustObject);
        Destroy(gameObject);
    }
}
