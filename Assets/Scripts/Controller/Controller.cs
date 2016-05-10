using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Attacks))]
public class Controller : MonoBehaviour
{
    public delegate void OnStunDelegate();
    public event OnStunDelegate OnStun;
    public event OnStunDelegate OnEndStun;

    #region variables & properties

    [SerializeField]
    private float _baseLateralSpeed = 5f;
    public float BaseLateralSpeed
    {
        get { return _baseLateralSpeed; }
    }

    private float _lateralSpeed;
    public float LateralSpeed
    {
        get { return _lateralSpeed; }
        set { _lateralSpeed = value; }
    }

    [SerializeField]
    private float _baseJumpSpeed = 3f;
    public float BaseJumpSpeed
    {
        get { return _baseJumpSpeed; }
    }

    private float _jumpSpeed;
    public float JumpSpeed
    {
        get { return _jumpSpeed; }
        set { _jumpSpeed = value; }
    }

    [SerializeField]
    private float _baseJumpVariationTime = 1f;
    public float BaseJumpVariationTime
    {
        get { return _baseJumpVariationTime; }
    }

    private float _jumpVariationTime = 1f;
    public float JumpVariationTime
    {
        get { return _jumpVariationTime; }
        set { _jumpVariationTime = value; }
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
        set 
        {
            _stunned = value;
            if (_stunned)
            {
                if (OnStun != null)
                {
                    OnStun();
                }
            }
            else
            {
                if (OnEndStun != null)
                {
                    OnEndStun();
                }
            }
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

    private bool _dashing = false;
    public bool Dashing
    {
        get { return _dashing; }
        set { _dashing = value; }
    }

    [SerializeField]
    private float _baseDashDuration = 1f;
    public float BaseDashDuration
    {
        get { return _baseDashDuration; }
    }

    private float _dashDuration;
    public float DashDuration
    {
        get { return _dashDuration; }
        set { _dashDuration = value; }
    }

    [SerializeField]
    private float _baseDashVelocity = 30f;
    public float BaseDashVelocity
    {
        get { return _baseDashVelocity; }
    }

    private float _dashVelocity;
    public float DashVelocity
    {
        get { return _dashVelocity; }
        set { _dashVelocity = value; }
    }

    public int playerNumber;

    private Transform _transform;

    private Rigidbody2D _rgbd2d;
    public Rigidbody2D Rgbd2d
    {
        get { return _rgbd2d; }
    }

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

    private Attacks _attacks;

    [SerializeField]
    private Transform[] ItemCorner; // in this order : topleft, topright, bottomleft, bottomright, center

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

    private bool _movementBlocked;
    public bool MovementBlocked
    {
        get { return (_movementBlocked); }
        set { _movementBlocked = value; }
    }

    private bool _movementSlowed;
    public bool MovementSlowed
    {
        get { return (_movementSlowed); }
        set { _movementSlowed = value; }
    }

    private bool _jumpBlocked;
    public bool JumpBlocked
    {
        get { return (_jumpBlocked); }
        set { _jumpBlocked = value; }
    }

    #endregion

    #region Unity CallBacks

    void Awake()
    {
        _rgbd2d = GetComponent<Rigidbody2D>();
        _attacks = GetComponent<Attacks>();

        GameObject go = new GameObject();
        go.name = "Controller";
        go.transform.parent = this.transform;

        _jsm = go.AddComponent<JoyStickManager>();
        _jsm.Reset(playerNumber);

        _transform = this.GetComponent<Transform>();
        _anim = this.GetComponent<Animator>();

        _movementBlocked = false;
        _movementSlowed = false;
        _jumpBlocked = false;
    }

    void Start()
    {
        ResetController();
    }

    private float xVel, yVel = 0;

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();

        jumpHelper();
        _anim.SetBool("Grounded", _grounded);
        _anim.SetFloat("xVel", xVel);

        _rgbd2d.velocity = new Vector2(xVel, _rgbd2d.velocity.y);
    }

    void HandleMovement()
    {
        if (!_stunned && !_movementBlocked)
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
        if (_movementBlocked && !_dashing)
        {
            xVel = 0;
        }
        if (_movementSlowed)
        {
            xVel /= 5;
        }
    }

    void HandleJump()
    {
        if (!_stunned && _jumpcharges > 0 && !_jumpBlocked && _jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.A))
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
            StartCoroutine("Co_HandleJumpVariation");
            // pushing back the player under me ?
        }
    }

    void HandleDash()
    {
        if (!_stunned && !_movementBlocked)
        {
            int direction = 0;

            if (_jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.LB))
            {
                direction = -1;
            }
            else if (_jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.RB))
            {
                direction = 1;
            }
            if (direction != 0)
            {
                _anim.Play(IsLookingRight ? (direction == 1 ? "Dash_Front" : "Dash_Back") : ((direction == -1 ? "Dash_Front" : "Dash_Back")));

                _movementBlocked = true;
                _attacks.AttackBlocked = true;
                _jumpBlocked = true;
                _dashing = true;
                xVel = _dashVelocity * direction;
            }
        }
    }

    public void StopDash()
    {
        xVel = 0;
        _attacks.AttackBlocked = false;
    }

    void DashEnd()
    {
        _movementBlocked = false;
        _jumpBlocked = false;
        _dashing = false;
    }

    #endregion

    #region Coroutines

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

    private IEnumerator Co_HandleJumpVariation()
    {
        float timeSpent = 0;

        while (_jsm.GetButton(JoyStickManager.e_XBoxControllerButtons.A) && timeSpent < _jumpVariationTime)
        {
            _rgbd2d.velocity = new Vector2(_rgbd2d.velocity.x, _jumpSpeed);
            yield return new WaitForEndOfFrame();
            timeSpent += Time.deltaTime;
        }
    }

    #endregion

    #region helpers

    private void jumpHelper()
    {
        RaycastHit2D hit, hit2;

        hit = Physics2D.Linecast(ItemCorner[2].position, ItemCorner[3].position, (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("PlatformHard")));
        hit2 = Physics2D.Linecast(ItemCorner[2].position + Vector3.up * 0.05f, ItemCorner[3].position + Vector3.up * 0.05f, (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("PlatformHard")));

        Grounded = hit.collider != null && hit2.collider == null;
    }

    #endregion

    #region public action

    public void Stun(float duration)
    {
        if (_dashing)
            return;
        StopAllCoroutines();
        StartCoroutine("Co_Stun", duration);
        _anim.Play("Damage_Taken");
    }

    /// <summary>
    /// direction also take into account the intensity of the push. (ie, the vector is not normalized)
    /// </summary>
    /// <param name="direction"></param>
    public void Push(Vector2 direction)
    {
        if (_dashing)
            return;
        xVel = direction.x;
        yVel = direction.y;
    }

    /// <summary>
    /// when respawning, for example
    /// </summary>
    public void ResetController()
    {
        DashVelocity = _baseDashVelocity;
        JumpSpeed = _baseJumpSpeed;
        LateralSpeed = _baseLateralSpeed;
        JumpVariationTime = _baseJumpVariationTime;

        MaxJumpCharges = 2;

        _dashing = false;
        _stunned = false;

        StopAllCoroutines();
        GetComponent<Attacks>().ResetAttacks();
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
