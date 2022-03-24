using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Graph
{
    public partial interface IVertex
    { 
        string GetName();  // Returns the name of the vertex

        LinkedList<Edge> GetAdjacents();

        double GetDistance();

        Vertex GetPrevious();

        bool GetKnown();

        void Reset();  // Resets prev, distance (infinity) and known for a vertex
    }
}
