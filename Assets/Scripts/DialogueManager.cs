using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    // Singleton
    public static DialogueManager Instance { get; private set; }

    [Header("Text Managing")]
    [SerializeField] private string path;
    [SerializeField] private TextMeshProUGUI textbox;
    [SerializeField] private TextMeshProUGUI namebox;

    // Dialogue
    private string dialogue;
    private string nextLine;
    private string charName;
    [SerializeField] private string playerName;

    // Dialogue Settings
    [SerializeField] private int charLimit;
    // speed
    // insta skip

    // Reader
    private StreamReader reader;

    // Art Interaction
    [SerializeField] private SpriteRenderer girlSprite;
    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        // If Instance exists and isn't this one - destroy it
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
            // maybe try not to use a try in a game, intensive
            try
            {
                GetNextLine();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    private void GetNextLine()
    {
        // Reads next line
        nextLine = reader.ReadLine();

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
                textbox.text = nextLine;
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
