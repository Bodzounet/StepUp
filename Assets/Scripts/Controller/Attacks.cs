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

    /// <summary>
    /// the recover time in addition of the strong attack animation.
    /// </summary>
    [SerializeField]
    private float _baseStrongAttackRecoverTime = 0.25f;
    public float BaseStrongAttackRecoverTime
    {
        get { return _baseStrongAttackRecoverTime; }
    }

    private float _strongAttackRecoverTime;
    public float StrongAttackRecoverTime
    {
        get { return _strongAttackRecoverTime; }
        set { _strongAttackRecoverTime = value; }
    }
    
    [SerializeField]
    private float _baseStrongAttackCooldown = 7.5f;
    public float BaseStrongAttackCooldown
    {
        get { return _baseStrongAttackCooldown; }
    }

    private float _strongAttackCooldown;
    public float StrongAttackCooldown
    {
        get { return _strongAttackCooldown; }
        set { _strongAttackCooldown = value; }
    }

    [SerializeField]
    private float _baseLightAttackCooldwon = 3.5f;
    public float BaseLightAttackCooldwon
    {
        get { return _baseLightAttackCooldwon; }
    }

    private float _lightAttackCooldown;
    public float LightAttackCooldown
    {
        get { return _lightAttackCooldown; }
        set { _lightAttackCooldown = value; }
    }

    private bool _chargingStrongHit = false;
    private bool _usingLightHit = false;
    private float _startChargingTime;

    private bool _cdLightHit = false;
    private bool _cdStrongHit = false;

    private Controller _controller;

    [SerializeField]
    private GameObject[] _lightHits; // up, front, down;

    #endregion

    public void ResetAttacks()
    {
        _chargingStrongHit = false;
        _usingLightHit = false;
        _cdLightHit = false;
        _cdLightHit = false;

        LightAttackCooldown = _baseLightAttackCooldwon;
        LightAttackStunDuration = _baseLightAttackStunDuration;
        MaxStunDuration = _baseMaxStunDuration;
        StrongAttackRecoverTime = _baseStrongAttackRecoverTime;
        StrongAttackChargeDelay = _baseStrongAttackChargeDelay;
        StrongAttackCooldown = _baseStrongAttackCooldown;
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
        if (!_chargingStrongHit && !_cdStrongHit && _controller.Grounded && !_controller.Stunned && _controller.Jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.LT))
        {
            _startChargingTime = Time.time;
            StartCoroutine("Co_StrongHit");
        }

        // unleash string attack
        if ((_chargingStrongHit && _controller.Jsm.GetButtonUp(JoyStickManager.e_XBoxControllerButtons.LT)) /*|| _controller.IsMoving*/)
        {
            StopCoroutine("Co_StrongHit");
            StrongHit((Time.time - _startChargingTime) / _strongAttackChargeDelay);
        }

        // use light attack
        if (!_usingLightHit && !_cdLightHit && !_chargingStrongHit && !_controller.Stunned && _controller.Jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.RT))
        {
            _usingLightHit = true;
            float angle = _controller.Jsm.GetAxisAngle();

            if (angle > 45 && angle < 135)
            {
                _lightHits[0].SetActive(true);
                _controller.Anim.Play("Up_Attack");
            }
            else if (angle < -45 && angle > -135)
            {
                _lightHits[2].SetActive(true);
                _controller.Anim.Play("Down_Attack");
            }
            else
            {
                _lightHits[1].SetActive(true);
                _controller.Anim.Play("Front_Attack");
            }
            StartCD(e_AttackType.LIGHT);
        }
    }

    /// <summary>
    /// stun la cible.
    /// appelé par la point lorsqu'il est actif
    /// </summary>
    public void LightHit(GameObject target)
    {
        if (target.layer == LayerMask.NameToLayer("Player"))
        {
            Controller targetController = target.GetComponent<Controller>();

            targetController.Stun(LightAttackStunDuration);
            targetController.Push((target.transform.position - transform.position).normalized * 4);
        }
    }

    public void LightHitIsOver(int id)
    {
        // recover time ?
        // anim.Play("landing");
        _usingLightHit = false;
        _lightHits[id].SetActive(false);
    }

    /// <summary>
    /// stun en AOE circulaire.
    /// plus on proche de l'épicentre, plus le stun est long
    /// </summary>
    /// <param name="chargePercentage"></param> la puissance du stun, 100% -> radius += _strongAttackFullChargeRadiusIncrease
    void StrongHit(float chargePercentage)
    {
        _controller.Anim.Play("Strong_Attack");
        StartCD(e_AttackType.STRONG);
        _chargingStrongHit = false;

        float radius = _strongAttackRadius * (1 + chargePercentage * _strongAttackFullChargeRadiusIncrease);

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
            targetController.Stun(_maxStunDuration * percentStun);
            targetController.Push((v.transform.position - transform.position).normalized * 10);
        }

        // recover time, pour "punir" en cas d'échec

        _controller.Stun(_strongAttackRecoverTime);
    }

    void StartCD(e_AttackType at)
    {
        switch (at)
        {
            case e_AttackType.LIGHT:
                StopCoroutine("Co_CDLightHit");
                StartCoroutine("Co_CDLightHit");
                break;
            case e_AttackType.STRONG:
                StopCoroutine("Co_CDStrongHit");
                StartCoroutine("Co_CDStrongHit");
                break;
        }
    }

    void FinishCD(e_AttackType at)
    {
        switch (at)
        {
            case e_AttackType.LIGHT:
                StopCoroutine("Co_CDLightHit");
                _cdLightHit = false;
                break;
            case e_AttackType.STRONG:
                StopCoroutine("Co_CDStrongHit");
                _cdStrongHit = false;
                break;
        }
    }

    public IEnumerator Co_StrongHit()
    {
        _chargingStrongHit = true;
        _controller.Anim.Play("Charge");
        yield return new WaitForSeconds(StrongAttackChargeDelay);
        StrongHit(1);
    }

    public IEnumerator Co_CDLightHit()
    {
        _cdLightHit = true;
        yield return new WaitForSeconds(LightAttackCooldown);
        _cdLightHit = false;
    }

    public IEnumerator Co_CDStrongHit()
    {
        _cdStrongHit = true;
        yield return new WaitForSeconds(StrongAttackCooldown);
        _cdStrongHit = false;
    }

    public void OnStun()
    {
        _chargingStrongHit = false;
        StopCoroutine("Co_StrongHit");
        if (_chargingStrongHit)
        {
            StartCD(e_AttackType.STRONG);
        }
    }

    public void OnEndStun()
    {
        _usingLightHit = false;
        _chargingStrongHit = false;
    }
}
