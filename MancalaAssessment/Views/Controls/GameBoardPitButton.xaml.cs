using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MancalaWPF.Views.Controls
{
    public partial class GameBoardPitButton : Button
    {
        public Color PlayerColor
        {
            get { return (Color)GetValue(PlayerColorProperty); }
            set { SetValue(PlayerColorProperty, value); }
        }

        public static readonly DependencyProperty PlayerColorProperty =
            DependencyProperty.Register(nameof(PlayerColor), typeof(Color), typeof(GameBoardPitButton), new PropertyMetadata(null));

        public int Stones
        {
            get { return (int)GetValue(StonesProperty); }
            set { SetValue(StonesProperty, value); }
        }

        public static readonly DependencyProperty StonesProperty =
            DependencyProperty.Register(nameof(Stones), typeof(int), typeof(GameBoardPitButton), new PropertyMetadata(0));

        public GameBoardPitButton()
        {
            InitializeComponent();
        }
    }
}
