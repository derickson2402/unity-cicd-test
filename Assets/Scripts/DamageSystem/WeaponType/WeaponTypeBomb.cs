using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponTypeBomb : MonoBehaviour
{
    private GameObject[] allEnemies;
    private List<GameObject> enemiesInRange;
    public float damage;
    public float distanceThreshold;
    public float timePerSprite;
    public GameObject ExplosionSpritePrefab;
    public List<Sprite> dustSprites;

    void Start()
    {
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesInRange = new List<GameObject>();
        foreach (GameObject enemy in allEnemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < distanceThreshold)
            {
                enemiesInRange.Add(enemy);
            }
        }
        Debug.Log("Bomb placed");
        StartCoroutine(CoroutineHelper.DelayFunction(4, Detonate));
    }

    void Detonate()
    {
        Debug.Log("Bomb detonating");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        foreach (GameObject enemy in enemiesInRange)
        {
            Debug.Log("Bomb damaging " + enemy.name + " for " + damage);
            enemy.GetComponent<TakesDamage>().Damage(damage);
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
        foreach (var t in dustSprites)
        {
            r.sprite = t;
            yield return new WaitForSeconds(timePerSprite);
        }
        Destroy(dustObject);
        Destroy(gameObject);
    }
}
