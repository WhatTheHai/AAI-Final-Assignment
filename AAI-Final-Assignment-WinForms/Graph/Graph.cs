using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Graph {
    public partial class Graph : IGraph {
        public static readonly double INFINITY = System.Double.MaxValue;

        public Dictionary<Vector2D, Vertex> VertexMap;
        public const int VectorDistance = 25;
        public List<Vector2D>? MovePath;
        public Boolean RenderPath = false;

        public Graph(GameWorld gameWorld) {
            VertexMap = new Dictionary<Vector2D, Vertex>();

            // Create initial vertices on the map
            for(int y = 0; y < gameWorld.Height; y += VectorDistance) {
                for (int x = 0; x < gameWorld.Width; x += VectorDistance) {
                    GetVertex(new Vector2D(x, y));
                }
            }

            //Removes the vertices that are on objects
            foreach (StaticEntity entity in gameWorld.StaticEntities) {
                Vector2D vector = ClosestVertex(entity.Pos);

                int x = (int)vector.X;
                int y = (int)vector.Y;

                int eHeight = (int)(entity.TextureHeight);
                int eWidth = (int)(entity.TextureWidth);

                int bufferDistance = 8;

                for (int i = x; i < (eWidth + x + bufferDistance); i += VectorDistance) {
                    for (int j = y; j < (eHeight + y + bufferDistance); j += VectorDistance) {
                        Vector2D nearVector = new Vector2D(i, j);
                        if (VertexMap.ContainsKey(nearVector))
                            VertexMap.Remove(nearVector);
                    }
                }
            }

            //Generate edges
            foreach (Vertex vertex in VertexMap.Values) {
                List<Vector2D> edgesList = new List<Vector2D>();

                Vector2D pos = vertex.pos;
                //Order goes from up to all the other directions clockwise. E.G. Up, up right, right, etc...
                Vector2D[] directions = new Vector2D[] {
                    new Vector2D(0, -VectorDistance),
                    new Vector2D(VectorDistance, -VectorDistance),
                    new Vector2D(VectorDistance, 0),
                    new Vector2D(VectorDistance, VectorDistance),
                    new Vector2D(0, VectorDistance),
                    new Vector2D(-VectorDistance, VectorDistance),
                    new Vector2D(-VectorDistance, 0),
                    new Vector2D(-VectorDistance, -VectorDistance)
                };

                foreach (Vector2D dir in directions) {
                    Vector2D newPos = pos.Clone().Add(dir);
                    if (VertexMap.ContainsKey(newPos)) {
                        //If either X or Y has a zero, it's a non-diagional so the cost is 1. Otherwise 1.4 (Sqrt (1+1) rounded down)
                        double cost = (dir.X == 0 || dir.Y == 0) ? 1: 1.4;
                        AddEdge(pos, newPos, cost);
                    }
                }
            }
        }

        /// <summary>
        /// Looks for the nearest Vertex within a given position
        /// </summary>
        /// <param name="position">Position of the entity</param>
        /// <returns></returns>
        public Vector2D ClosestVertex(Vector2D position)
        {
            double nearestX = Math.Round(position.X / VectorDistance) * VectorDistance;
            double nearestY = Math.Round(position.Y / VectorDistance) * VectorDistance;
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

        /// <summary>
        /// Tries to find a given vertex for a position, if it doesn't exist return a new one.
        /// </summary>
        /// <param name="pos">Position of the entity</param>
        /// <returns>A (new) vertex</returns>
        private Vertex CreateOrReturnVertex(Vector2D pos) {
            Vertex vertex;
            if (VertexMap.ContainsKey(pos))
                return VertexMap[pos];

            vertex = new Vertex(pos);
            VertexMap.Add(pos, vertex);
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

            // Check for duplicates
            if (vSource.adj.Any(edge => edge.dest == vDest)) {
                return;
            }

            vSource.adj.AddFirst(new Edge(vDest, cost));
        }


        /// <summary>
        ///    Clears all info within the vertices. This method will not remove any
        ///    vertices or edges.
        /// </summary>
        public void ClearAll() {
            foreach (var vertex in VertexMap) {
                vertex.Value.Reset();
            }
        }

        /// <summary>
        ///    Performs the Breadth-First algorithm for unweighted graphs.
        /// </summary>
        /// <param name="name">The name of the starting vertex</param>
        public void Unweighted(Vector2D pos) {
            ClearAll();
            Vertex startV;
            if (VertexMap[pos] != null) {
                startV = VertexMap[pos];
            }
            else {
                throw new SystemException();
            }

            Queue<Vertex> q = new Queue<Vertex>();
            q.Enqueue(startV);
            startV.distance = 0;

            while (q.Any()) {
                Vertex prev = q.Dequeue();
                //Check every adjadj
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
        
        /// <summary>
        /// Performs Dijkstra's algorithm, mainly used for testing the edges.
        /// </summary>
        /// <param name="pos">Position of the entity</param>
        /// <exception cref="SystemException"></exception>
        public void Dijkstra(Vector2D pos) {
            ClearAll();
            Vertex startV;
            if (VertexMap[pos] != null) {
                startV = VertexMap[pos];
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
        /// Performs the A* algorithm to find the shortest path between two points in the graph.
        /// </summary>
        /// <param name="start">The starting point of the path.</param>
        /// <param name="goal">The goal point of the path.</param>
        /// <returns>A list of vertices that represents the shortest path from the starting point to the goal point, or null if a path cannot be found.</returns>
        public List<Vector2D>? AStar(Vector2D start, Vector2D goal)
        {
            ClearAll();

            Vertex startVertex = CreateOrReturnVertex(ClosestVertex(start));
            Vertex goalVertex = CreateOrReturnVertex(ClosestVertex(goal));

            //openSet = vertices that are awaiting to be explored
            //closedSet = already explored vertices
            PriorityQueue<Vertex> openSet = new PriorityQueue<Vertex>();
            HashSet<Vertex> closedSet = new HashSet<Vertex>();

            //Adjusted node cost = cost to reach a node + estimate cost to target
            // F Score = G score + H score
            // G: distance from node to start node
            // H: distance from nod to the target node
            // Set the G, H, and F scores of the start vertex
            startVertex.gScore = 0;
            startVertex.hScore = startVertex.Heuristic(goalVertex);
            startVertex.fScore = startVertex.gScore + startVertex.hScore;
            openSet.Add(startVertex);

            // Continue until the queue is empty and all vertices are seen.
            while (openSet.size > 0)
            {
                Vertex currentVertex = openSet.Remove();

                if (currentVertex == goalVertex)
                {
                    //Goal found, make the path.
                    return BuildPath(startVertex, goalVertex);
                }

                //Before exploring add it to the closedSet
                closedSet.Add(currentVertex);

                //Explore the adjacent vertices
                foreach (Edge edge in currentVertex.GetAdjacents())
                {
                    //Edge to Vertex
                    Vertex nearVertex = edge.dest;

                    //Found in closedSet means already explored
                    if (closedSet.Contains(nearVertex)) 
                    {
                        continue;
                    }

                    // Calculate the relative G score

                    double relativeGScore = currentVertex.gScore + edge.cost;

                    //Add to the openSet if not already there
                    if (!openSet.Contains(nearVertex)) {
                        nearVertex.hScore = Heuristic(nearVertex.GetPos(), goalVertex.GetPos());
                        nearVertex.gScore = relativeGScore;
                        nearVertex.fScore = nearVertex.hScore + nearVertex.gScore;
                        nearVertex.prev = currentVertex;
                        openSet.Add(nearVertex);
                    //If the relative G score is higher or equal, it is not a better part.
                    } else if (relativeGScore >= nearVertex.gScore) {
                        //Do nothing
                    }
                    else {
                        nearVertex.gScore = relativeGScore;
                        nearVertex.fScore = nearVertex.gScore + nearVertex.hScore;
                        nearVertex.prev = currentVertex;
                        openSet.UpdatePriority(nearVertex);
                    }
                }
            }
            //If it doesn't find the goal, return nothing.
            return null;
        }

        public List<Vector2D> BuildPath(Vertex startVertex, Vertex goalVertex) {
            List<Vector2D> path = new List<Vector2D>();
            Vertex current = goalVertex;

            while (current != startVertex) {
                path.Add(current.pos);
                current = current.GetPrevious();
            }

            path.Add(startVertex.pos);

            //Reverse from goal -> start to start -> goal
            path.Reverse();

            return path;
        }

        /// <summary>
        /// Calculates a heuristic value based on the Euclidean distance between two 2D vectors.
        /// </summary>
        /// <param name="start">The start vector.</param>
        /// <param name="end">The end vector.</param>
        /// <returns>The Euclidean distance between the two vectors.</returns>
        private static double Heuristic(Vector2D start, Vector2D end)
        {
            double deltaX = end.X - start.X;
            double deltaY = end.Y - start.Y;
            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            return distance;
        }

        /// <summary>
        ///    Converts this instance of Graph to its string representation.
        ///    It will call the ToString method of each Vertex. The output is
        ///    ascending on vertex name.
        /// </summary>
        /// <returns>The string representation of this Graph instance</returns>
        public override string ToString() {
            string s = "";
            foreach (Vector2D key in VertexMap.Keys.OrderBy(x => x)) {
                s += VertexMap[key];
            }

            return s;
        }

        public bool IsConnected() {
            throw new System.NotImplementedException();
        }

        public void Render(Graphics g)
        {
            Pen p = new Pen(Color.Gray, 1);
            Pen p1 = p;
            Pen p2 = new Pen(Color.Red, 1);
            //Prints all edges
            foreach (Vertex vertex in VertexMap.Values) {
                foreach (Edge edge in vertex.adj) {
                    g.DrawLine(p, (int)vertex.pos.X, (int)vertex.pos.Y, (int)edge.dest.pos.X, (int)edge.dest.pos.Y);
                }
            }
            //Prints all vertices
            foreach (Vertex vertex in VertexMap.Values) 
            {
                g.DrawEllipse(vertex.p, new Rectangle((int)vertex.pos.X-3, (int)vertex.pos.Y-3, 6, 6));
                if (RenderPath && vertex.fScore != Double.MaxValue) {
                    g.DrawString($"{(int)vertex.fScore}\n{(int)vertex.gScore}\n{(int)vertex.hScore}", new Font("Arial", 5), new SolidBrush(Color.Black), new PointF((int)vertex.pos.X-1, (int)vertex.pos.Y+2));
                }
            }

            //If there's a movepath
            if (RenderPath && MovePath != null) {
                //toList to prevent crashing when updating MovePath too fast
                var points = MovePath.ToList();
                for (int i = 0; i < points.Count - 1; i++) {
                    g.DrawLine(p2, (int)points[i].X, (int)points[i].Y, (int)points[i+1].X, (int)points[i+1].Y);
                }
                foreach (var vector in points) {
                    g.DrawEllipse(p2, (int)vector.X-2, (int)vector.Y-2, 4,4);
                }
            }
        }
    }
}
