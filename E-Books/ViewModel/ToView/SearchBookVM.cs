namespace E_Books.ViewModel.ToView;
public class SearchBookVM
{
    public int Id { get; set; }

    public string Title { get; set; }

    public ICollection<string> Authors { get; set; }
}