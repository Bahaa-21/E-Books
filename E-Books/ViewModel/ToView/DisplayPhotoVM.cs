namespace E_Books.ViewModel.ToView;

public class DisplayPhotoVM
{
    public string ProfilePhoto { get; set; }
    public string AddedOn {get;set;}
    public DisplayPhotoVM()
    {
        AddedOn = DateTime.UtcNow.ToString("f");
    }
}
