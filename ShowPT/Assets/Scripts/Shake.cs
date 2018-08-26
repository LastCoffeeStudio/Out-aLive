using UnityEngine;

public class Shake
{
    public enum ShakeState
    {
        SHAKING,
        FADING_IN,
        FADING_OUT,
        END
    }

    public float shakeTime;
    public float timeToFadeIn;
    public float timeToFadeOut;
    public float speed;
    public float magnitude;
    public ShakeState state;

    private float elapsedTime;
    private float actualSpeed;
    private float actualMagnitude;
    private float perlinIndex;
    private float cameraPivotZPos;
    

    public Shake(float shakeTime, float timeToFadeIn, float timeToFadeOut, float speed, float magnitude, float cameraPivotZPos)
    {
        this.shakeTime = shakeTime;
        this.timeToFadeIn = timeToFadeIn;
        this.timeToFadeOut = timeToFadeOut;
        this.speed = speed;
        this.magnitude = magnitude;
        state = ShakeState.FADING_IN;
        elapsedTime = 0f;
        actualSpeed = 0f;
        actualMagnitude = 0f;
        perlinIndex = Random.Range(0f, 20f);
        
        this.cameraPivotZPos = cameraPivotZPos;
    }

    public Vector3 shakeCamera()
    {
        Debug.Log(elapsedTime);
        elapsedTime += Time.deltaTime;

        switch (state)
        {
            case ShakeState.FADING_IN:

                actualSpeed = Mathf.Lerp(0f, speed, elapsedTime / timeToFadeIn);
                actualMagnitude = Mathf.Lerp(0f, magnitude, elapsedTime / timeToFadeIn);
                if (elapsedTime >= timeToFadeIn)
                {
                    elapsedTime = 0f;
                    state = ShakeState.SHAKING;
                }
                break;

            case ShakeState.SHAKING:

                if (elapsedTime >= shakeTime)
                {
                    elapsedTime = 0f;
                    state = ShakeState.FADING_OUT;
                }
                break;

            case ShakeState.FADING_OUT:

                actualSpeed = Mathf.Lerp(speed, 0f, elapsedTime / timeToFadeOut);
                actualMagnitude = Mathf.Lerp(magnitude, 0f, elapsedTime / timeToFadeOut);
                if (elapsedTime >= timeToFadeOut)
                {
                    state = ShakeState.END;
                }
                break;
        }

        perlinIndex += Time.deltaTime * actualSpeed;

        float xPerlin = Mathf.PerlinNoise(perlinIndex, 0f) * 2f - 1f;
        float yPerlin = Mathf.PerlinNoise(0f, perlinIndex) * 2f - 1f;

        return new Vector3(xPerlin * actualMagnitude, yPerlin * actualMagnitude, cameraPivotZPos);
    }
}
