using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    const float updateInterval = .5f;
    float accum;
    float frames;
    float timeLeft;
    float fpsCount;

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = updateInterval;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if(timeLeft <= 0)
        {
            fpsCount = accum / frames;
            text.text = fpsCount.ToString("f1");

            if (fpsCount < 30)
                text.color = Color.red;
            else if (fpsCount < 60)
                text.color = Color.yellow;
            else if (fpsCount > 60)
                text.color = Color.green;

            timeLeft = updateInterval;
            accum = 0;
            frames = 0;
        }
    }
}
