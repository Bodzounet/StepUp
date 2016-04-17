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

    /// <summary>
    /// the required time for a strong attack to be fully charged
    /// </summary>
    [SerializeField]
    private float _strongAttackChargeDelay;
    public float ChargeDelay
    {
        get { return _strongAttackChargeDelay; }
        set { _strongAttackChargeDelay = value; }
    }

    /// <summary>
    /// the base radius of the circle AOE
    /// </summary>
    [SerializeField]
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
    private float _lightAttackStunDuration;
    public float LightAttackStunDuration
    {
        get { return _lightAttackStunDuration; }
        set { _lightAttackStunDuration = value; }
    }

    [SerializeField]
    private float _strongAttackRecoverTime = 0.75f;
    public float StrongAttackRecoverTime
    {
        get { return _strongAttackRecoverTime; }
        set { _strongAttackRecoverTime = value; }
    }

    [SerializeField]
    private float _strongAttackCooldown = 7.5f;
    public float StrongAttackCooldown
    {
        get { return _strongAttackCooldown; }
        set { _strongAttackCooldown = value; }
    }

    [SerializeField]
    private float _lightAttackCooldown = 3.5f;
    public float LightAttackCooldown
    {
        get { return _lightAttackCooldown; }
        set { _lightAttackCooldown = value; }
    }

    private bool _chargingStrongHit = false;
    private float _startChargingTime;

    private bool _cdLightHit = false;
    private bool _cdStrongHit = false;

    private Controller _controller;

    void Awake()
    {
        _controller = GetComponent<Controller>();
    }

    void Start()
    {

    }

    void Update()
    {
        // start charging strong attack
        if (!_chargingStrongHit && _controller.Jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.LT))
        {
            Debug.Log("charging strong attack");
            _startChargingTime = Time.time;
            StartCoroutine("Co_StrongHit");
        }

        // unleash string attack
        if ((_chargingStrongHit && _controller.Jsm.GetButtonUp(JoyStickManager.e_XBoxControllerButtons.LT)) /*|| _controller.IsMoving*/)
        {
            Debug.Log("releasing strong attack");
            StopCoroutine("Co_StrongHit");
            StrongHit((Time.time - _startChargingTime) / _strongAttackChargeDelay);
        }

        // use light attack
        if (_controller.Jsm.GetButtonDown(JoyStickManager.e_XBoxControllerButtons.RT))
        {
            // anim.play(LightAttack);
        }
    }

    /// <summary>
    /// stun la cible.
    /// appelé par la point lorsqu'il est actif
    /// </summary>
    public void LightHit(GameObject target)
    {
        if (target.layer == 1 << LayerMask.NameToLayer("Player"))
        {
            Controller targetController = target.GetComponent<Controller>();
            targetController.Stun(LightAttackStunDuration);
            //targetController.Push(_controller.);
        }
    }

    /// <summary>
    /// stun en AOE circulaire.
    /// plus on proche de l'épicentre, plus le stun est long
    /// </summary>
    /// <param name="chargePercentage"></param> la puissance du stun, 100% -> radius += _strongAttackFullChargeRadiusIncrease
    void StrongHit(float chargePercentage)
    {
        // anim.play(StrongAttack)
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
        // anim.play(Charging)
        yield return new WaitForSeconds(ChargeDelay);
        StrongHit(1);
    }

    public IEnumerator Co_CDLightHit()
    {
        _cdLightHit = true;
        yield return new WaitForSeconds(LightAttackCooldown);
        _cdLightHit = false;
    }

    public IEnumerator Co_CDStrongtHit()
    {
        _cdStrongHit = true;
        yield return new WaitForSeconds(StrongAttackCooldown);
        _cdStrongHit = false;
    }
}
