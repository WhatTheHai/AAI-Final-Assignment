using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.Graph; 

public interface IGraph {
    void AddVertex(Vector2D pos);
    Vertex GetVertex(Vector2D pos);

    void AddEdge(Vector2D source, Vector2D dest, float cost);

    void ClearAll();

    void Unweighted(Vector2D pos);

    void Dijkstra(Vector2D pos);
    bool IsConnected();
}