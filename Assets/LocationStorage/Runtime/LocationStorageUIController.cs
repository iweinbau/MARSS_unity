using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationStorageUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressLabel;
    private TextMeshProUGUI finishedLabel;

    private string gameStartText = "Place some objects in the world";
    public void ShowReachedVisitObject(int reached, int total)
    {
        Debug.Log("Update UI");
        progressLabel.text = $"Progress: {reached}/{total}";
    }
}
