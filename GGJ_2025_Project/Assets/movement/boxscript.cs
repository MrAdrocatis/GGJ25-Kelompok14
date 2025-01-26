using UnityEngine;

public class boxscript : MonoBehaviour
{
    [Header("movement")]
    public float movementspeed = 12f;
    private float horizontalMovement;
    private Rigidbody2D rB;

    //[Header("jumping")]
    //public float jumpforce = 20f;
    //private bool justJumped = false;

    [Header("ground")]
    public bool onground = false;

    private void Start() => rB = GetComponent<Rigidbody2D>();

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate()
    {
        rB.velocity = new Vector2(x: horizontalMovement * movementspeed, rB.velocity.y);
    }
}

    