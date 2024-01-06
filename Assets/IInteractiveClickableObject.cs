public interface IInteractiveClickableObject : ISnippetObject 
{
    public void SendClickObject();
}

public interface ISnippetObject
{
    public string GetSnippetText { get; }
    public bool HasSnippet { get; }
    public void DisplaySnippet();
}
