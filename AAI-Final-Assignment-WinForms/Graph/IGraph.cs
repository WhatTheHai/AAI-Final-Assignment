using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Graph
{
    public partial interface IGraph
    {
        void AddVertex(string name);
        Vertex GetVertex(string name);

        void AddEdge(string source, string dest, double cost);

        void ClearAll();

        void Unweighted(string name);

        void Dijkstra(string name);
        bool IsConnected();
    }
}
