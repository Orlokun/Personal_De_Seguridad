public interface IInteractiveClickableObject : ISnippetObject
{
    public void ReceiveActionClickedEvent();
    public void ReceiveDeselectObjectEvent();

    public void ReceiveSelectClickEvent();
}

public interface ISnippetObject
{
    public string GetSnippetText { get; }
    public bool HasSnippet { get; }
    public void DisplaySnippet();
}
