using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRemove : MonoBehaviour
{
    float lerpTime = 2.0f;
    List<Color> colors;
    SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        colors = new List<Color>() { new Color(0.458f, 1, 0.4198f), Color.green, Color.black };
        StartCoroutine(ColorLerp());    // Start color cycle
        Destroy(gameObject, 2f);      // Kill this in 3.5 seconds
    }

    IEnumerator ColorLerp()
    {
        for (int i = 1; i < colors.Count; i++)
        {
            float startTime = Time.time;
            float percentageComplete = 0;

            while (percentageComplete < 1)
            {
                float elapsedTime = Time.time - startTime;
                percentageComplete = elapsedTime / (lerpTime / (colors.Count - 1));
                rend.color = Color.Lerp(colors[i - 1], colors[i], percentageComplete);

                yield return null;
            }
        }
    }
}
