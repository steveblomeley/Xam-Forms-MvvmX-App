using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;

namespace MvvmCrossApp.Core.ViewModels
{
    public class ContactDetailsViewModel : MvxViewModel<Contact>
    {
        public Contact Contact { get; private set; }

        public override void Prepare(Contact contact)
        {
            base.Prepare();

            Contact = contact;
        }
    }
}