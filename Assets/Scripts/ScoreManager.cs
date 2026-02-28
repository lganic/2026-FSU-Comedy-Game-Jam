using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int CurrentScore;

    [Header("Object References")]
    [SerializeField] public Text scoreText;
    [SerializeField] public GameObject scoreHandle;

    [Header("Animation")]
    [Range(0.1f, 2f)]
    public float animationLength = 1f;

    private float wiggle_time = 0;
    public bool is_wiggle = false;

    private void WiggleSign()
    {
        is_wiggle = true;
        wiggle_time = Time.time;
    }
    
    public void AddToScore(int score)
    {
        CurrentScore += score;
        scoreText.text = CurrentScore.ToString();
        WiggleSign();
    }

    private float wiggle(float x, float i)
    {
        return i * 4 * x * (1 - x);
    }

    private void Update()
    {
        // We just gotta do the animation update if we need to.
        
        if (is_wiggle)
        {
            float animation_timestep = (Time.time - wiggle_time) / animationLength;

            if (animation_timestep > 1)
            {
                animation_timestep = 1;
                is_wiggle = false;

            }

            float rot_wiggle = wiggle(animation_timestep, 15);
            float scale_wiggle = wiggle(animation_timestep, 1.1f);

            Debug.Log(rot_wiggle);

            scoreHandle.transform.eulerAngles = new Vector3(0, 0, rot_wiggle);
            scoreHandle.transform.localScale = new Vector3(scale_wiggle + 1, 1, 1);
        }
    }
}
