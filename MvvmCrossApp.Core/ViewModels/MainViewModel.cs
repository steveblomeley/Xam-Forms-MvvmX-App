using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;
using Xamarin.Forms;

namespace MvvmCrossApp.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();
        public ICommand ShowContactDetailsCommand { get; }

        private readonly IMvxNavigationService _navigationService;
        private readonly IContactService _contactService;

        public MainViewModel(IMvxNavigationService navigationService, IContactService contactService)
        {
            _contactService = contactService;
            _navigationService = navigationService;
            ShowContactDetailsCommand = new Command<Contact>(async contact => await ShowContactDetails(contact));
        }

        public string WelcomeText => "Xamarin Forms feat MvvmX";

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            foreach (var contact in _contactService.GetContacts())
            {
                Contacts.Add(contact);
            }
        }

        public async Task ShowContactDetails(Contact contact)
        {
            await _navigationService.Navigate<ContactDetailsViewModel, Contact>(contact);
        }
    }
}