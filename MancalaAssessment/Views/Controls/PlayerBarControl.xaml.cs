using MancalaWPF.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MancalaWPF.Views.Controls
{
    public partial class PlayerBarControl : UserControl
    {
        public Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockProperty, value); }
        }

        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register(nameof(Dock), typeof(Dock), typeof(PlayerBarControl), new PropertyMetadata(Dock.Left));

        public PlayerModel Player
        {
            get { return (PlayerModel)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register(nameof(Player), typeof(PlayerModel), typeof(PlayerBarControl), new PropertyMetadata(null));

        public Color PlayerColor
        {
            get { return (Color)GetValue(PlayerColorProperty); }
            set { SetValue(PlayerColorProperty, value); }
        }

        public static readonly DependencyProperty PlayerColorProperty =
            DependencyProperty.Register(nameof(PlayerColor), typeof(Color), typeof(PlayerBarControl), new PropertyMetadata(null));

        public PlayerBarControl()
        {
            InitializeComponent();
        }
    }
}
