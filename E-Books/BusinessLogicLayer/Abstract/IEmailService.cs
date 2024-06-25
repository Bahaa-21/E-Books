using E_Books.ViewModel.FromView;

namespace E_Books.BusinessLogicLayer.Abstract;
public interface IEmailService
{
    void SendEmail(EmailDto emailDto);
}