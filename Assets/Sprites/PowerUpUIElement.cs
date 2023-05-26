using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIElement : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image fillImage;
    float duration;
    ConsumableType type;
    public bool enablePowerUp;
    public void Init(Sprite _icon, float _duration,ConsumableType _type)
    {
        icon.sprite = _icon;
        duration = _duration;
        type = _type;
        enablePowerUp = true;
    }
    public ConsumableType GetConsumableType()
    {
        return type;
    }
    void Update()
    {
        if (enablePowerUp)
        {
            fillImage.fillAmount -= 1.0f / duration * Time.deltaTime;
            if (fillImage.fillAmount <= 0)
            {
                enablePowerUp = false;
                Destroy(gameObject);
            }
        }
    }
}
