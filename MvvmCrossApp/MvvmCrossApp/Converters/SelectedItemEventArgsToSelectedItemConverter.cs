using System;
using System.Globalization;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace MvvmCrossApp
{
    public class ItemTappedEventArgsToItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ItemTappedEventArgs eventArgs
                ? eventArgs.Item
                : throw BadValueTypeException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Exception BadValueTypeException()
        {
            return new ArgumentException(
                $"{nameof(ItemTappedEventArgsToItemConverter)}.{nameof(Convert)} - expected \"value\" argument to be of type {nameof(SelectedItemChangedEventArgs)}.");
        }
    }
}