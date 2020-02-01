using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static readonly object threadLock = new object();
    [SerializeField]
    private string currencyPrefix = "Cash money flow:";
    [SerializeField]
    private Text CurrencyView;
    private float currency = 20.50f;

    [SerializeField]
    private List<GameObject> customers;
    //[SerializeField]
    //private List

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            lock (threadLock)
            {
                if (Instance == null)
                {
                    Instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }

        CurrencyView.text = currencyPrefix + " " + currency.ToString("c2");
        DontDestroyOnLoad(gameObject);
    }

    public void AddToCurrency(float ammount)
    {
        setCurrency(currency + ammount);
    }

    private void setCurrency(float currency)
    {
        this.currency = currency;
        //update UI element displaying currency
        CurrencyView.text = currencyPrefix + " " + currency.ToString("#.##");
    }
}
