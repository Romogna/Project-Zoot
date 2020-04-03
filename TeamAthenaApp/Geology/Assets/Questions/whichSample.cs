using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct choice
{
    [TextArea(2, 5)]
    public string text;
//    public Conversation conversation;
}

[CreateAssetMenu(fileName = "New Question", menuName = "Question")]
public class whichSample : ScriptableObject{
    [TextArea(2, 5)]
    public string text;
    public choice[] choices;
}