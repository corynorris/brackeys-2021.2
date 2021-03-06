using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerCombat : MonoBehaviour
{
    private Player player;

    public bool clampNESW = true;
    public bool drawDebugBox = true;
    public float attackRange = 1.5f;
    public float attackSize = 0.5f;
    
    public LayerMask enemyLayers;
    public float attackDamage = 3;

    [SerializeField]
    private AudioClip[] miningClip;

    void Awake()
    {
        player = GetComponent<Player>();
     
    }

    private void Start()
    {
    }

    void Update()
    {
        if (Time.timeScale == 0 && !player.IsAlive()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

    }

    void Attack()
    {
        player.TriggerAllAnimators("Attack");

        Vector3 attackPoint = CalculateAttackPoint();
        DrawDebugBox(attackPoint);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackSize, enemyLayers);

        // Do something with the resources
        foreach (Collider2D enemy in hitEnemies)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                Debug.Log("Attacking : " + enemy.name);
                health.TakeDamage(attackDamage, player.GetForwardDirection());
            }
        }
        if (hitEnemies.Length > 0)
        {
            Utils.spawnAudio(gameObject, miningClip[Random.Range(0,miningClip.Length-1)], 0.5f);
        }
    }

    private Vector3 CalculateAttackPoint()
    {

        // Detect enemies in range
        Vector3 pointInFrontOfPlayer;
        if (clampNESW)
        {
            Vector3 forward = player.GetForwardDirection();
            
            
            if (forward.sqrMagnitude > 1)
            {
                forward.x = 0.0f;
            }

            // Hack to make it a bit more align with animation
            if (forward.y >= 0 )
            {
                Vector3 offset = new Vector2(0, 0.5f);
                return player.GetCenter() + offset + forward * attackRange;
            }

            return player.GetCenter() + forward * attackRange;


        }
        else
        {
            return pointInFrontOfPlayer = player.GetCenter() + player.GetForwardDirection() * attackRange;
        }
    }

    private void DrawDebugBox(Vector3 pointInFrontOfPlayer)
    {
        if (drawDebugBox)
        {
            Vector3 point1 = new Vector3(pointInFrontOfPlayer.x - attackSize, pointInFrontOfPlayer.y + attackSize);
            Vector3 point2 = new Vector3(pointInFrontOfPlayer.x + attackSize, pointInFrontOfPlayer.y + attackSize);
            Vector3 point3 = new Vector3(pointInFrontOfPlayer.x + attackSize, pointInFrontOfPlayer.y - attackSize);
            Vector3 point4 = new Vector3(pointInFrontOfPlayer.x - attackSize, pointInFrontOfPlayer.y - attackSize);

            Debug.DrawLine(point1, point2, Color.red, 1, false);
            Debug.DrawLine(point2, point3, Color.red, 1, false);
            Debug.DrawLine(point3, point4, Color.red, 1, false);
            Debug.DrawLine(point4, point1, Color.red, 1, false);
        }
    }
}


