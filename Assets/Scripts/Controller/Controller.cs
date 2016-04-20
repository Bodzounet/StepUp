using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Attacks))]
public class Controller : MonoBehaviour
{
    public delegate void OnStunDelegate();
    public event OnStunDelegate OnStun;

    #region variables & properties

    [SerializeField]
    private float _lateralSpeed = 5f;
    public float LateralSpeed
    {
        get { return _lateralSpeed; }
        set { _lateralSpeed = value; }
    }

    [SerializeField]
    private float _jumpSpeed = 3f;
    public float JumpSpeed
    {
        get { return _jumpSpeed; }
        set { _jumpSpeed = value; }
    }

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

    private bool _stunned;
    public bool Stunned
    {
        get { return _stunned; }
        set { _stunned = value; }
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

    private bool _dashing = false;
    public bool Dashing
    {
        get { return _dashing; }
        set { _dashing = value; }
    }

    [SerializeField]
    private float _dashDuration = 1f;
    public float DashDuration
    {
        get { return _dashDuration; }
        set { _dashDuration = value; }
    }

    [SerializeField]
    private float _dashVelocity = 30f;
    public float DashVelocity
    {
        get { return _dashVelocity; }
        set { _dashVelocity = value; }
    }

    public int playerNumber;

    private Rigidbody2D _rgbd2d;
    private Transform _transform;

    private Animator _anim;
    public Animator Anim
    {
        get { return _anim; }
    }

    private JoyStickManager _jsm;

    public JoyStickManager Jsm
    {
        get { return _jsm; }
    }

    [SerializeField]
    private Transform[] ItemCorner; // in this order : topleft, topright, bottomleft, bottomright, center
    private Collider2D[] _colliders;

    public bool IsLookingRight
    {
        get
        {
            return Mathf.Sign(ItemCorner[4].position.x - ItemCorner[0].position.x) > 0;
        }
    }

    public bool IsMoving
    {
        get
        {
            return _rgbd2d.velocity != Vector2.zero;
        }
    }

    #endregion

    #region Unity CallBacks

    void Awake()
    {
        _rgbd2d = GetComponent<Rigidbody2D>();

        GameObject go = new GameObject();
        go.name = "Controller";
        go.transform.parent = this.transform;

        _jsm = go.AddComponent<JoyStickManager>();
        _jsm.Reset(playerNumber);

        _transform = this.GetComponent<Transform>();
        _anim = this.GetComponent<Animator>();

        _colliders = this.GetComponents<Collider2D>().Where(x => x.isTrigger == false).ToArray();
    }

    void Start()
    {
        _maxJumpCharges = _jumpcharges;
    }

    private float xVel, yVel = 0;

    void Update()
    {
        if (!_stunned && !_dashing)
        {
            xVel = _jsm.GetAxisClamped(JoyStickManager.e_XBoxControllerAxis.Horizontal) * _lateralSpeed;
            if (xVel < 0)
            {
                _transform.localScale = new Vector3(-3, 3, 3);
            }
            else if (xVel > 0)
            {
                _transform.localScale = new Vector3(3, 3, 3);
            }
        }

        if (!_stunned && _jumpcharges > 0 && _jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.A))
        {
            if (!Grounded)
            {
                _jumpcharges--;
                _anim.Play("Air Jump");
            }
            else
            {
                _anim.Play("Jump");
            }
            yVel = _jumpSpeed;
        }
        else
        {
            yVel = _rgbd2d.velocity.y;
        }

        if (!_stunned && !_dashing)
        {
            if (_jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.LB))
            {
                StartCoroutine("Co_Dash", -1);
            }
            else if (_jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.RB))
            {
                StartCoroutine("Co_Dash", 1);
            }
        }

        Debug.Log("Grounded : " + Grounded);

        jumpHelper();
        _anim.SetBool("Grounded", _grounded);

        _rgbd2d.velocity = new Vector2(xVel, yVel);
    }

    #endregion

    #region Coroutines

    private IEnumerator Co_Dash(int direction)
    {
        _anim.Play(IsLookingRight ? (direction == 1 ? "Dash_Front" : "Dash_Back") : ((direction == -1 ? "Dash_Front" : "Dash_Back")));

        _dashing = true;
        xVel = _dashVelocity * direction;
        yield return new WaitForSeconds(_dashDuration);
        xVel = 0;
        _dashing = false;
    }

    private IEnumerator Co_Stun(float duration)
    {
        //_anim.Play("Stun");
        _stunned = true;
        xVel = 0;
        if (yVel > 0)
        {
            yVel = 0;
        }
        yield return new WaitForSeconds(duration);
        _stunned = false;
    }

    #endregion

    #region helpers

    private void jumpHelper()
    {
        RaycastHit2D hit, hit2;

        hit = Physics2D.Linecast(ItemCorner[2].position, ItemCorner[3].position, 1 << LayerMask.NameToLayer("Platform"));
        hit2 = Physics2D.Linecast(ItemCorner[2].position + Vector3.up * 0.05f, ItemCorner[3].position + Vector3.up * 0.05f, 1 << LayerMask.NameToLayer("Platform"));
        Debug.Log("hit : " + hit.collider);
        Debug.Log("hit2 : " + hit2.collider);

        Grounded = hit.collider != null && hit2.collider == null;
    }

    #endregion

    #region public action

    public void Stun(float duration)
    {
        if (OnStun != null)
            OnStun();
        StartCoroutine("Co_Stun", duration);
    }

    /// <summary>
    /// direction also take into account the intensity of the push. (ie, the vector is not normalized)
    /// </summary>
    /// <param name="direction"></param>
    public void Push(Vector2 direction)
    {
        xVel = direction.x;
        yVel = direction.y;
    }

    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(ItemCorner[2].position + Vector3.up * 0.05f, ItemCorner[3].position + Vector3.up * 0.05f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(ItemCorner[2].position, ItemCorner[3].position);
    }
}
