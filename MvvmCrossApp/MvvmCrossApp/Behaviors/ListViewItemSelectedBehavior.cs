using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MvvmCrossApp.Behaviors
{
    public class ListViewItemSelectedBehavior : Behavior<ListView>
    {
        public static readonly  BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ListViewItemSelectedBehavior));
        public static readonly BindableProperty InputConverterProperty = 
            BindableProperty.Create("Converter", typeof(IValueConverter), typeof(ListViewItemSelectedBehavior));

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter) GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }
        public ListView AssociatedObject { get; private set; }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;
            bindable.ItemSelected += OnListViewItemSelected;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemSelected -= OnListViewItemSelected;
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        private void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Command == null)
            {
                return;
            }

            var parameter = Converter.Convert(e, typeof(object), null, null);
            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}