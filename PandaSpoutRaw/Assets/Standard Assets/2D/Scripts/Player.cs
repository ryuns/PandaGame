using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Tell unity thats its Serializable
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
        }

        public void Init ()
        {
            currentHealth = maxHealth;
        }

    }

    // Creating an instance of it
    public PlayerStats stats = new PlayerStats();

    public int fallBoundary = -20;

    public string deathSoundName = "deathvoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    public Transform deathParticles;

    void Start()
    {
        if (deathParticles == null)
        {
            Debug.LogError("No death particles referenced on Enemy");
        }
        stats.Init();
        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator referenced on Player");
        }
        else
        {
            statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth);
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("ERROR: no audiomanager in scene (player script)");
        }
    }
    public void DamagePlayer (int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            // play deathvoice
            audioManager.PlaySound(deathSoundName);
            Debug.Log("Player Died");
            // kill player
            GameMaster.KillPlayer(this);
            ArmRotation.rotationOffset = 360;
        }
        else
        {
            //play damage sound
            audioManager.PlaySound(damageSoundName);
        }

        statusIndicator.SetHealth (stats.currentHealth, stats.maxHealth);
    }
    void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer (9999999);
        }
    }

}
