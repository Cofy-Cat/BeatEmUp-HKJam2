using DocumentFormat.OpenXml.Office2010.Excel;
using UnityEngine;

public class AA : MonoBehaviour
{
    [SerializeField] private float warningTime = 5f;
    [SerializeField] private float enableTime = 20f;
    [SerializeField] private GameObject scan;
    [SerializeField] private GameObject bullets;
    private float warningTimer;
    private float enableTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        warningTimer = Time.time + warningTime;
        enableTimer = Time.time + enableTimer;
        gameObject.SetActive(true);
        scan.SetActive(true);
        bullets.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > warningTimer) {
            scan.SetActive(false);
            bullets.SetActive(true);
        }

        //if (Time.time > enableTimer) gameObject.SetActive(false);
    }
}
