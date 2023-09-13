using UnityEngine;

public class GPTUIController : MonoBehaviour
{

    [SerializeField] Transform ConversationTransform;
    [SerializeField] GameObject textMessagePrefab;

    public void AddUserPrompt(string userPrompt)
    {
        GameObject go = GameObject.Instantiate(textMessagePrefab, ConversationTransform);
        TextMessageUIController textMessageUI = go.GetComponent<TextMessageUIController>();

        textMessageUI.Init("User", userPrompt);
    }

    public void AddGPTPrompt(string gptPrompt)
    {
        GameObject go = GameObject.Instantiate(textMessagePrefab, ConversationTransform);
        TextMessageUIController textMessageUI = go.GetComponent<TextMessageUIController>();

        textMessageUI.Init("Chat GPT", gptPrompt);
    }
}
