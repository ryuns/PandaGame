using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class LivesCounterUI : MonoBehaviour
{
    private Text LivesText;

    void Awake ()
    {
        LivesText = GetComponent<Text>();
    }

    void Update ()
    {
        LivesText.text = "LIVES: " + GameMaster.RemainingLives.ToString();


    }
}
