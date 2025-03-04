using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reroad : MonoBehaviour
{
    [SerializeField] private Image image;
    private RectTransform trm;
    private Canvas canvas;
    private Camera cam;
    private IGun gun;

    private void Awake()
    {
        gun = GetComponent<IGun>();
        cam = Camera.main;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        trm = Instantiate(image, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform).GetComponent<RectTransform>();
        image = trm.GetComponent<Image>();
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
    }

    private void OnEnable()
    {
        gun.OnReroad += StartReroad;
    }

    private void OnDisable()
    {
        gun.OnReroad -= StartReroad;
    }

    private void Update()
    {
        trm.position = cam.WorldToScreenPoint(transform.right * 1.5f + transform.position);
    }

    private void StartReroad(float time)
    {
        StartCoroutine(Reroading(time));
    }

    private IEnumerator Reroading(float time)
    {
        float delta = 0f;

        while(delta <= time)
        {
            delta += Time.deltaTime;
            image.fillAmount = delta / time;
            yield return null;
        }

        image.fillAmount = 0f;
    }
}
