namespace UI
{
    public interface ITutorialMaskOperator
    {
        public string[] GetLastHighlight{get;}
        public void SetHighlightState(string[] newHightlight);
    }
}