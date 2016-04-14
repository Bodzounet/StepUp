using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller))]
public class Defiance : MonoBehaviour 
{
    private float _defiance;
    public float DefianceBar
    {
        get { return _defiance; }
        set 
        {
            _defiance = value;
            if (_defiance > MaxDefiance)
            {
                _defiance = MaxDefiance;
            }
            else if (_defiance < 0)
            {
                _defiance = 0;
                _controller.Stun(StunDurationWhenDefianceBarBreaks);
            }
        }
    }

    [SerializeField]
    private float _maxDefiance;
    public float MaxDefiance
    {
        get { return _maxDefiance; }
        set { _maxDefiance = value; }
    }

    [SerializeField]
    private float _stunDurationWhenDefianceBarBreaks = 1.5f;
    public float StunDurationWhenDefianceBarBreaks
    {
        get { return _stunDurationWhenDefianceBarBreaks; }
        set { _stunDurationWhenDefianceBarBreaks = value; }
    }

    [SerializeField]
    private float _delayBeforeRegen = 5f;
    public float DelayBeforeRegen
    {
        get { return _delayBeforeRegen; }
        set { _delayBeforeRegen = value; }
    }

    [SerializeField]
    private float _regenSpeed = 5f; // defiance point per second
    public float RegenSpeed
    {
        get { return _regenSpeed; }
        set { _regenSpeed = value; }
    }

    private Controller _controller;

    void Awake()
    {
        _controller = this.GetComponent<Controller>();
    }

	void Start () 
    {
        Regen();
	}
	
    public void Hit(float dmg)
    {
        CancelInvoke("Regen");
        StopCoroutine("Co_Regen");
        _defiance -= dmg;
        Invoke("Regen", DelayBeforeRegen);
    }

    public void Regen()
    {
        StartCoroutine("Co_Regen");
    }

    private IEnumerable Co_Regen()
    {
        while (true)
        {
            _defiance += _regenSpeed / 10;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
