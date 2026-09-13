using UnityEngine;

public class Bullet : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;


    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;


    void Start() {

    }

    public void SetTarget(Transform _target) {
        target = _target;
    }

    private void FixedUpdate() {
        if (!target) {
            Destroy(gameObject);
            return;
        } 

        Vector2 direction = target.position - transform.position;

        rb.linearVelocity = direction.normalized * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform != target)  {
            Debug.Log("Bullet hit something other than the target");
            return;
        }

        other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }

}
