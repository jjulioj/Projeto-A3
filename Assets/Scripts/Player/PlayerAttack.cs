using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        if (anim == null) Debug.LogWarning("Animator not found on " + gameObject.name);
        if (playerMovement == null) Debug.LogWarning("PlayerMovement not found on " + gameObject.name);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement != null && playerMovement.canAttack()
            && Time.timeScale > 0)
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(fireballSound);
        }
        else
        {
            Debug.LogWarning("SoundManager instance is null.");
        }

        anim.SetTrigger("attack");
        cooldownTimer = 0;

        int fireballIndex = FindFireball();
        if (fireballIndex != -1)
        {
            fireballs[fireballIndex].transform.position = firePoint.position;
            fireballs[fireballIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
        else
        {
            Debug.LogWarning("No available fireball found.");
        }
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return -1; 
    }
}
