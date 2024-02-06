using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamentusAI : NPCController
{
    public float xMovementLimit;
    public float attackTimeGap = 3f;
    public GameObject weapon;
    public float angleSpread = 15f;
    [SerializeField] protected AudioClip bossScreamAttackNoise;
    private Direction prevMove = Direction.Left;

    protected override void Start()
    {
        base.Start();
        TakesDamage health = GetComponent<TakesDamage>();
        weapon.GetComponent<DealsDamage>().affectEnemy = !health.isEnemy;
        weapon.GetComponent<DealsDamage>().affectPlayer = health.isEnemy;
        state = AIState.Aggression;
        StartCoroutine(AIAttack());
    }

    protected override IEnumerator AIMovement()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                Vector3 deltaPos = (transform.position - initialPosition);
                if (deltaPos.x > xMovementLimit)
                {
                    prevMove = Direction.Left;
                    mover.Move(prevMove);
                }
                else if (deltaPos.x < -xMovementLimit)
                {
                    prevMove = Direction.Right;
                    mover.Move(prevMove);
                }
                else
                {
                    mover.Move(prevMove);
                }
            }
            yield return new WaitForSeconds(movementWaitTime);
        }
    }

    private void ThrowFireball(Vector3 destination, int quantity)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Vector3 weaponOffset = weapon.GetComponent<DealsDamage>().spawnOffsetDistance * direction;

        for (int i = 0; i < quantity; i++)
        {
            float angle = angleSpread * ((float)i / (quantity - 1) - 0.5f) * 2;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedDir = rot * direction;
            GameObject weaponObj = Instantiate(weapon, transform.position + weaponOffset,
                rot);
            Debug.Log("WeaponObj: " + (weaponObj != null));
            if (weaponObj != null)
            {
                Projectile weaponScript = weaponObj.GetComponent<Projectile>();
                Debug.Log("Projectile Attached: " + (weaponScript != null));
                if (weaponScript != null)
                {
                    weaponScript.Shoot(rotatedDir);
                }

                ScriptAnim4Sprite animScript = weaponObj.GetComponent<ScriptAnim4Sprite>();
                Debug.Log("ScriptAnim4Sprite Attached: " + (animScript != null));
                if (animScript != null)
                {
                    animScript.active = true;
                }
            }
        }
    }

    private IEnumerator AIAttack()
    {
        while (true)
        {
            if (mover.movementEnabled)
            {
                AudioSource.PlayClipAtPoint(bossScreamAttackNoise, Camera.main.transform.position);
                ThrowFireball(player.transform.position, 3);
            }
            yield return new WaitForSeconds(attackTimeGap);
        }
    }
}
