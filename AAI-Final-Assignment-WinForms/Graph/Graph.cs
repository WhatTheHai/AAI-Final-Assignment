using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Graph; 

public class Graph : IGraph {
    public const int VectorDistance = 24;
    public static readonly float INFINITY = float.MaxValue;
    public List<Vector2D>? MovePath;
    public bool RenderPath = false;

    public Dictionary<Vector2D, Vertex> VertexMap;

    public Graph(GameWorld gameWorld) {
        VertexMap = new Dictionary<Vector2D, Vertex>();

        // Create initial vertices on the map
        for (var y = 0; y < gameWorld.Height; y += VectorDistance)
        for (var x = 0; x < gameWorld.Width; x += VectorDistance)
            GetVertex(new Vector2D(x, y));

        //Removes the vertices that are on objects
        foreach (StaticEntity entity in gameWorld.StaticEntities) {
            var vector = ClosestVertex(entity.Pos);
            var radius = (int)entity.Radius;
            foreach (var Vertex in VertexMap)
                if (Vertex.Key.Distance(entity.Pos) <= radius)
                    VertexMap.Remove(Vertex.Key);
        }

        //Generate edges
        foreach (var vertex in VertexMap.Values) {
            var edgesList = new List<Vector2D>();

            var pos = vertex.pos;
            //Order goes from up to all the other directions clockwise. E.G. Up, up right, right, etc...
            Vector2D[] directions = {
                new(0, -VectorDistance),
                new(VectorDistance, -VectorDistance),
                new(VectorDistance, 0),
                new(VectorDistance, VectorDistance),
                new(0, VectorDistance),
                new(-VectorDistance, VectorDistance),
                new(-VectorDistance, 0),
                new(-VectorDistance, -VectorDistance)
            };

            foreach (var dir in directions) {
                var newPos = pos.Clone().Add(dir);
                if (VertexMap.ContainsKey(newPos)) {
                    //If either X or Y has a zero, it's a non-diagional so the cost is 1. Otherwise 1.4 (Sqrt (1+1) rounded down)
                    var cost = dir.X == 0 || dir.Y == 0 ? 1f : 1.4f;
                    AddEdge(pos, newPos, cost);
                }
            }
        }
    }

    /// <summary>
    ///     Adds a vertex to the graph. If a vertex with the given name
    ///     already exists, no action is performed.
    /// </summary>
    /// <param name="name">The name of the new vertex</param>
    public void AddVertex(Vector2D pos) {
        CreateOrReturnVertex(pos);
    }

    /// <summary>
    ///     Gets a vertex from the graph by name. If no such vertex exists,
    ///     a new vertex will be created and returned.
    /// </summary>
    /// <param name="name">The name of the vertex</param>
    /// <returns>The vertex withe the given name</returns>
    public Vertex GetVertex(Vector2D pos) {
        return CreateOrReturnVertex(pos);
    }

    /// <summary>
    ///     Creates an edge between two vertices. Vertices that are non existing
    ///     will be created before adding the edge.
    ///     There is no check on multiple edges!
    /// </summary>
    /// <param name="source">The vector of the source vertex</param>
    /// <param name="dest">The vector of the destination vertex</param>
    /// <param name="cost">cost of the edge</param>
    public void AddEdge(Vector2D source, Vector2D dest, float cost = 1) {
        var vSource = CreateOrReturnVertex(source);
        var vDest = CreateOrReturnVertex(dest);

        // Check for duplicates
        if (vSource.adj.Any(edge => edge.dest == vDest)) return;

        vSource.adj.AddFirst(new Edge(vDest, cost));
    }

    /// <summary>
    ///     Clears all info within the vertices. This method will not remove any
    ///     vertices or edges.
    /// </summary>
    public void ClearAll() {
        foreach (var vertex in VertexMap) vertex.Value.Reset();
    }

    /// <summary>
    ///     Performs the Breadth-First algorithm for unweighted graphs.
    /// </summary>
    /// <param name="name">The name of the starting vertex</param>
    public void Unweighted(Vector2D pos) {
        ClearAll();
        Vertex startV;
        if (VertexMap[pos] != null)
            startV = VertexMap[pos];
        else
            throw new SystemException();

        var q = new Queue<Vertex>();
        q.Enqueue(startV);
        startV.distance = 0;

        while (q.Any()) {
            var prev = q.Dequeue();
            //Check every adj
            foreach (var edge in prev.adj) {
                var next = edge.dest;
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
    ///     Performs Dijkstra's algorithm, mainly used for testing the edges.
    /// </summary>
    /// <param name="pos">Position of the entity</param>
    /// <exception cref="SystemException"></exception>
    public void Dijkstra(Vector2D pos) {
        ClearAll();
        Vertex startV;
        if (VertexMap[pos] != null)
            startV = VertexMap[pos];
        else
            throw new SystemException();

        var priorityQ = new PriorityQueue<Vertex>();
        priorityQ.Add(startV);
        startV.distance = 0;

        while (priorityQ.size != 0) {
            var prev = priorityQ.Remove();

            //Only check if it's not known
            if (prev.known == false) {
                prev.known = true;
                foreach (var edge in prev.adj) {
                    var next = edge.dest;
                    var newDistance = prev.distance + edge.cost;
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

    public bool IsConnected() {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Looks for the nearest Vertex within a given position
    /// </summary>
    /// <param name="position">Position of the entity</param>
    /// <returns></returns>
    public Vector2D ClosestVertex(Vector2D position) {
        var nearestX = MathF.Round(position.X / VectorDistance) * VectorDistance;
        var nearestY = MathF.Round(position.Y / VectorDistance) * VectorDistance;
        return new Vector2D(nearestX, nearestY);
    }

    /// <summary>
    ///     Tries to find a given vertex for a position, if it doesn't exist return a new one.
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
    ///     Tries to find a given vertex for a position.
    /// </summary>
    /// <param name="pos">Position of the entity</param>
    /// <returns>A (new) vertex</returns>
    private Vertex FindVertex(Vector2D pos) {
        Vertex nearestVertex = null;
        var nearestDistance = float.MaxValue;

        foreach (var vertex in VertexMap.Values) {
            var distance = vertex.pos.Distance(pos);

            if (distance < nearestDistance) {
                nearestVertex = vertex;
                nearestDistance = distance;
            }
        }

        if (nearestVertex != null && nearestDistance < float.MaxValue) return nearestVertex;
        return new Vertex(pos);
    }

    /// <summary>
    ///     Performs the A* algorithm to find the shortest path between two points in the graph.
    /// </summary>
    /// <param name="start">The starting point of the path.</param>
    /// <param name="goal">The goal point of the path.</param>
    /// <returns>
    ///     A list of vertices that represents the shortest path from the starting point to the goal point, or null if a
    ///     path cannot be found.
    /// </returns>
    public List<Vector2D>? AStar(Vector2D start, Vector2D goal) {
        ClearAll();

        var startVertex = FindVertex(ClosestVertex(start));
        var goalVertex = FindVertex(ClosestVertex(goal));

        if (startVertex == null || goalVertex == null) return null;

        var openSet = new PriorityQueue<Vertex>();
        var closedSet = new HashSet<Vertex>();

        //Adjusted vertex cost = cost to reach a vertex + estimate cost to target
        // F Score = G score + H score
        // G: distance from vertex to start vertex
        // H: distance from vertex to the target vertex
        // Set the G, H, and F scores of the start vertex
        startVertex.gScore = 0;
        startVertex.hScore = startVertex.Heuristic(goalVertex);
        startVertex.fScore = startVertex.gScore + startVertex.hScore;
        openSet.Add(startVertex);

        // Continue until the queue is empty and all vertices are seen.
        while (openSet.size > 0) {
            var currentVertex = openSet.Remove();

            if (currentVertex == goalVertex)
                //Goal found, make the path.
                return BuildPath(startVertex, goalVertex);

            //Before exploring add it to the closedSet
            closedSet.Add(currentVertex);

            //Explore the adjacent vertices
            foreach (var edge in currentVertex.GetAdjacents()) {
                //Edge to Vertex
                var nearVertex = edge.dest;

                //Found in closedSet means already explored
                if (closedSet.Contains(nearVertex)) continue;

                // Calculate the relative G score

                var relativeGScore = currentVertex.gScore + edge.cost;

                //Add to the openSet if not already there
                if (!openSet.Contains(nearVertex)) {
                    nearVertex.hScore = Heuristic(nearVertex.GetPos(), goalVertex.GetPos());
                    nearVertex.gScore = relativeGScore;
                    nearVertex.fScore = nearVertex.hScore + nearVertex.gScore;
                    nearVertex.prev = currentVertex;
                    openSet.Add(nearVertex);
                    //If the relative G score is higher or equal, it is not a better part.
                }
                else if (relativeGScore >= nearVertex.gScore) {
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

    /// <summary>
    /// Takes two arguments, start and goal vertex and returns a built path. (Only works if the path has been found)
    /// </summary>
    /// <param name="startVertex"></param>
    /// <param name="goalVertex"></param>
    /// <returns></returns>
    public List<Vector2D> BuildPath(Vertex startVertex, Vertex goalVertex) {
        var path = new List<Vector2D>();
        var current = goalVertex;

        while (current != startVertex) {
            path.Add(current.pos);
            current = current.GetPrevious();
        }

        path.Add(startVertex.pos);

        //Reverse from goal -> start to start -> goal
        path.Reverse();
        //Removes the first vertex in the list to avoid some weird movements
        path.RemoveAt(0);

        return path;
    }

    /// <summary>
    ///     Calculates a heuristic value based on the Euclidean distance between two 2D vectors.
    /// </summary>
    /// <param name="start">The start vector.</param>
    /// <param name="end">The end vector.</param>
    /// <returns>The Euclidean distance between the two vectors.</returns>
    private static float Heuristic(Vector2D start, Vector2D end) {
        var deltaX = end.X - start.X;
        var deltaY = end.Y - start.Y;
        var distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        return distance;
    }

    /// <summary>
    ///     Converts this instance of Graph to its string representation.
    ///     It will call the ToString method of each Vertex. The output is
    ///     ascending on vertex name.
    /// </summary>
    /// <returns>The string representation of this Graph instance</returns>
    public override string ToString() {
        var s = "";
        foreach (var key in VertexMap.Keys.OrderBy(x => x)) s += VertexMap[key];

        return s;
    }

    public void Render(Graphics g) {
        var p = new Pen(Color.Gray, 1);
        var p1 = p;
        var p2 = new Pen(Color.Red, 1);
        //Prints all edges
        foreach (var vertex in VertexMap.Values)
        foreach (var edge in vertex.adj)
            g.DrawLine(p, (int)vertex.pos.X, (int)vertex.pos.Y, (int)edge.dest.pos.X, (int)edge.dest.pos.Y);
        //Prints all vertices
        foreach (var vertex in VertexMap.Values) {
            g.DrawEllipse(p, new Rectangle((int)vertex.pos.X - 3, (int)vertex.pos.Y - 3, 6, 6));

            if (RenderPath && (vertex.fScore != float.MaxValue || vertex.hScore != float.MaxValue ||
                               vertex.gScore != float.MaxValue))
                //Debug purposes, prints all the scores
                //g.DrawString($"{(int)vertex.fScore}\n{(int)vertex.gScore}\n{(int)vertex.hScore}", new Font("Arial", 5), new SolidBrush(Color.Black), new PointF((int)vertex.pos.X-1, (int)vertex.pos.Y+2));
                g.DrawEllipse(p2, new Rectangle((int)vertex.pos.X - 3, (int)vertex.pos.Y - 3, 6, 6));
        }

        //If there's a movepath
        if (MovePath != null && MovePath.Any())
            g.DrawEllipse(p2, (int)MovePath.Last().X - 2, (int)MovePath.Last().Y - 2, 4, 4);
        if (RenderPath && MovePath != null) {
            //toList to prevent crashing when updating MovePath too fast
            var points = MovePath.ToList();
            for (var i = 0; i < points.Count - 1; i++)
                g.DrawLine(p2, (int)points[i].X, (int)points[i].Y, (int)points[i + 1].X, (int)points[i + 1].Y);
            foreach (var vector in points) g.DrawEllipse(p2, (int)vector.X - 2, (int)vector.Y - 2, 4, 4);
        }
    }
}