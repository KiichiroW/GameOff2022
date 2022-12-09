using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    #region Public Members
    [Header("Text Managing")]
    [SerializeField] private string path;
    [SerializeField] private TextMeshProUGUI textbox;
    [SerializeField] private TextMeshProUGUI namebox;
    [SerializeField] private DialoguePlayer dialoguePlayer;

    [Header("Dialogue Settings")]
    [SerializeField] private int charLimit;
    [SerializeField] private float textSpeed;

    [Header("Art Managing")]
    [SerializeField] private SpriteRenderer girlSprite;
    [SerializeField] private Sprite[] sprites;
    #endregion


    #region Private Members
    // Singleton
    public static DialogueManager Instance { get; private set; }

    // Reader
    private StreamReader reader;

    // Dialogue Managing
    private string dialogue; // All dialogue text
    private string nextLine; // Next line to be showed
    private string charName; // Character Name
    [SerializeField] private string playerName; // Player's name
    #endregion

    private void Awake()
    {
        // Creates singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Initializes values
        textbox.text = "EMPTY";
        namebox.text = "EMPTY";
        nextLine = "";
        dialogue = "";

        // Stores text file into dialogue
        ReadText();

        // Reopens reader
        reader = new StreamReader(path);

        // Always skips first line
        reader.ReadLine();
    }

    void Update()
    {
        // Progress dialogue
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            GetNextLine();
        }
    }

    private void GetNextLine()
    {
        // Reads next line
        try 
        {
            nextLine = reader.ReadLine();
        }
        catch (Exception e)
        {
            Debug.Log("Failed to read line: " + e.Message);
        }

        // Display next line if present
        if (nextLine != null)
        {
            // Empty line so get name then display line
            if (nextLine.Equals(""))
            {
                // Sets name and art, try to only readlines in this one
                GetName(reader.ReadLine());
                GetArt(reader.ReadLine());
                GetNextLine();
            }
            else
            {
                InsertName();
                //textbox.text = nextLine;
                dialoguePlayer.PlayNextLine(nextLine, textbox, textSpeed);
            }
        }
        else
        {
            Debug.Log("Next line is empty");
            reader.Close();
        }
    }

    private void GetName(String nameLine)
    {
        // Gets name
        charName = nameLine;
        
        // If textbox line is Player, sets name to player name
        if (charName.Equals("Player"))
        {
            namebox.text = playerName;
        }
        else if (charName.Equals("Narrator"))
        {
            namebox.text = "";
        }
        else
        {
            namebox.text = charName;
        }
    }

    // Inserts name into text
    private void InsertName()
    {
        if (nextLine.Contains("#name"))
        {
            nextLine = nextLine.Replace("#name", playerName);
        }
    }

    public void GetArt(String artLine)
    {
        int artIndex = Int32.Parse(artLine);

        if (artIndex != -1)
        {
            girlSprite.sprite = sprites[artIndex];
            Debug.Log("Changed Sprite to #: " + artIndex);
        }
    }

    // Parses through dating sim text files
    private void ReadText()
    {
        try
        {
            // Opens reader
            reader = new StreamReader(path);

            // Stores all dialogue
            dialogue = reader.ReadToEnd();
            
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Exception occured: " + e.Message);
        }
    }
}
