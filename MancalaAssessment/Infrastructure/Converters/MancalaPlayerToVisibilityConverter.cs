using MancalaGame;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MancalaWPF.Infrastructure.Converters
{
    public class MancalaPlayerToVisibilityConverter : IValueConverter
    {
        public MancalaPlayer? Player { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return Visibility.Collapsed;
            }

            if (Player is not null)
            {
                return Player == (MancalaPlayer)value ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
