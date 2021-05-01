using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Bird : MonoBehaviour
{
    public enum BirdState
    {
        Idle, Thrown, HitSomething
    }
    public GameObject Parent;
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public float impactField;
    public float force;
    public LayerMask ImToHit;
    public GameObject explosionPrefab;

    public BirdState State
    {
        get
        {
            return _state;
        }
    }

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    private void Start()
    {
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    private void FixedUpdate()
    {
        if (_state == BirdState.Idle && RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) && RigidBody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
        {
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }

   
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo (Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot (Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

    private void OnDestroy()
    {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
        OnBirdDestroyed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
        Explosion();
    }

    public virtual void OnTap()
    {
        //Do nothing
    }

    void Explosion()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, impactField, ImToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 dir = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(dir * force);
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, impactField);
    //}
}
