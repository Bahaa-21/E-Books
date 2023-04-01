namespace E_Books.ViewModel.ToView;

public class PublisherBooksVM
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IList<string> BookTitle { get; set; }
}
