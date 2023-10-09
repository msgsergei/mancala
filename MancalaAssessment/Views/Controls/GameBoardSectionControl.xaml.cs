using System.Windows;
using System.Windows.Controls;

namespace MancalaWPF.Views.Controls
{
    /// <summary>
    /// Interaction logic for GamePitControl.xaml
    /// </summary>
    public partial class GameBoardSectionControl : ContentControl
    {
        public Dock Dock
        {
            get { return (Dock)GetValue(DockProperty); }
            set { SetValue(DockProperty, value); }
        }

        public static readonly DependencyProperty DockProperty =
            DependencyProperty.Register(nameof(Dock), typeof(Dock), typeof(GameBoardSectionControl), new PropertyMetadata(Dock.Top));

        public bool IsLargeStonesCounter
        {
            get { return (bool)GetValue(IsLargeStonesCounterProperty); }
            set { SetValue(IsLargeStonesCounterProperty, value); }
        }

        public static readonly DependencyProperty IsLargeStonesCounterProperty =
            DependencyProperty.Register(nameof(IsLargeStonesCounter), typeof(bool), typeof(GameBoardSectionControl), new PropertyMetadata(false));

        public int Stones
        {
            get { return (int)GetValue(StonesProperty); }
            set { SetValue(StonesProperty, value); }
        }

        public static readonly DependencyProperty StonesProperty =
            DependencyProperty.Register(nameof(Stones), typeof(int), typeof(GameBoardSectionControl), new PropertyMetadata(0));

        public GameBoardSectionControl()
        {
            InitializeComponent();
        }
    }
}
