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
    private int dialogueIndex = -1;
    private float timeLimit = 0;
    private float timeSinceLast = 0;
    Speech speech = null;
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
        initializeCustomers();
        initializeAndHideDialogue();
        CurrencyView.text = currencyPrefix + " " + currency.ToString("c2");
        DontDestroyOnLoad(gameObject);
    }


    //private async Task progress()
    //{
    //    Speech s = activeCharacter.talk();
    //    foreach(Entry statement in s.statements){
    //        dialogue.text = statement.text;
    //        dialogueRoot.SetActive(true);
    //        var wt = System.Math.Ceiling(statement.text.Length / 10f);
            
    //        //wait 20 seconds
    //        //dialogueRoot.SetActive(false);
    //    }
    //    return Task.CompletedTask;
    //}

    //dirty hack
    private void Update()
    {
        if (talking)
        {
            timeSinceLast += Time.deltaTime;
            if (timeSinceLast >= timeLimit)
            {
                if (speech == null)
                {
                    speech = activeCharacter.talk();
                }
                dialogueRoot.SetActive(true);
                dialogue.text = speech.statements[++dialogueIndex].text;
                timeLimit = dialogue.text.Length / 5f;
                if (dialogueIndex == speech.statements.Count)
                {
                    talking = false;
                    timeSinceLast = 0;
                    dialogueIndex = -1;
                    timeLimit = 0;
                }
            }
        }
    }

    private void initializeAndHideDialogue()
    {
        dialogueRoot = getParent(getParent(dialogue));
        dialogueRoot.SetActive(false);
    }
    private void initializeCustomers()
    {
        updateActiveCharacter(customers[0]);
    }

    private void updateActiveCharacter(GameObject obj)
    {
        foreach (GameObject o in customers)
        {
            o.SetActive(false);
        }
        obj.SetActive(true);
        talking = true;
        activeCharacter = customers[0].GetComponent<Character>();
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


    private static GameObject getParent(Text obj)
    {
        return obj.transform.parent.gameObject;
    }


    private static GameObject getParent(GameObject obj)
    {
        return obj.transform.parent.gameObject;
    }

}
