using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace MvvmCrossApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactDetailsPage : MvxContentPage
	{
		public ContactDetailsPage ()
		{
			InitializeComponent ();
		}
	}
}