using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Graph
{
    public class Edge : IComparable<Edge>
    {
        public Vertex dest;
        public double cost;

        public Edge(Vertex d, double c)
        {
            dest = d;
            cost = c;
        }
        public int CompareTo(Edge? other) {
            if (other == null) {
                return 0;
            }
            return cost.CompareTo(other.cost);
        }
    }
}
