using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 25f;
    public int damage = 1;
    public float attackCooldown = 0.5f;
    public Animator swordAnimator;
    public AudioSource swingSound;
    public AudioSource hitSound;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            swingSound.PlayOneShot(swingSound.clip);

            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        swordAnimator.SetTrigger("Attack");

        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        Debug.Log("Enemies found: " + enemies.Length);

        if (enemies.Length == 0)
        {
            Debug.Log("No EnemyHealth scripts found in scene.");
            return;
        }

        EnemyHealth closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Vector3 attackPosition = Camera.main != null ? Camera.main.transform.position : transform.position;

        foreach (EnemyHealth enemy in enemies)
        {
            float distance = Vector3.Distance(attackPosition, enemy.transform.position);
            Debug.Log(enemy.name + " distance: " + distance);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && closestDistance <= attackRange)
        {
            closestEnemy.TakeDamage(damage);
            Debug.Log("Enemy damaged: " + closestEnemy.name);
            if (hitSound != null)
            {
                hitSound.PlayOneShot(hitSound.clip);
            }
        }
        else
        {
            Debug.Log("Closest enemy is too far away. Distance: " + closestDistance);
        }
    }
}