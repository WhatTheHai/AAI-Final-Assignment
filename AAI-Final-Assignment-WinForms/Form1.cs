using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;
using Timer = System.Timers.Timer;

namespace AAI_Final_Assignment_WinForms
{
    public partial class Form1 : Form
    {
        private GameWorld _world;
        private Timer _timer;

        public const float timeDelta = 0.8f;

        public Form1()
        {
            InitializeComponent();
            _world = new GameWorld(_mainPanel.Width, _mainPanel.Height);
            _timer = new Timer();
            _timer.Elapsed += Timer_Elapsed;
            _timer.Interval = 20;
            _timer.Enabled = true;
            
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _world.Update(timeDelta);
            _mainPanel.Invalidate();
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            _world.Render(e.Graphics);
        }

        private void _mainPanel_MouseClick(object sender, MouseEventArgs e)
        {
            _world.Witch.Pos = new Vector2D(e.X, e.Y);
        }
    }
}