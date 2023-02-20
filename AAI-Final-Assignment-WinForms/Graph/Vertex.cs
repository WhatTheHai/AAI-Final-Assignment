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
        public double fScore;
        public double gScore { get; set; }
        public double hScore { get; set; }
        public Vertex prev;
        public bool known;
        public Vector2D pos;
        public Pen p = new Pen(Color.Gray, 1);
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
            fScore = Double.MaxValue;
            gScore = Double.MaxValue;
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
        public double EuclideanDistance(Vertex other)
        {
            return Math.Sqrt(Math.Pow(pos.X - other.pos.X, 2) + Math.Pow(pos.Y - other.pos.Y, 2));
        }

        public int CompareTo(Vertex? other)
        {
            return other == null ? 1 : fScore.CompareTo(other.fScore);
        }
    }
}
