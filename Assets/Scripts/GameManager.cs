using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static readonly object threadLock = new object();
    
    private float currency = 20.50f;
    [SerializeField]
    private string currencyPrefix = "Cash money flow:";
    [SerializeField]
    private Text CurrencyView;
    
    [SerializeField]
    private Text dialogue;
    private GameObject dialogueRoot;
    private bool talking;
    
    [SerializeField]
    private List<GameObject> customers;
    
    private GameObject activeCharacterObject;
	private Character activeCharacter;

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
        initializeAndHideDialogue();
        initializeCustomers();
        CurrencyView.text = currencyPrefix + " " + currency.ToString("c2");
        DontDestroyOnLoad(gameObject);
    }




    private void initializeAndHideDialogue()
    {
        dialogueRoot = getParent(getParent(dialogue));
        dialogueRoot.SetActive(false);
        dialogue.text = "";
    }
    private void initializeCustomers()
    {
        updateActiveCharacter(customers[0]);
    }

    private void updateActiveCharacter(GameObject obj)
    {
        activeCharacter = obj.GetComponent<Character>();
        foreach (GameObject o in customers)
        {
            o.SetActive(false);
        }
        obj.SetActive(true);
        StartCoroutine(activeCharacter.Talk(dialogueRoot));
    }

    public void AddToCurrency(float ammount)
    {
        setCurrency(currency + ammount);
    }

    private void setCurrency(float currency)
    {
        this.currency = currency;
        //update UI element displaying currency
        CurrencyView.text = currencyPrefix + " " + currency.ToString("c2");
    }

	public Character GetActiveCharacter() { return activeCharacter; }
    private static GameObject getParent(Text obj)
    {
        return obj.transform.parent.gameObject;
    }


    private static GameObject getParent(GameObject obj)
    {
        return obj.transform.parent.gameObject;
    }

}
