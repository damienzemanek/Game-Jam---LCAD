using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected UnityEventPlus onDeathHook;

    private void Awake()
    {
        if (onDeathHook == null) onDeathHook = new UnityEventPlus();
        maxHp = currentHp;
        UpdateSliderUI();

    }

    private void OnEnable()
    {
        UpdateSliderUI();
    }


    [Button]
    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        UpdateSliderUI();

        if (currentHp < 0)
        {
            onDeathHook?.get?.Invoke();
            Die();
        }
    }

    public void UpdateSliderUI()
    {
        this.Log("Updating slider value");
        float sliderVal = currentHp / maxHp;
        slider.value = sliderVal;
    }

    public abstract void Die();
}
