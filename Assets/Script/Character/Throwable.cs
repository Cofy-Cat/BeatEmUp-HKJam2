using UnityEngine;

public class Throwable: MonoBehaviour
{
    [SerializeField] private float gravity = 10f;
    
    [SerializeField] private bool isAttached;
    [SerializeField] private bool isOnAir;

    private Transform _attachGroundPoint;
    private float _groundPosition;

    private Vector3 currentVelocity;

    private void Update()
    {
        if (isAttached)
        {
            transform.position = _attachGroundPoint.position;
        }
        else if(isOnAir)
        {
            if (transform.position.y <= _groundPosition)
            {
                transform.position = new Vector2(transform.position.x, _groundPosition);
                isOnAir = false;
            }

            transform.position += currentVelocity * Time.deltaTime;
            currentVelocity = new Vector2(currentVelocity.x, currentVelocity.y - gravity * Time.deltaTime);
        }
    }

    public void AttachToTransform(Transform attachPoint)
    {
        _attachGroundPoint = attachPoint;
        isAttached = true;
        isOnAir = false;
    }

    public void Throw(Vector2 force, float groundPosition)
    {
        isAttached = false;
        isOnAir = true;
        _groundPosition = groundPosition;
        currentVelocity = force;
    }
}