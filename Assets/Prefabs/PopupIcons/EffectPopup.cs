using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectPopup : MonoBehaviour
{

    float lifeSpanRemaining;
    [SerializeField] TextMeshProUGUI numberText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeSpanRemaining < 0)
        {
            Destroy(gameObject);
        }

        lifeSpanRemaining -= Time.deltaTime;
    }

    public void SetUp(int number, float lifeSpan)
    {
        if (number > 0) {
            numberText.text = "+" + number.ToString();
        }
        else
        {
            numberText.text = number.ToString();
        }

        lifeSpanRemaining = lifeSpan;
    }


}
