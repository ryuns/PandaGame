using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Tell unity thats its Serializable
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;

        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth;  }
            set { _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
        }
        public void Init ()
        {
            currentHealth = maxHealth;
        }

        public int damage = 5;

    }

    // Creating an instance of it
    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;
    public Transform hitParticles;

    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start ()
    {
        stats.Init ();

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth);
        }
        if (deathParticles == null)
        {
            Debug.LogError ("No death particles referenced on Enemy");
        }
    }

    public void DamageEnemy (int damage)
    {
        stats.currentHealth -= damage;
        Transform _clone = Instantiate(hitParticles, transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 20f);
        if (stats.currentHealth <= 0)
        {
            Debug.Log ("Enemy Died");
            GameMaster.KillEnemy (this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth);
        }
    }
    void OnCollisionEnter2D (Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer (stats.damage);
            stats.currentHealth -= stats.damage;
            //DamageEnemy(9999999);
        }
    }

}
