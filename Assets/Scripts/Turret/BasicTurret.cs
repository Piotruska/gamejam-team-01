using Interface;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BasicTurret : MonoBehaviour, ITurret, IDamagable
{
    [Header("Turrets")]
    [SerializeField] private GameObject _forceField;
    [SerializeField] private Light2D _lightFiled;

    [Header("Turret settings")]
    [SerializeField] private int _MaxHealth = 10;
    [SerializeField] private GameObject _Gores;

    private Rigidbody2D _rb;
    private GameObject _mount;
    private MountTrack _mountTrack;
    private Transform _mountTrans;
    private GameObject _cannon;
    private CannonShoot _cannonShoot;
    private Transform _cannonTrans;
    private int _currentHealth;

    private bool _powerField;

    private void Awake()
    {
        _currentHealth = _MaxHealth;
        _powerField = true;
        _rb = gameObject.GetComponent<Rigidbody2D>();

        _mountTrans = transform.Find("Mount");
        if (_mountTrans != null)
        {
            _mount = _mountTrans.gameObject;

            _cannonTrans = _mountTrans.Find("Cannon");

            if (_cannonTrans != null)
            {
                _cannon = _cannonTrans.gameObject;
            }
        }

        _cannonShoot = _cannon.GetComponent<CannonShoot>();
        _mountTrack = _mountTrans.gameObject.GetComponent<MountTrack>();
    }

    private void Start()
    {
        if (_mount == null || _cannon == null)
        {
            Debug.LogError("B");
        }
    }

    public void PowerFieldOff()
    {
        _powerField = false;
        Destroy(_forceField);
        _lightFiled.intensity = 0;
    }

    public void TakeDamage(int damage)
    {
        if (_currentHealth <= 0)
        {
            Destruct();
        }
        else if (!_powerField)
        {
            _currentHealth -= damage;
        }
    }

    public void Destruct()
    {
        _mountTrack.destroyed = true;
        _cannonShoot.isDestroyed = true;
        _cannonShoot.canShoot = false;

        // FX
        Instantiate(_Gores, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
