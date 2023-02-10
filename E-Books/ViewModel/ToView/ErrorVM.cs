using Newtonsoft.Json;

namespace E_Books.ViewModel.ToView;
public class ErrorVM
{
    public int StatusCode { get; set; }
    public string Masseage { get; set; }
    public string Path {get; set;}

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}