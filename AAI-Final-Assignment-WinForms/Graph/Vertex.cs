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
        public float distance;
        public float fScore;
        public float gScore { get; set; }
        public float hScore { get; set; }
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

        public float GetDistance()
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
            distance = float.MaxValue;
            known = false;
            fScore = float.MaxValue;
            gScore = float.MaxValue;
            hScore = float.MaxValue;
        }

        public override string ToString()
        {
            string s = pos + (distance != float.MaxValue ? $"({this.distance})" : "") + "[";
            foreach (Edge e in adj.OrderBy(x => x.dest.name))
            {
                s += $"{e.dest.name}({e.cost})";
            }
            s += "]";

            return s;
        }
        public float Heuristic(Vertex other)
        {
            return MathF.Sqrt(MathF.Pow(pos.X - other.pos.X, 2) + MathF.Pow(pos.Y - other.pos.Y, 2));
        }


        public int CompareTo(Vertex? other)
        {
            return other == null ? 1 : fScore.CompareTo(other.fScore);
        }
    }
}
