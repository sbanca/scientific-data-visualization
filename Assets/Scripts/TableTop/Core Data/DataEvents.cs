

namespace TableTop
{


    // Class declaration
    [System.Serializable]
    public class OptionClicked : UnityEngine.Events.UnityEvent<string> { }

    // Class declaration
    [System.Serializable]
    public class TaskOptionClicked : UnityEngine.Events.UnityEvent<string, string> { }

    // Class declaration
    [System.Serializable]
    public class AddRemoveButtonClicked : UnityEngine.Events.UnityEvent<string> { }

   
}