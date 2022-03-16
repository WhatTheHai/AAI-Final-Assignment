using System.Drawing.Printing;
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

            _world = new GameWorld(w: Screen.Width, h: Screen.Height);
            _timer = new Timer();
            _timer.Elapsed += Timer_Elapsed;
            _timer.Interval = 2000;
            _timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _world.Update(timeDelta);
            Screen.Invalidate();
        }

        private void Screen_Paint(object sender, PaintEventArgs e)
        {
            _world.Render(e.Graphics);
        }
    }
}