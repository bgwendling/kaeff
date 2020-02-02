using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private List<GameObject> customers;

    [SerializeField]
    private AudioSource musicPlayer;
    [SerializeField]
    private List<AudioClip> musicClips = new List<AudioClip>();

    private int? nextCharacter;

    private Character activeCharacter;

    bool talking = true;
    [SerializeField]
    private GameObject cup;

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
        cup.SetActive(false);
        initializeAndHideDialogue();
        initializeCustomers();
        CurrencyView.text = currencyPrefix + " " + currency.ToString("c2");
        DontDestroyOnLoad(gameObject);
    }

    private void RandomizeMusic()
    {
        musicPlayer.clip = musicClips[Random.Range(0, musicClips.Count)];
    }

    private void initializeAndHideDialogue()
    {
        dialogueRoot = getParent(getParent(dialogue));
        dialogueRoot.SetActive(false);
        dialogue.text = "";
    }
    private void initializeCustomers()
    {
        if (nextCharacter != null && nextCharacter >= customers.Count)
            nextCharacter = customers.Count - 1;
        updateActiveCharacter(customers[nextCharacter ?? Random.Range(0, customers.Count)]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            musicPlayer.clip = musicClips[musicClips.IndexOf(musicPlayer.clip) == 0 ? 1 : 0];
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
        if (!talking)
        {
            nextCharacter = nextCharacter ?? Random.Range(0, customers.Count);
            if (nextCharacter != null && nextCharacter < customers.Count)
            {
                StartCoroutine(WaitForSeconds(Random.Range(5f, 20f)));
                updateActiveCharacter(customers[(int)nextCharacter]);
            }
        }
    }

    private void updateActiveCharacter(GameObject obj)
    {
        talking = true;
        nextCharacter = null;
        activeCharacter = obj.GetComponent<Character>();
        foreach (GameObject o in customers)
        {
            o.SetActive(false);
        }
        obj.SetActive(true);
        StartCoroutine(activeCharacter.Talk(dialogueRoot, cup));
    }

    public void SetTalking(bool talking)
    {
        this.talking = talking;
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

    public void SetNextCharacter(int? characterIndex)
    {
        nextCharacter = characterIndex;
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

    IEnumerator WaitForSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
