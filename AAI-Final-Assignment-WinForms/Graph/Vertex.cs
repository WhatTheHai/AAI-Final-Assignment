using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.Graph; 

public class Vertex : IVertex, IComparable<Vertex> {
    public LinkedList<Edge> adj;
    public float distance;
    public float fScore;
    public bool known;
    public string name;
    public Pen p = new(Color.Gray, 1);
    public Vector2D pos;
    public Vertex prev;

    public Vertex(Vector2D pos) {
        adj = new LinkedList<Edge>();
        this.pos = pos;
        Reset();
    }

    public float gScore { get; set; }
    public float hScore { get; set; }


    public int CompareTo(Vertex? other) {
        return other == null ? 1 : fScore.CompareTo(other.fScore);
    }

    public string GetName() {
        return name;
    }

    public LinkedList<Edge> GetAdjacents() {
        return adj;
    }

    public float GetDistance() {
        return distance;
    }

    public Vertex GetPrevious() {
        return prev;
    }

    public bool GetKnown() {
        return known;
    }

    public void Reset() {
        prev = null;
        distance = float.MaxValue;
        known = false;
        fScore = float.MaxValue;
        gScore = float.MaxValue;
        hScore = float.MaxValue;
    }

    public Vector2D GetPos() {
        return pos;
    }

    public override string ToString() {
        var s = pos + (distance != float.MaxValue ? $"({distance})" : "") + "[";
        foreach (var e in adj.OrderBy(x => x.dest.name)) s += $"{e.dest.name}({e.cost})";
        s += "]";

        return s;
    }

    public float Heuristic(Vertex other) {
        return MathF.Sqrt(MathF.Pow(pos.X - other.pos.X, 2) + MathF.Pow(pos.Y - other.pos.Y, 2));
    }
}