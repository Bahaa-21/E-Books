namespace E_Books.ViewModel.ToView;
public class OrderItemsVM
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; }
    public List<object> Books { get; set; }
    public string Created { get; set; }
    public string TotalPice { get; set; }
}