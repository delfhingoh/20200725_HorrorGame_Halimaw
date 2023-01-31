using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class FirstPersonManager : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private int maxStressLimit;
    [SerializeField] private Slider stressSlider;

    private Vignette vignetteOverlay;
    private bool isVignetteShown;

    private float stressLevel;
    private bool isHiding;

    private void Start()
    {
        vignetteOverlay = postProcessVolume.profile.GetSetting<Vignette>();
        isVignetteShown = false;

        stressLevel = 0f;
    }

    public Vignette GetVignetteOverlay()
    {
        return vignetteOverlay;
    }

    public bool GetIsVignetteShown()
    {
        return isVignetteShown;
    }

    public void SetIsVignetteShown(bool _isShown)
    {
        isVignetteShown = _isShown;
    }

    public bool GetIsPlayerHiding()
    {
        return isHiding;
    }

    public void SetIsPlayerHiding(bool _isHidden)
    {
        isHiding = _isHidden;
    }

    public float GetStressLevel()
    {
        return stressLevel;
    }

    public void SetStressLevel(float _stressLevel)
    {
        stressLevel = _stressLevel;
    }

    public int GetMaxStressLevel()
    {
        return maxStressLimit;
    }

    public void SetStressSlider(float _stressLevel)
    {
        stressSlider.value = _stressLevel;
    }

    public Slider GetStressSlider()
    {
        return stressSlider;
    }
}
