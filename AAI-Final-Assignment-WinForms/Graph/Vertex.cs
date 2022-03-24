using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Graph
{
    public partial class Vertex : IVertex, IComparable<Vertex>
    {
        public string name;
        public LinkedList<Edge> adj;
        public double distance;
        public Vertex prev;
        public bool known;
        public Vertex(string name)
        {
            this.name = name;
            this.adj = new LinkedList<Edge>();
            this.Reset();
        }
        public string GetName()
        {
            return name;
        }
        public LinkedList<Edge> GetAdjacents()
        {
            return adj;
        }

        public double GetDistance()
        {
            return distance;
        }

        public Vertex GetPrevious()
        {
            return prev;
        }

        public bool GetKnown()
        {
            return known;
        }

        public void Reset()
        {
            prev = null;
            distance = Double.MaxValue;
            known = false;
        }

        public override string ToString()
        {
            string s = name + (distance != Double.MaxValue ? $"({this.distance})" : "") + "[";
            foreach (Edge e in adj.OrderBy(x => x.dest.name))
            {
                s += $"{e.dest.name}({e.cost})";
            }
            s += "]";

            return s;
        }
        public int CompareTo(Vertex other)
        {
            return distance.CompareTo(other.distance);
        }
    }
}
