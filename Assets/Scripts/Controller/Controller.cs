﻿using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Attacks))]
public class Controller : MonoBehaviour
{
    public delegate void OnStunDelegate();
    public delegate void OnInvulnerableDelegate();
    public delegate void StunPercentageChange(float newPercentage);

    public event OnStunDelegate OnStun;
    public event OnStunDelegate OnEndStun;
    public event OnInvulnerableDelegate OnStartBeingInvulnerable;
    public event OnInvulnerableDelegate OnEndBeingInvulnerable;
    public event StunPercentageChange OnStunPercentageChange;

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
        set 
        { 
            _maxJumpCharges = value;
            _jumpcharges = value;
        }
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

        private bool _invulnerable = false;
    public bool Invulnerable
    {
        get { return _invulnerable; }
        private set 
        {
            _invulnerable = value; 
            if (value && OnStartBeingInvulnerable != null)
            {
                OnStartBeingInvulnerable();
            }
            else if (OnEndBeingInvulnerable != null)
            {
                OnEndBeingInvulnerable();
            }
        }
    }

    public float InvulnerabilityDuration
    {
        set 
        {
            Invulnerable = true;
            CancelInvoke("EndInvulnerability");
            Invoke("EndInvulnerability", value);
        }
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

    private Items.Inventory _inventory;

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

    private float stunningPercentage = 0;
    public float StunningPercentage
    {
        get { return stunningPercentage; }
        set 
        {
            stunningPercentage = value;
            if (OnStunPercentageChange != null)
                OnStunPercentageChange(value);
        }
    }

    private bool _onIce;
    public bool OnIce
    {
        get { return _onIce; }
        set { _onIce = value; }
    }

    private bool _wasOnMud = false;
    private bool _OnMud;
    public bool OnMud
    {
        get { return _OnMud; }
        set 
        {
            if (value)
            {
                LateralSpeed = BaseLateralSpeed / 3;
                JumpSpeed = BaseJumpSpeed / 3;
            }
            else
            {
                LateralSpeed = BaseLateralSpeed;
                _wasOnMud = true;
            }
            _OnMud = value; 
        }
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
        _jsm.Reset(playerNumber - 1);

        _transform = this.GetComponent<Transform>();
        _anim = this.GetComponent<Animator>();

        _inventory = this.GetComponent<Items.Inventory>();

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
        if (Input.GetKeyDown(KeyCode.Escape))
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");

        if (_jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.X))
            _inventory.UseItem();

        if (!_stunned && !_dashing)
            HandleMovement();
        HandleJump();
        HandleDash();

        HandleIce();

        jumpHelper();
        _anim.SetBool("Grounded", _grounded);
        _anim.SetFloat("xVel", xVel);

        _rgbd2d.velocity = new Vector2(xVel, _rgbd2d.velocity.y);
    }

    void HandleIce()
    {
        if (OnIce)
        {
            if (IsLookingRight)
                xVel = _lateralSpeed;
            else
                xVel = -LateralSpeed;
        }
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
        int direction = 0;

        if (!_stunned && !_movementBlocked)
        {
            if (_jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.B))
            {
                if (xVel > 0)
                {
                    direction = 1;
                }
                else if (xVel < 0)
                {
                    direction = -1;
                }
            }

            if (direction != 0)
            {
                _anim.Play("Dash_Front");
                _movementBlocked = true;
                _attacks.AttackBlocked = true;
                _jumpBlocked = true;
                _dashing = true;
                xVel = _dashVelocity * direction;
            }
        }
    }

    #endregion

    #region Coroutines & Invoke

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
        Stunned = true;
        xVel = 0;
        if (yVel > 0)
        {
            yVel = 0;
        }
        yield return new WaitForSeconds(duration);
        Stunned = false;
        _jumpBlocked = false;
        _movementBlocked = false;
        _movementSlowed = false;
        _attacks.AttackBlocked = false;
    }

    private void EndInvulnerability()
    {
        Invulnerable = false;
    }

    private IEnumerator Co_HandleJumpVariation()
    {
        float timeSpent = 0;

        float actualJumpSpeed = _wasOnMud ? BaseJumpSpeed / 3 : JumpSpeed;

        while (_jsm.GetButton(JoyStickManager.e_XBoxControllerButtons.A) && timeSpent < _jumpVariationTime)
        {
            _rgbd2d.velocity = new Vector2(_rgbd2d.velocity.x, actualJumpSpeed);
            yield return new WaitForEndOfFrame();
            timeSpent += Time.deltaTime;
        }
        if (_wasOnMud)
        {
            _wasOnMud = false;
            JumpSpeed = BaseJumpSpeed;
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

    public void Stun(float duration, float stunningPercentageIncrease = 0.05f)
    {
        if (_dashing || _invulnerable)
            return;

        StunningPercentage += stunningPercentageIncrease;

        StopAllCoroutines();
        StartCoroutine("Co_Stun", duration * (1 + 2 * StunningPercentage));
        _anim.Play("Damage_Taken");
    }

    /// <summary>
    /// direction also take into account the intensity of the push. (ie, the vector is not normalized)
    /// </summary>
    /// <param name="direction"></param>
    public void Push(Vector2 direction)
    {
        if (_dashing || _invulnerable)
            return;
        xVel = direction.x * (1 + StunningPercentage);
        yVel = direction.y * (1 + StunningPercentage);
    }

    /// <summary>
    /// when respawning, for example
    /// </summary>
    public void ResetController()
    {
        DashVelocity = _baseDashVelocity;
        JumpSpeed = _baseJumpSpeed;
        LateralSpeed = _baseLateralSpeed;
        Invulnerable = false;
        JumpVariationTime = _baseJumpVariationTime;

        StunningPercentage = 0;

        MaxJumpCharges = 2;

        _dashing = false;
        Stunned = false;

        StopAllCoroutines();
        GetComponent<Attacks>().ResetAttacks();
    }

    #endregion

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(ItemCorner[2].position + Vector3.up * 0.05f, ItemCorner[3].position + Vector3.up * 0.05f);
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawLine(ItemCorner[2].position, ItemCorner[3].position);
    //}

    public void playSounds(string musicName)
    {
        SoundManager.PlaySound(musicName);
    }
}
