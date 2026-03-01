using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public Text TimerText;
    public Text CountdownText;

    public float TimerAmount = 0;
    private float TimerBase;
    private AudioSource ass;

    private float TimerStart;

    public int leadup = 5;

    public string TargetEndScene;

    public AudioClip ThingToSwapAudioToOnceTimerIsFinishedBecauseIAmTooLazyToMakeASeperateSoundSource;

    void StartTimer(float length)
    {
        TimerAmount = length;
        TimerStart = Time.time;
        ass.Play();
    }

    private void Start()
    {
        ass = gameObject.GetComponent<AudioSource>();
        StartTimer(165);
    }

    // Update is called once per frame
    void Update()
    {

        if (TimerAmount > 0)
        {
            float elapsed_time = Time.time - TimerStart;

            if (elapsed_time < leadup + 1)
            {
                string cd = ((int)(leadup - elapsed_time)).ToString();

                if (cd == "0")
                {
                    cd = "Go!";
                }

                CountdownText.text = cd;
            }

            else
            {
                // Switch the audio clip in the player to the goal meet. 
                gameObject.GetComponent<AudioSource>().clip = ThingToSwapAudioToOnceTimerIsFinishedBecauseIAmTooLazyToMakeASeperateSoundSource;
                gameObject.GetComponent<AudioSource>().volume = 1;

                CountdownText.text = "";

                elapsed_time = TimerAmount - (elapsed_time - leadup - 1);

                if (elapsed_time < 0)
                {
                    SceneManager.LoadScene(TargetEndScene);
                }

                string p1 = ((int)elapsed_time / 60).ToString();
                string p2 = (((int)elapsed_time) % 60).ToString();

                if (p2.Length == 1)
                {
                    p2 = "0" + p2;
                }

                TimerText.text = p1 + ":" + p2;
            }

        }
        
    }
}
