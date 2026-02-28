using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TextureSwitcher : MonoBehaviour
{
    [Header("UI Warning")]
    public TMP_Text warningText;
    public float warningDuration = 3f;

    private Coroutine warningCoroutine;

    [Header("Structures")]
    public GameObject Day;
    public GameObject Night;

    [Header("Player")]
    public GameObject player;

    [Header("Materials")]
    public Material material1;
    public Material material2;

    [Header("Material 1 Textures")]
    public Texture mat1TextureA;
    public Texture mat1TextureB;

    [Header("Material 2 Textures")]
    public Texture mat2TextureA;
    public Texture mat2TextureB;

    [Header("Lighting")]
    public Light directionalLight;
    public Color dayLightColor = new Color(1f, 0.65f, 0.3f); // Orange / Golden Hour
    public Color nightLightColor = new Color(0.2f, 0.3f, 0.5f); // Faint Blue

    [Header("Post-Processing")]
    public Volume postProcessVolume;

    [Header("Day/Night PP Settings")]
    public float dayBloomIntensity = 1f;
    public float nightBloomIntensity = 0.2f;

    public float dayExposure = 1f;
    public float nightExposure = 0.3f;

    public float dayVignette = 0f;
    public float nightVignette = 0.4f;

    public float dayTemperature = 15f; // warm golden
    public float nightTemperature = -10f; // cool blue

    // Post Processing Components
    private Bloom bloom;
    private ColorAdjustments colorAdjust;
    private Vignette vignette;
    private WhiteBalance whiteBalance;

    [Header("Skybox Options")]
    public Material skyboxA;
    public Material skyboxB;

    [Header("Terrain")]
    public Terrain terrain;
    public int terrainLayerIndex = 0;

    public Texture2D terrainTextureA;
    public Texture2D terrainTextureB;

    public bool usingSetA = false;

    [Header("Exclude From Collision Check")]
    public GameObject excludeFromDayCheck;


    private void Start()
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out bloom);
            postProcessVolume.profile.TryGet(out colorAdjust);
            postProcessVolume.profile.TryGet(out vignette);
            postProcessVolume.profile.TryGet(out whiteBalance);
        }

        if (usingSetA)
        {
            material1.mainTexture = mat1TextureB;
            material2.mainTexture = mat2TextureB;

            RenderSettings.skybox = skyboxB;

            Night.SetActive(false);
            Day.SetActive(true);
            
            // Day Lighting
            if (directionalLight != null)
            {
                directionalLight.intensity = 1f;
                directionalLight.color = dayLightColor;
            }

            // Post-Processing
            if (bloom != null) bloom.intensity.value = dayBloomIntensity;
            
            if (colorAdjust != null)
            {
                colorAdjust.active = true;
                colorAdjust.postExposure.overrideState = true;
                colorAdjust.postExposure.value = dayExposure;
            }

            if (whiteBalance != null)
            {
                whiteBalance.active = true;
                whiteBalance.temperature.overrideState = true;
                whiteBalance.temperature.value = dayTemperature;
            }

            if (vignette != null) vignette.intensity.value = dayVignette;

            // Fog
            RenderSettings.fogColor = Color.gray;
            RenderSettings.fogDensity = 0.001f;
        }
        // ---------------------------------------------------------
        // SET A: NIGHT SETTINGS
        // ---------------------------------------------------------
        else
        {
            material1.mainTexture = mat1TextureA;
            material2.mainTexture = mat2TextureA;

            RenderSettings.skybox = skyboxA;

            Night.SetActive(true);
            Day.SetActive(false);
            
            // Night Lighting
            if (directionalLight != null)
            {
                directionalLight.intensity = 0.05f;
                directionalLight.color = nightLightColor;
            }

            // Post-Processing
            if (bloom != null) bloom.intensity.value = nightBloomIntensity;
            
            if (colorAdjust != null)
            {
                colorAdjust.active = true;
                colorAdjust.postExposure.overrideState = true;
                colorAdjust.postExposure.value = nightExposure;
            }

            if (whiteBalance != null)
            {
                whiteBalance.active = true;
                whiteBalance.temperature.overrideState = true;
                whiteBalance.temperature.value = nightTemperature;
            }

            if (vignette != null) vignette.intensity.value = nightVignette;

            // Fog
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.01f;
        }

        TerrainLayer[] layers = terrain.terrainData.terrainLayers;

        if (layers.Length > terrainLayerIndex)
        {
            if (usingSetA)
                layers[terrainLayerIndex].diffuseTexture = terrainTextureB;
            else
                layers[terrainLayerIndex].diffuseTexture = terrainTextureA;

            terrain.terrainData.terrainLayers = layers;
        }

        usingSetA = !usingSetA;

        DynamicGI.UpdateEnvironment();
    }

    public void SwitchTextures()
    {
        Collider playerCol = player.GetComponent<Collider>();

        if (playerCol == null)
        {
            Debug.LogWarning("Player has no collider!");
            return;
        }

        // Check Day hierarchy
        Collider[] dayColliders = Day.GetComponentsInChildren<Collider>(true);

        foreach (Collider col in dayColliders)
        {
            // Skip excluded object and its children
            if (excludeFromDayCheck != null &&
                col.gameObject == excludeFromDayCheck)
            {
                continue;
            }

            if (playerCol.bounds.Intersects(col.bounds))
            {
                if (warningCoroutine != null)
                    StopCoroutine(warningCoroutine);

                warningCoroutine = StartCoroutine(ShowWarning("Cannot switch, try moving around"));
                return;
            }
        }

        // Check Night hierarchy
        Collider[] nightColliders = Night.GetComponentsInChildren<Collider>(true);
        foreach (Collider col in nightColliders)
        {
            if (playerCol.bounds.Intersects(col.bounds))
            {
                Debug.Log("Cannot switch — player intersecting Night structure.");
                return;
            }
        }

        // ---------------------------------------------------------
        // SET B: DAY SETTINGS
        // ---------------------------------------------------------
        if (usingSetA)
        {
            material1.mainTexture = mat1TextureB;
            material2.mainTexture = mat2TextureB;

            RenderSettings.skybox = skyboxB;

            Night.SetActive(false);
            Day.SetActive(true);
            
            // Day Lighting
            if (directionalLight != null)
            {
                directionalLight.intensity = 1f;
                directionalLight.color = dayLightColor;
            }

            // Post-Processing
            if (bloom != null) bloom.intensity.value = dayBloomIntensity;
            
            if (colorAdjust != null)
            {
                colorAdjust.active = true;
                colorAdjust.postExposure.overrideState = true;
                colorAdjust.postExposure.value = dayExposure;
            }

            if (whiteBalance != null)
            {
                whiteBalance.active = true;
                whiteBalance.temperature.overrideState = true;
                whiteBalance.temperature.value = dayTemperature;
            }

            if (vignette != null) vignette.intensity.value = dayVignette;

            // Fog
            RenderSettings.fogColor = Color.gray;
            RenderSettings.fogDensity = 0.001f;
        }
        // ---------------------------------------------------------
        // SET A: NIGHT SETTINGS
        // ---------------------------------------------------------
        else
        {
            material1.mainTexture = mat1TextureA;
            material2.mainTexture = mat2TextureA;

            RenderSettings.skybox = skyboxA;

            Night.SetActive(true);
            Day.SetActive(false);
            
            // Night Lighting
            if (directionalLight != null)
            {
                directionalLight.intensity = 0.05f;
                directionalLight.color = nightLightColor;
            }

            // Post-Processing
            if (bloom != null) bloom.intensity.value = nightBloomIntensity;
            
            if (colorAdjust != null)
            {
                colorAdjust.active = true;
                colorAdjust.postExposure.overrideState = true;
                colorAdjust.postExposure.value = nightExposure;
            }

            if (whiteBalance != null)
            {
                whiteBalance.active = true;
                whiteBalance.temperature.overrideState = true;
                whiteBalance.temperature.value = nightTemperature;
            }

            if (vignette != null) vignette.intensity.value = nightVignette;

            // Fog
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.01f;
        }

        TerrainLayer[] layers = terrain.terrainData.terrainLayers;

        if (layers.Length > terrainLayerIndex)
        {
            if (usingSetA)
                layers[terrainLayerIndex].diffuseTexture = terrainTextureB;
            else
                layers[terrainLayerIndex].diffuseTexture = terrainTextureA;

            terrain.terrainData.terrainLayers = layers;
        }

        usingSetA = !usingSetA;


        DynamicGI.UpdateEnvironment();
    }

    private IEnumerator ShowWarning(string message)
    {
        warningText.text = message;
        Color originalColor = warningText.color;
        originalColor.a = 1f;
        warningText.color = originalColor;

        float elapsed = 0f;

        while (elapsed < warningDuration)
        {
            elapsed += Time.deltaTime;
            Color c = warningText.color;
            c.a = Mathf.Lerp(1f, 0f, elapsed / warningDuration);
            warningText.color = c;
            yield return null;
        }

        // Ensure fully invisible
        Color final = warningText.color;
        final.a = 0f;
        warningText.color = final;
    }
}