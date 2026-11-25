using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [Header("Typing")]
    public float charsPerSecond = 40f;

    private Queue<string> sentences = new Queue<string>();
    private bool isTyping;
    private string fullSentence;

    private Dialogue activeDialogue;
    private DialogueNode currentNode;

    private QuestManager _questManager => ServiceHub.Instance.QuestManager;

    public bool IsInDialogue { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsInDialogue)
            DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null) return;

        activeDialogue = dialogue;
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.npcName;

        currentNode = GetNodeByQuestState(dialogue);

        CheckQuestTalkObjective(dialogue.npcName);

        sentences.Clear();
        if (currentNode != null && currentNode.sentences != null)
        {
            foreach (var s in currentNode.sentences)
            {
                if (!string.IsNullOrEmpty(s))
                    sentences.Enqueue(s);
            }
        }

        Debug.Log($"[DialogueManager] Starting dialogue with {dialogue.npcName}. Quest state: {(currentNode == dialogue.notStarted ? "Not Started" : currentNode == dialogue.inProgress ? "In Progress" : "Completed")}. Sentences count: {sentences.Count}");

        IsInDialogue = true;
        DisplayNextSentence();
    }

    private DialogueNode GetNodeByQuestState(Dialogue dialogue)
    {
        if (dialogue == null) return null;

        if (dialogue.quest == null)
            return dialogue.notStarted;

        int questID = dialogue.quest._questID;

        if (ServiceHub.Instance.QuestManager.IsQuestCompleted(questID))
            return dialogue.completed;

        if (ServiceHub.Instance.QuestManager.IsQuestActive(questID))
            return dialogue.inProgress;

        return dialogue.notStarted;
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = fullSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            TryGiveQuest();
            EndDialogue();
            return;
        }

        fullSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(fullSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        float delay = 1f / Mathf.Max(1f, charsPerSecond);

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }

    private void TryGiveQuest()
    {
        if (currentNode == null) return;

        if (!currentNode.givesQuest) return;

        Quest questToGive = currentNode.quest != null ? currentNode.quest : activeDialogue?.quest;

        if (questToGive == null) return;

        int qid = questToGive._questID;

        if (ServiceHub.Instance.QuestManager.IsQuestActive(qid)) return;
        if (ServiceHub.Instance.QuestManager.IsQuestCompleted(qid)) return;

        ServiceHub.Instance.QuestManager.AcceptQuest(questToGive);
        Debug.Log($"[DialogueManager] Quest '{questToGive._questTitle}' given after dialogue.");
    }

    private void CheckQuestTalkObjective(string npcName)
    {
        List<QuestProgress> questsToComplete = new List<QuestProgress>();

        foreach (var quest in _questManager._activeQuests)
        {
            if (quest.Quest._objectiveType == ObjectiveType.Talk &&
                quest.Quest._targetName == npcName)
            {
                questsToComplete.Add(quest);
            }
        }

        foreach (var quest in questsToComplete)
        {
            _questManager.CompleteQuest(quest.Quest._questID);
            Debug.Log($"[DialogueManager] Quest '{quest.Quest._questTitle}' completed by talking to {npcName}");
        }
    }

    private void EndDialogue()
    {
        IsInDialogue = false;
        dialoguePanel.SetActive(false);
        activeDialogue = null;
        currentNode = null;
        fullSentence = null;
    }
}