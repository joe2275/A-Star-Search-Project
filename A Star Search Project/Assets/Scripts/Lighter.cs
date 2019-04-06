using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour
{
    public Light photon;

    private float minLuminosity;
    private float maxLuminosity;

    void Start()
    {
        if(photon == null)
        {
            photon = GetComponentInChildren<Light>();
        }
        minLuminosity = photon.intensity * (1f - 0.1f);
        maxLuminosity = photon.intensity * (1f + 0.05f);
        StartCoroutine(fadeInAndOutRepeat(photon));
    }

    void Update()
    {
 
        
    }

    IEnumerator fadeInAndOut(Light lightToFade, bool fadeIn, float duration)
    {
        float counter = 0f;


        float a, b;

        if (fadeIn)
        {
            a = minLuminosity;
            b = maxLuminosity;
        }
        else
        {
            a = maxLuminosity;
            b = minLuminosity;
        }

        float currentIntensity = lightToFade.intensity;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            lightToFade.intensity = Mathf.Lerp(a, b, counter / duration);

            yield return null;
        }
    }

    IEnumerator fadeInAndOutRepeat(Light lightToFade)
    {
        float duration;

        while (true)
        {
            duration = Random.Range(0.05f, 0.2f);
            yield return fadeInAndOut(lightToFade, false, duration);
            duration = Random.Range(0.05f, 0.2f);
            yield return new WaitForSeconds(duration);
            duration = Random.Range(0.05f, 0.2f);
            yield return fadeInAndOut(lightToFade, true, duration);
        }
    }
}
