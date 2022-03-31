using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Graph {
    public partial class Graph : IGraph {
        public static readonly double INFINITY = System.Double.MaxValue;

        public Dictionary<Vector2D, Vertex> vertexMap;
        public const int vectorDistance = 32;

        public Graph(GameWorld gameWorld) {
            vertexMap = new Dictionary<Vector2D, Vertex>();

            // Create initial vertexes on the map
            for(int y = 0; y <= gameWorld.Height; y += vectorDistance) {
                for (int x = 0; x <= gameWorld.Width; x += vectorDistance) {
                    GetVertex(new Vector2D(x, y));
                }
            }

            //TODO: Remove vertexes depending on objects

            foreach (StaticEntity entity in gameWorld.StaticEntities) {
                Vector2D vector = ClosestVertex(entity.Pos);

                int x = (int)vector.X;
                int y = (int)vector.X;

                int eHeight = (int)(entity.TextureHeight);
                int eWidth = (int)(entity.TextureWidth);

                for (int i = x; i < (eWidth + x); i += vectorDistance) 
                {
                    for (int j = y; j < (eHeight + y); j += vectorDistance) {
                        Vector2D nearVector = new Vector2D(i, j);
                        if (vertexMap.ContainsKey(nearVector))
                            vertexMap.Remove(nearVector);
                    }
                }
            }

            //Generate edges
            foreach (Vertex vertex in vertexMap.Values) {
                List<Vector2D> edgesList = new List<Vector2D>();

                Vector2D pos = vertex.pos;
                Vector2D rightUp = new Vector2D(pos.X + vectorDistance, pos.Y - vectorDistance);
                Vector2D right = new Vector2D(pos.X + vectorDistance, pos.Y);
                Vector2D rightDown = new Vector2D(pos.X + vectorDistance, pos.Y + vectorDistance);
                Vector2D down = new Vector2D(pos.X, pos.Y + vectorDistance);

                //Diagonals have sqrt 2 cost but rounded to 1.5
                if(vertexMap.ContainsKey(rightUp))
                    AddEdge(pos, rightUp, 1.5);
                if (vertexMap.ContainsKey(right))
                    AddEdge(pos, right, 1);
                if (vertexMap.ContainsKey(rightDown))
                    AddEdge(pos, rightDown, 1.5);
                if (vertexMap.ContainsKey(down))
                    AddEdge(pos, down, 1);
            } 
        }

        public Vector2D ClosestVertex(Vector2D position) {
            double nearestX = ((int)position.X / vectorDistance) * vectorDistance + vectorDistance;
            double nearestY = ((int)position.Y / vectorDistance) * vectorDistance + vectorDistance;
            return new Vector2D(nearestX, nearestY);
        }

        /// <summary>
        ///    Adds a vertex to the graph. If a vertex with the given name
        ///    already exists, no action is performed.
        /// </summary>
        /// <param name="name">The name of the new vertex</param>
        public void AddVertex(Vector2D pos) {
            this.CreateOrReturnVertex(pos);
        }


        /// <summary>
        ///    Gets a vertex from the graph by name. If no such vertex exists,
        ///    a new vertex will be created and returned.
        /// </summary>
        /// <param name="name">The name of the vertex</param>
        /// <returns>The vertex withe the given name</returns>
        public Vertex GetVertex(Vector2D pos) {
            return CreateOrReturnVertex(pos);
        }

        private Vertex CreateOrReturnVertex(Vector2D pos) {
            Vertex vertex;
            if (vertexMap.ContainsKey(pos))
                return vertexMap[pos];

            vertex = new Vertex(pos);
            vertexMap.Add(pos, vertex);
            return vertex;
        }

        /// <summary>
        ///    Creates an edge between two vertices. Vertices that are non existing
        ///    will be created before adding the edge.
        ///    There is no check on multiple edges!
        /// </summary>
        /// <param name="source">The vector of the source vertex</param>
        /// <param name="dest">The vector of the destination vertex</param>
        /// <param name="cost">cost of the edge</param>
        public void AddEdge(Vector2D source, Vector2D dest, double cost = 1) {
            Vertex vSource = this.CreateOrReturnVertex(source);
            Vertex vDest = this.CreateOrReturnVertex(dest);

            //Check for duplicates
            vSource.adj.AddFirst(new Edge(vDest, cost));
        }


        /// <summary>
        ///    Clears all info within the vertices. This method will not remove any
        ///    vertices or edges.
        /// </summary>
        public void ClearAll() {
            foreach (var vertex in vertexMap) {
                vertex.Value.Reset();
            }
        }

        /// <summary>
        ///    Performs the Breatch-First algorithm for unweighted graphs.
        /// </summary>
        /// <param name="name">The name of the starting vertex</param>
        public void Unweighted(Vector2D pos) {
            ClearAll();
            Vertex startV;
            if (vertexMap[pos] != null) {
                startV = vertexMap[pos];
            }
            else {
                throw new SystemException();
            }

            Queue<Vertex> q = new Queue<Vertex>();
            q.Enqueue(startV);
            startV.distance = 0;

            while (q.Any()) {
                Vertex prev = q.Dequeue();
                //Check every adj
                foreach (var edge in prev.adj) {
                    Vertex next = edge.dest;
                    //If it's not infinity it's checked
                    if (next.distance == INFINITY) {
                        next.distance = prev.distance + 1;
                        q.Enqueue(next);
                        //w.prev = v; Not needed?
                    }
                }
            }
        }
        
        public void Dijkstra(Vector2D pos) {
            ClearAll();
            Vertex startV;
            if (vertexMap[pos] != null) {
                startV = vertexMap[pos];
            }
            else {
                throw new SystemException();
            }

            PriorityQueue<Vertex> priorityQ = new PriorityQueue<Vertex>();
            priorityQ.Add(startV);
            startV.distance = 0;

            while (priorityQ.size != 0) {
                Vertex prev = priorityQ.Remove();

                //Only check if it's not known
                if (prev.known == false) {
                    prev.known = true;
                    foreach (var edge in prev.adj) {
                        Vertex next = edge.dest;
                        double newDistance = prev.distance + edge.cost;
                        //Overwrite if the newer found distance is smaller
                        if (next.distance > newDistance) {
                            next.distance = newDistance;
                            next.prev = prev;
                        }

                        priorityQ.Add(next);
                    }
                }

            }
        }
        /// <summary>
        ///    Converts this instance of Graph to its string representation.
        ///    It will call the ToString method of each Vertex. The output is
        ///    ascending on vertex name.
        /// </summary>
        /// <returns>The string representation of this Graph instance</returns>
        public override string ToString() {
            string s = "";
            foreach (Vector2D key in vertexMap.Keys.OrderBy(x => x)) {
                s += vertexMap[key];
            }

            return s;
        }
        public bool IsConnected() {
            throw new System.NotImplementedException();
        }

        public void Render(Graphics g)
        {
            Pen p = new Pen(Color.Red, 1);
            foreach (Vertex vertex in vertexMap.Values) {
                foreach (Edge edge in vertex.adj) {
                    g.DrawLine(p, (int)vertex.pos.X, (int)vertex.pos.Y, (int)edge.dest.pos.X, (int)edge.dest.pos.Y);
                }
            }
            //Vertex
            foreach (Vertex vertex in vertexMap.Values) 
            {
                g.DrawEllipse(p, new Rectangle((int)vertex.pos.X-3, (int)vertex.pos.Y-3, 6, 6));
            }
        }
    }
}
