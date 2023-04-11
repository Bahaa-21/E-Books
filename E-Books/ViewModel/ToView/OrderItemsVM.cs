namespace E_Books.ViewModel.ToView;
public class OrderItemsVM
{
    public string Id { get; set; }
    public IList<object> Books { get; set; }
    public string Created { get; set; }
    public string TotalPice { get; set; }
}