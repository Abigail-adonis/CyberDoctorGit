using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mainpanel : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform midTransform;
    private int price;
    void Start()
    {

    }

    // Update is called once per frame
    private void OnEnable()
    {
        midTransform = transform.Find("Mid");
        midTransform.localPosition = new Vector3(1960, 0);
    }
    void Update()
    {
        if (midTransform.localPosition.x > 1)
        {
            midTransform.localPosition = Vector2.Lerp(midTransform.localPosition, new Vector2(0, 0), Time.deltaTime * 15f);
        }
    }

    public void SetPrice(int num)
    {
        price = num;
    }
}
