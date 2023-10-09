using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MancalaWPF.Views.Controls
{
    public partial class GameBoardStonesControl : UserControl
    {
        public int Stones
        {
            get { return (int)GetValue(StonesProperty); }
            set { SetValue(StonesProperty, value); }
        }

        public static readonly DependencyProperty StonesProperty =
            DependencyProperty.Register(nameof(Stones), typeof(int), typeof(GameBoardStonesControl), new PropertyMetadata(0, OnStonesPropertyChanged));

        private readonly Random _random;

        public int Seed { get; private set; } = Guid.NewGuid().GetHashCode();
        public ObservableCollection<VisibleStone> VisibleStones { get; private set; } = new ObservableCollection<VisibleStone>();

        private static void OnStonesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (GameBoardStonesControl)d;
            var visibleNumberOfStones = (int)e.NewValue;

            foreach (var stone in self.VisibleStones)
            {
                stone.Visibility = visibleNumberOfStones > 0 ? Visibility.Visible : Visibility.Hidden;
                visibleNumberOfStones--;
            }
        }

        public GameBoardStonesControl()
        {
            InitializeComponent();
            _random = new Random(Seed);

            VisibleStones = new ObservableCollection<VisibleStone>(
                Enumerable.Range(0, 48).Select(_ => new VisibleStone()
                {
                    Rotation = _random.Next(180),
                    Visibility = Visibility.Hidden
                }));
        }

        public class VisibleStone : ObservableObject
        {
            private Visibility _visibility = Visibility.Hidden;

            public int Rotation { get; init; } = 0;
            public Visibility Visibility
            {
                get => _visibility;
                set => SetProperty(ref _visibility, value);
            }
        }
    }
}
