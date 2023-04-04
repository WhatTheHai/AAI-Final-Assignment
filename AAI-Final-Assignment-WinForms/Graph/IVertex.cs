namespace AAI_Final_Assignment_WinForms.Graph; 

public interface IVertex {
    string GetName(); // Returns the name of the vertex

    LinkedList<Edge> GetAdjacents();

    float GetDistance();

    Vertex GetPrevious();

    bool GetKnown();

    void Reset(); // Resets prev, distance (infinity) and known for a vertex
}