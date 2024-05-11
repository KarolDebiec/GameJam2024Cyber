using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarColor : MonoBehaviour
{
    public Slider slider;
    public Image image;

    void Update()
    {
        image.color = new Color(1,(1-((slider.value-1)/1.8f)),0);
    }

}
