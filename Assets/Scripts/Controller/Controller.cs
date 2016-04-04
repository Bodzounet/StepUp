using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour 
{
    public float lateralSpeed = 5f;
    public float jumpSpeed = 3f;

    public int playerNumber;

    private Rigidbody2D _rgbd2d;
    private JoyStickManager _jsm;

    [SerializeField]
    private Transform[] ItemCorner; // in this order : topleft, topright, bottomleft, bottomright
    private Collider2D[] _colliders;

    private bool _grounded;
    public bool Grounded
    {
        get { return _grounded; }
        private set 
        {
            if (_grounded != value)
            {
                if (value == true) // the player have just touched the ground
                {
                    _jumpcharges = MaxJumpCharges;
                }
                else // the player has left the ground, either by jumping or falling -> it is counted as a jump
                {
                    _jumpcharges--;
                }
            }
            _grounded = value;
        }
    }
    
    // the number of jump the player can do (2 by default : basic jump and air jump)
    private int _jumpcharges = 2;

    /// <summary>
    /// if we want to add more air jump.
    /// </summary>
    private int _maxJumpCharges;
    public int MaxJumpCharges
    {
        get { return _maxJumpCharges; }
        set { _maxJumpCharges = value; }
    }

    void Awake()
    {
        _rgbd2d = GetComponent<Rigidbody2D>();
        _jsm = new JoyStickManager(playerNumber);

        _colliders = this.GetComponents<Collider2D>().Where(x => x.isTrigger == false).ToArray();
    }

    void Start()
    {
        _maxJumpCharges = _jumpcharges;
    }

    void FixedUpdate()
    {
        float xVel, yVel;

        xVel = _jsm.GetAxisClamped(JoyStickManager.e_XBoxControllerAxis.Horizontal) * lateralSpeed;

        if (_jumpcharges > 0 && _jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.A))
        {
            if (!Grounded)
                _jumpcharges--;
            yVel = jumpSpeed;
        }
        else
        {
            yVel = _rgbd2d.velocity.y;
        }
        jumpHelper();

        _rgbd2d.velocity = new Vector2(xVel, yVel);

        //Debug.Log("A activated : " + _jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.B));
        //Debug.Log("A activated : " + Input.GetButtonDown("B1"));
    }

    private void jumpHelper()
    {
        RaycastHit2D hit, hit2;

        hit = Physics2D.Linecast(ItemCorner[2].position, ItemCorner[3].position, 1 << LayerMask.NameToLayer("Platform"));
        hit2 = Physics2D.Linecast(ItemCorner[2].position + Vector3.up * 0.02f, ItemCorner[3].position + Vector3.up * 0.02f, 1 << LayerMask.NameToLayer("Platform"));
        //Debug.Log("hit : " + hit.collider);

        Grounded = hit.collider != null && hit2.collider == null;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (Collider2D collider in _colliders)
        {
            Physics2D.IgnoreCollision(col, collider, true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        foreach (Collider2D collider in _colliders)
        {
            Physics2D.IgnoreCollision(col, collider, false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(ItemCorner[2].position + Vector3.up * 0.02f, ItemCorner[3].position + Vector3.up * 0.02f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(ItemCorner[2].position, ItemCorner[3].position);
    }
}
