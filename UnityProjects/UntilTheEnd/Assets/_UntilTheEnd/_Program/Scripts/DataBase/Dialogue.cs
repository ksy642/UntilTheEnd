public class Dialogue
{
    public string sceneNameCSV;
    public int numberCSV;
    public string npcCSV;
    public bool answerCSV;
    public string dialogueCSV;

    public Dialogue(string sceneName, int number, string npc, bool answer, string dialogue)
    {
        sceneNameCSV = sceneName;
        numberCSV = number;
        npcCSV = npc;
        answerCSV = answer;
        dialogueCSV = dialogue;
    }
}