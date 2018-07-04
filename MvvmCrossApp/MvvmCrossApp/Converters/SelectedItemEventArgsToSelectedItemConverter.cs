using System;
using System.Globalization;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace MvvmCrossApp
{
    public class SelectedItemEventArgsToSelectedItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is ItemTappedEventArgs eventArgs)
                ? eventArgs.Item
                : throw new ArgumentException(
                    $"{nameof(SelectedItemEventArgsToSelectedItemConverter)}.{nameof(Convert)} - expected {nameof(value)} argument to be of type {nameof(SelectedItemChangedEventArgs)}.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}