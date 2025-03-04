using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AddSlider : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.AddSlider(GetComponent<Slider>());
    }
}
