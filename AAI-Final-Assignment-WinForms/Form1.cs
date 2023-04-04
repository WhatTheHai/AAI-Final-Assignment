using System.Timers;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;
using Timer = System.Timers.Timer;

namespace AAI_Final_Assignment_WinForms; 

public partial class Form1 : Form {
    public const float timeDelta = 0.8f;
    private readonly Timer _timer;
    private readonly GameWorld _world;

    public Form1() {
        InitializeComponent();
        _world = new GameWorld(_mainPanel.Width, _mainPanel.Height);
        _timer = new Timer();
        _timer.Elapsed += Timer_Elapsed;
        _timer.Interval = 20;
        _timer.Enabled = true;
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
        _world.Update(timeDelta);
        _mainPanel.Invalidate();
        //label1.Invalidate();
    }

    private void MainPanel_Paint(object sender, PaintEventArgs e) {
        _world.Render(e.Graphics);
    }

    private void _mainPanel_MouseClick(object sender, MouseEventArgs e) {
        _world.Witch.SetDestination(new Vector2D(e.X, e.Y));
    }

    private void label1_Paint(object sender, PaintEventArgs e) {
        //label1.Text = _world.MovingEntities[0].SteeringBehaviour.DistanceAhead.ToString();
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e) {
        switch (e.KeyData) {
            case Keys.G:
                _world.GraphEnabled = !_world.GraphEnabled;
                break;
            case Keys.H:
                _world.GameGraph.RenderPath = !_world.GameGraph.RenderPath;
                break;
            case Keys.F:
                _world.ShowForces = !_world.ShowForces;
                break;
            case Keys.T:
                _world.ShowGoals = !_world.ShowGoals;
                break;
        }
    }
}