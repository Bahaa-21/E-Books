namespace E_Books.ViewModel.ToView;
public class OrderItemsVM
{
    public string Id { get; set; }
    public List<object> Book { get; set; }
    public string Created { get; set; }
    public string TotalPice { get; set; }
}