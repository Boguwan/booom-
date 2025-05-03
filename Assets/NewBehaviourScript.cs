using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public int id;
        public string character;
        public string position;
        public string content;
        public int nextId;
    }

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public Button continueButton;

    [Header("Dialogue Data")]
    public TextAsset csvFile;
    public Sprite bossSprite;
    public Sprite heroSprite;
    public Sprite octopusSprite;

    private Dictionary<int, DialogueEntry> dialogueDictionary = new Dictionary<int, DialogueEntry>();
    private int currentDialogueId = 0;
    private bool isDialogueActive = false;

    // ��ӶԻ������¼�
    public delegate void DialogueEndedHandler();
    public event DialogueEndedHandler OnDialogueEnded;

    void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(ContinueDialogue);

        if (csvFile != null)
        {
            ParseCSVData();
        }
        else
        {
            Debug.LogError("CSV file not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDialogueActive)
        {
            StartDialogue(0);
        }
    }

    private void ParseCSVData()
    {
        string[] lines = csvFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');

            if (columns[0].Trim() == "END") break;

            DialogueEntry entry = new DialogueEntry();

            entry.id = int.Parse(columns[1].Trim());
            entry.character = columns[2].Trim();
            entry.position = columns[3].Trim();
            entry.content = columns[4].Trim();
            entry.nextId = int.Parse(columns[5].Trim());

            dialogueDictionary.Add(entry.id, entry);
        }
    }

    public void StartDialogue(int startId)
    {
        if (dialogueDictionary.Count == 0 || !dialogueDictionary.ContainsKey(startId))
        {
            Debug.LogError("Dialogue not found for ID: " + startId);
            return;
        }

        isDialogueActive = true;
        currentDialogueId = startId;
        dialoguePanel.SetActive(true);
        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        if (!dialogueDictionary.ContainsKey(currentDialogueId))
        {
            EndDialogue();
            return;
        }

        DialogueEntry currentEntry = dialogueDictionary[currentDialogueId];

        speakerText.text = currentEntry.character;
        dialogueText.text = currentEntry.content;
        UpdateCharacterImages(currentEntry.character, currentEntry.position);
    }

    private void UpdateCharacterImages(string character, string position)
    {
        leftCharacterImage.gameObject.SetActive(false);
        rightCharacterImage.gameObject.SetActive(false);

        Image targetImage = position == "��" ? leftCharacterImage : rightCharacterImage;
        targetImage.gameObject.SetActive(true);

        switch (character)
        {
            case "BOSS":
                targetImage.sprite = bossSprite;
                break;
            case "����":
                targetImage.sprite = heroSprite;
                break;
            default:
                targetImage.sprite = null;
                break;
        }
    }

    public void ContinueDialogue()
    {
        if (!dialogueDictionary.ContainsKey(currentDialogueId))
        {
            EndDialogue();
            return;
        }

        int nextId = dialogueDictionary[currentDialogueId].nextId;

        // ����Ƿ�����Ч����һ��ID
        if (nextId >= 0 && dialogueDictionary.ContainsKey(nextId))
        {
            currentDialogueId = nextId;
            ShowCurrentDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        // �������жԻ����״̬
        isDialogueActive = false;
        currentDialogueId = 0;

        // ����UIԪ��
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // ���ؽ�ɫͼ��
        if (leftCharacterImage != null)
        {
            leftCharacterImage.gameObject.SetActive(false);
        }
        if (rightCharacterImage != null)
        {
            rightCharacterImage.gameObject.SetActive(false);
        }

        // ����ı�
        if (speakerText != null)
        {
            speakerText.text = "";
        }
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        // �����Ի������¼�
        OnDialogueEnded?.Invoke();

        Debug.Log("Dialogue ended successfully.");
    }
}

