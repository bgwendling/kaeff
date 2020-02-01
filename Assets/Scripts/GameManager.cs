using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text CurrencyView;
    private float currency = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToCurrency(float ammount)
    {
        setCurrency(currency + ammount);
    }

    private void setCurrency(float currency)
    {
        this.currency = currency;
        //update UI element displaying currency
        CurrencyView.text = "Currency " + currency.ToString("#.##");
    }
}
