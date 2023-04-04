namespace AAI_Final_Assignment_WinForms.Graph; 

public class Edge : IComparable<Edge> {
    public float cost;
    public Vertex dest;

    public Edge(Vertex d, float c) {
        dest = d;
        cost = c;
    }

    public int CompareTo(Edge? other) {
        if (other == null) return 0;
        return cost.CompareTo(other.cost);
    }
}