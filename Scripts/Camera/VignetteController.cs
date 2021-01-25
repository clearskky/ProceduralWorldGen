using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteController : MonoBehaviour
{
    public float vignetteDepthMultiplier;
    public int radarLevel;
    public Camera mainCamera;

    private PostProcessVolume ppv;
    private Vignette vignette;
    
    private void Start()
    {
        radarLevel = PlayerPrefs.GetInt("radarLevel", 0);
        vignetteDepthMultiplier = UpgradeStation.radarLevelModifiers[radarLevel];
        ppv = GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out vignette);
        vignette.intensity.value = 0f;
    }
    void FixedUpdate()
    {
        float vignetteIntensity = (Mathf.Abs(transform.position.y) / 1000f) * vignetteDepthMultiplier;
        vignetteIntensity = vignetteIntensity > 1 ? 1 : vignetteIntensity < 0 ? 0 : vignetteIntensity; // Clamp intensity to 1 if its greater than 1, clamp to 0 if its less than 0
        vignette.intensity.value = vignetteIntensity;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(Player.Instance.transform.position);
        vignette.center.value = new Vector2(Normalize(screenPos.x, Screen.width, 0), Normalize(screenPos.y, Screen.height, 0));
    }

    public float Normalize(float value, float maxValue, float minValue) 
    { 
        return (value - minValue) / (maxValue - minValue); 
    }

    public void AdjustVignetteProperties()
    {
        vignetteDepthMultiplier = UpgradeStation.radarLevelModifiers[radarLevel];
    }
}
