namespace ControlExplorer
{
    public interface ISearchable
    {
        bool Contains(string word);
        bool ContainsAny(string[] searchKeys);
    }
}
