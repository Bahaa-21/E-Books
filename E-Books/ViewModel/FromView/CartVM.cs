namespace E_Books.ViewModel.FromView;
public class CartVM
{
    public int UserId {get; set;}
    public ICollection<int> CartBooks {get; set;}
}