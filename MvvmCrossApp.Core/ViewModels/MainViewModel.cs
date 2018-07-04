using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;

namespace MvvmCrossApp.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();
        public string ClickedCount => $"Clicked {_testCommandExecutedCount} times.";
        public IMvxAsyncCommand<Contact> ShowContactDetailsCommand { get; }
        public IMvxCommand TestCommand { get; }

        private readonly IMvxNavigationService _navigationService;
        private readonly IContactService _contactService;

        private int _testCommandExecutedCount;
        private int TestCommandExecutedCount
        {
            set
            {
                if (value == _testCommandExecutedCount) return;
                _testCommandExecutedCount = value;
                RaisePropertyChanged(() => ClickedCount);
            }
            get => _testCommandExecutedCount;
        }

        public MainViewModel(IMvxNavigationService navigationService, IContactService contactService)
        {
            _contactService = contactService;
            _navigationService = navigationService;
            ShowContactDetailsCommand = new MvxAsyncCommand<Contact>(async contact => await ShowContactDetails(contact));
            TestCommand = new MvxCommand(Test);
        }

        public string WelcomeText => "Xamarin Forms feat MvvmX";

        public override async Task Initialize()
        {
            await base.Initialize();

            foreach (var contact in _contactService.GetContacts())
            {
                Contacts.Add(contact);
            }
        }

        public async Task ShowContactDetails(Contact contact)
        {
            await _navigationService.Navigate<ContactDetailsViewModel, Contact>(contact);
        }

        public void Test()
        {
            TestCommandExecutedCount++;
        }
    }
}