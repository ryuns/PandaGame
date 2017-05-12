using UnityEngine.UI;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;
    void start()
    {
        if (spawner == null)
        {
            Debug.Log("No spawner referenced!");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.Log("No spawner referenced!");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.Log("No spawner referenced!");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.Log("No spawner referenced!");
            this.enabled = false;
        }

    }
    void Update ()
    {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;

            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
        }
        previousState = spawner.State;
    }
    void UpdateCountingUI()
    {
        if (previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
            //Debug.Log("COUNTING");
        }
        //parses it into a interger (removing the decimal point), then converting it into a string
        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }
    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = spawner.NextWave.ToString();
            //Debug.Log("SPAWNING");
        }
    }


}
