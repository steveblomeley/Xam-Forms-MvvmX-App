using MvvmCrossApp.Core.Models;
using Xamarin.Forms;

namespace MvvmCrossApp.Views
{
	public partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

	    private void ContactSelected(object sender, SelectedItemChangedEventArgs e)
	    {
	        ViewModel.ShowContactDetails(e.SelectedItem as Contact);
	    }
	}
}