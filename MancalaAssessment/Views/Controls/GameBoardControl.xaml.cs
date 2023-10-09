using MancalaWPF.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MancalaWPF.Views.Controls
{
    public partial class GameBoardControl : UserControl
    {
        public GameBoardModel GameBoard
        {
            get { return (GameBoardModel)GetValue(GameBoardProperty); }
            set { SetValue(GameBoardProperty, value); }
        }

        public static readonly DependencyProperty GameBoardProperty =
            DependencyProperty.Register(nameof(GameBoard), typeof(GameBoardModel), typeof(GameBoardControl), new PropertyMetadata(null));

        public ICommand PitCommand
        {
            get { return (ICommand)GetValue(PitCommandProperty); }
            set { SetValue(PitCommandProperty, value); }
        }

        public static readonly DependencyProperty PitCommandProperty =
            DependencyProperty.Register(nameof(PitCommand), typeof(ICommand), typeof(GameBoardControl), new PropertyMetadata(null));

        public GameBoardControl()
        {
            InitializeComponent();
        }
    }
}
