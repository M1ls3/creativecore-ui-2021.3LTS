using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunRotation : MonoBehaviour
{

    public Transform sunTransform;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        // You can remove this line from Update, as it's better to add the listener once in Start
    }

    private void OnSliderValueChanged(float value)
    {
        // Calculate the new rotation angle based on the slider value
        float rotationAngle = value;

        // Apply the rotation, combining it with the start angle to maintain the initial orientation
        sunTransform.rotation = Quaternion.Euler(value, 291.261f, 12.882f);
    }
}
