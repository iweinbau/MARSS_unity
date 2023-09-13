using TMPro;
using UnityEngine;

public class TextMessageUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI senderLabel;
    [SerializeField] TextMeshProUGUI messageLabel;

    public void Init(string sender, string message)
    {
        senderLabel.text = $"{sender}: ";
        messageLabel.text = message;
    }
}
