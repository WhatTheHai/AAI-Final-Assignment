using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.Graph
{
    public partial class Vertex : IVertex, IComparable<Vertex>
    {
        public string name;
        public LinkedList<Edge> adj;
        public double distance;
        public Vertex prev;
        public bool known;
        public Vector2D pos;
        public Vertex(Vector2D pos)
        {
            this.adj = new LinkedList<Edge>();
            this.pos = pos;
            this.Reset();
        }
        public string GetName()
        {
            return name;
        }
        public Vector2D GetPos()
        {
            return pos;
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
            string s = pos + (distance != Double.MaxValue ? $"({this.distance})" : "") + "[";
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
