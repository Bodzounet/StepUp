using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller))]
public class Attacks : MonoBehaviour
{
    public enum e_AttackType
    {
        LIGHT,
        STRONG
    }

    #region variables and props

    /// <summary>
    /// the required time for a strong attack to be fully charged
    /// </summary>
    [SerializeField]
    private float _baseStrongAttackChargeDelay = 2f;
    public float BaseStrongAttackChargeDelay
    {
        get { return _baseStrongAttackChargeDelay; }
    }

    private float _strongAttackChargeDelay;
    public float StrongAttackChargeDelay
    {
        get { return _strongAttackChargeDelay; }
        set { _strongAttackChargeDelay = value; }
    }

    /// <summary>
    /// the base radius of the circle AOE
    /// </summary>
    [SerializeField]
    private float _baseStrongAttackRadius = 2;
    public float BaseStrongAttackRadius
    {
        get { return _baseStrongAttackRadius; }
    }

    private float _strongAttackRadius;
    public float StrongAttackRadius
    {
        get { return _strongAttackRadius; }
        set { _strongAttackRadius = value; }
    }

    /// <summary>
    /// the radius increase with the charge.
    /// at 100% charge, the radius increases with a percentage equals to this variable
    /// </summary>
    [SerializeField]
    private float _baseStrongAttackFullChargeRadiusIncrease = 1.0f;
    public float BaseStrongAttackFullChargeRadiusIncrease
    {
        get { return _baseStrongAttackFullChargeRadiusIncrease; }
    }

    private float _strongAttackFullChargeRadiusIncrease = 1.0f; // percentage, default 100% increase
    public float StrongAttackFullChargeRadiusIncrease
    {
        get { return _strongAttackFullChargeRadiusIncrease; }
        set { _strongAttackFullChargeRadiusIncrease = value; }
    }

    /// <summary>
    /// how long this attack stun lasts
    /// stun time is proportionnal to the inverse of the distance between the center of the aoe and the target
    /// </summary>
    [SerializeField]
    private float _baseMaxStunDuration = 3f;
    public float BaseMaxStunDuration
    {
        get { return _baseMaxStunDuration; }
    }

    private float _maxStunDuration;
    public float MaxStunDuration
    {
        get { return _maxStunDuration; }
        set { _maxStunDuration = value; }
    }

    /// <summary>
    /// the distance from the center of the AOE where there is always 100% stun. after, the diminishing returns start.
    /// </summary>
    [SerializeField]
    private float _baseSubRadius100percentStun = 1f;
    public float BaseSubRadius100percentStun
    {
        get { return _baseSubRadius100percentStun; }
    }

    private float _subRadius100percentStun;
    public float SubRadius100percentStun
    {
        get { return _subRadius100percentStun; }
        set { _subRadius100percentStun = value; }
    }

    /// <summary>
    /// light attack stun duration
    /// </summary>
    [SerializeField]
    private float _baseLightAttackStunDuration = 0.5f;
    public float BaseLightAttackStunDuration
    {
        get { return _baseLightAttackStunDuration; }
    }

    private float _lightAttackStunDuration;
    public float LightAttackStunDuration
    {
        get { return _lightAttackStunDuration; }
        set { _lightAttackStunDuration = value; }
    }

    private bool _chargingStrongHit = false;
    private float _startChargingTime;

    private bool _attacksBlocked;
    public bool AttackBlocked
    {
        get { return (_attacksBlocked); }
        set { _attacksBlocked = value; }
    }

    [SerializeField]
    private float _ligthAttackPercentageStun = 0.05f;
    public float LigthAttackPercentageStun
    {
        get { return _ligthAttackPercentageStun; }
        set { _ligthAttackPercentageStun = value; }
    }

    private float _strongAttackPercentageStun = 0.1f; // Max value, reached if the attack if fully charged. less elsewhere.
    public float StrongAttackPercentageStun
    {
        get { return _strongAttackPercentageStun; }
        set { _strongAttackPercentageStun = value; }
    }

    private Controller _controller;
    private float _chargePercentage;

    #endregion

    public GameObject StrongEffectFx;

    public void ResetAttacks()
    {
        _chargingStrongHit = false;

        LightAttackStunDuration = _baseLightAttackStunDuration;
        MaxStunDuration = _baseMaxStunDuration;
        StrongAttackChargeDelay = _baseStrongAttackChargeDelay;
        StrongAttackFullChargeRadiusIncrease = _baseStrongAttackFullChargeRadiusIncrease;
        StrongAttackRadius = _baseStrongAttackRadius;
        SubRadius100percentStun = _baseSubRadius100percentStun;

        StopAllCoroutines();
    }

    void Awake()
    {
        _controller = GetComponent<Controller>();
        _controller.OnStun += OnStun;
        _controller.OnEndStun += OnEndStun;
    }

    void Start()
    {
        ResetAttacks();
    }

    void Update()
    {
        // start charging strong attack
        if (!_attacksBlocked && _controller.Grounded && !_controller.Stunned && _controller.Jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.LT))
        {
            _attacksBlocked = true;
            _controller.MovementBlocked = true;
            _controller.JumpBlocked = true;
            _startChargingTime = Time.time;
            StartCoroutine("Co_StrongHit");
        }

        // unleash strong attack
        if ((_chargingStrongHit && _controller.Jsm.GetButtonUp(JoyStickManager.e_XBoxControllerButtons.LT)) /*|| _controller.IsMoving*/)
        {
            StopCoroutine("Co_StrongHit");
            _chargePercentage = (Time.time - _startChargingTime) / _strongAttackChargeDelay;
            StrongHit();
        }

        // use light attack
        if (!_attacksBlocked && !_controller.Stunned && _controller.Jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.RT))
        {
            _attacksBlocked = true;
            _controller.JumpBlocked = true;
            if (_controller.Grounded)
            {
                _controller.MovementBlocked = true;
            }
            float angle = _controller.Jsm.GetAxisAngle();

            if (angle > 45 && angle < 135)
            {
                _controller.Anim.Play("Up_Attack");
            }
            else if (angle < -45 && angle > -135)
            {
                _controller.Anim.Play("Down_Attack");
            }
            else
            {
                _controller.Anim.Play("Front_Attack");
            }
        }
    }

    /// <summary>
    /// stun the target.
    /// called by the damage hitbox
    /// </summary>
    public void LightHit(GameObject target)
    {
        if (target.layer == LayerMask.NameToLayer("Player"))
        {
            Controller targetController = target.GetComponent<Controller>();

            targetController.Stun(LightAttackStunDuration, LigthAttackPercentageStun);
            targetController.Push((target.transform.position - transform.position).normalized * 4);
        }
    }


    /// <summary>
    /// Callback when the light hit is over. Allow the player to move.
    /// </summary>
    public void LightHitIsOver()
    {
        _attacksBlocked = false;
        _controller.JumpBlocked = false;
        _controller.MovementBlocked = false;
    }

    /// <summary>
    /// Callback when the strong hit is over. Allow the player to move.
    /// </summary>
    public void StrongHitIsOver()
    {
        print("coucou");
        _attacksBlocked = false;
        _controller.JumpBlocked = false;
        _controller.MovementBlocked = false;
    }

    /// <summary>
    /// stun en AOE circulaire.
    /// plus on proche de l'épicentre, plus le stun est long
    /// </summary>
    /// <param name="chargePercentage"></param> la puissance du stun, 100% -> radius += _strongAttackFullChargeRadiusIncrease
    void StrongHit()
    {
        _controller.Anim.Play("Strong_Attack");
        _chargingStrongHit = false;
    }

    void ActivateStrongHIt()
    {
        float radius = _strongAttackRadius * (1 + _chargePercentage * _strongAttackFullChargeRadiusIncrease);

        var fx = GameObject.Instantiate(StrongEffectFx, transform.position, Quaternion.identity) as GameObject;
        fx.transform.localScale = new Vector3(1.5f * radius, 1.5f * radius, 1);



        foreach (var v in Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Player")))
        {
            if (v.isTrigger || v.gameObject == this.gameObject || v.gameObject.layer != LayerMask.NameToLayer("Player"))
                continue;

            float percentStun;

            if (SubRadius100percentStun >= radius)
            {
                percentStun = 1;
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, v.transform.position);

                percentStun = Mathf.Clamp(1 - (hit.distance - SubRadius100percentStun) / (radius - SubRadius100percentStun), 0, 1);
            }

            Controller targetController = v.GetComponentInParent<Controller>();
            targetController.Stun(_maxStunDuration * percentStun, StrongAttackPercentageStun * percentStun);
            targetController.Push((v.transform.position - transform.position).normalized * 10);
        }
    }

    public IEnumerator Co_StrongHit()
    {
        _chargingStrongHit = true;
        _controller.Anim.Play("Charge");
        yield return new WaitForSeconds(StrongAttackChargeDelay);
        _chargePercentage = 1;
        StrongHit();
    }

    public void OnStun()
    {
        _chargingStrongHit = false;
        StopCoroutine("Co_StrongHit");
    }

    public void OnEndStun()
    {
        _chargingStrongHit = false;
    }
}
