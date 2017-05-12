using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;

    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    public Transform spawnPrefab;
    public string respawnCountdownSound = "RespawnCountdown";
    public string playerRespawnSound = "Spawn";

    public string gameOverSound = "GameOver";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    //cache
    private AudioManager audioManager;

    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No camera shake refernced in gamemaster");
        }
        _remainingLives = maxLives;

        //caching
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("ERROR: audioManager == null, in GM script (No AudioManager found in the scene!)");
        }
    }
    public void EndGame()
    {
        audioManager.PlaySound(gameOverSound);

        Debug.Log("EndGame");
        gameOverUI.SetActive(true);
    }

    //Co-routines in unity, when using yield
    public IEnumerator _RespawnPlayer ()
    {
        audioManager.PlaySound(respawnCountdownSound);
        yield return new WaitForSeconds (spawnDelay);

        audioManager.PlaySound(playerRespawnSound);
        Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy (clone.gameObject, 3f);
    }
    //its a way to make sure theres only 1 instance of a class in any scene
    public static void KillPlayer (Player player)
    {
        Transform _clone = Instantiate(player.deathParticles, player.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 20f);

        Destroy (player.gameObject);
        _remainingLives -= 1;

        if (_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }
    public static void KillEnemy (Enemy enemy)
    {
        gm._killEnemy (enemy);
    }
    public void _killEnemy (Enemy _enemy)
    {
        //Play some sounds

        audioManager.PlaySound(_enemy.deathSoundName);
        //add particles
        Transform _clone = Instantiate (_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy (_clone.gameObject, 5f);

        //CameraShake
        cameraShake.Shake (_enemy.shakeAmt, _enemy.shakeLength);

        //Destroy gameobject
        Destroy (_enemy.gameObject);
    }
}