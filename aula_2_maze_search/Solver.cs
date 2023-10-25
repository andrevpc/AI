using System.Collections.Generic;
using Stately;

public class Solver
{
    public Maze Maze { get; set; }

    public void Solve()
    {
        // DepthFirstSearch();

        DepthFirstRecursiveSearchWithPath(Maze.Root);

        // BreadthFirstSearch();

        // List<Space> spaces = new()
        // {
        //     Maze.Spaces[0]
        // };
        // BreadthFirstRecursiveSearch(spaces);

        // BreadthFirstSearchWithPath();
    }

    #region DepthFirstSearch

    public void DepthFirstSearch()
    {
        Stack<Space> spaces = new();

        spaces.Push(Maze.Root);

        while (spaces.Count > 0)
        {
            var crr = spaces.Pop();

            crr.Visited = true;

            if (VerifySpace(crr.Top))
            {
                spaces.Push(crr.Top);
                if (crr.Top.Exit)
                    return;
            }
            if (VerifySpace(crr.Left))
            {
                spaces.Push(crr.Left);
                if (crr.Left.Exit)
                    return;
            }
            if (VerifySpace(crr.Right))
            {
                spaces.Push(crr.Right);
                if (crr.Right.Exit)
                    return;
            }
            if (VerifySpace(crr.Bottom))
            {
                spaces.Push(crr.Bottom);
                if (crr.Bottom.Exit)
                    return;
            }

        }
    }

    public bool DepthFirstRecursiveSearchWithPath(Space crr)
    {
        crr.Visited = true;

        if (crr.Exit)
        {
            crr.IsSolution = true;
            return true;
        }

        if (VerifySpace(crr.Right))
        {
            crr.IsSolution = DepthFirstRecursiveSearchWithPath(crr.Right);
            if (crr.IsSolution)
                return true;
        }
        if (VerifySpace(crr.Bottom))
        {
            crr.IsSolution = DepthFirstRecursiveSearchWithPath(crr.Bottom);
            if (crr.IsSolution)
                return true;
        }
        if (VerifySpace(crr.Top))
        {
            crr.IsSolution = DepthFirstRecursiveSearchWithPath(crr.Top);
            if (crr.IsSolution)
                return true;
        }
        if (VerifySpace(crr.Left))
        {
            crr.IsSolution = DepthFirstRecursiveSearchWithPath(crr.Left);
            if (crr.IsSolution)
                return true;
        }

        return false;
    }

    #endregion

    #region BreadthFirstSearch

    public void BreadthFirstSearch()
    {
        Queue<Space> spaces = new();

        spaces.Enqueue(Maze.Root);

        while (spaces.Count > 0)
        {
            var crr = spaces.Dequeue();

            crr.Visited = true;

            if (VerifySpace(crr.Top))
            {
                spaces.Enqueue(crr.Top);
                if (crr.Top.Exit)
                    return;
            }
            if (VerifySpace(crr.Left))
            {
                spaces.Enqueue(crr.Left);
                if (crr.Left.Exit)
                    return;
            }
            if (VerifySpace(crr.Right))
            {
                spaces.Enqueue(crr.Right);
                if (crr.Right.Exit)
                    return;
            }
            if (VerifySpace(crr.Bottom))
            {
                spaces.Enqueue(crr.Bottom);
                if (crr.Bottom.Exit)
                    return;
            }

        }
    }

    public void BreadthFirstSearchWithPath()
    {
        var queue = new Queue<List<Space>>();
        List<Space> path = new()
        {
            Maze.Root
        };
        queue.Enqueue(path);

        while (queue.Count > 0)
        {
            path = queue.Dequeue();
            var last = path[^1];

            if (last.Exit)
            {
                foreach (var item in path)
                    item.IsSolution = true;
                break;
            }

            last.Visited = true;

            var options = new List<Space>();

            if (last.Top != null && !last.Top.Visited)
                options.Add(last.Top);
            if (last.Bottom != null && !last.Bottom.Visited)
                options.Add(last.Bottom);
            if (last.Left != null && !last.Left.Visited)
                options.Add(last.Left);
            if (last.Right != null && !last.Right.Visited)
                options.Add(last.Right);

            foreach (var child in options)
            {
                List<Space> newPath = new(path)
                {
                    child
                };
                queue.Enqueue(newPath);
            }
        }
    }

    public Space BreadthFirstRecursiveSearch(List<Space> spaces)
    {
        foreach (var space in spaces)
        {
            if (space.Exit)
            {
                space.IsSolution = true;
                return space;
            }

            space.Visited = true;
        }

        List<Space> newSpaces = new() { };

        foreach (var space in spaces)
        {
            if (VerifySpace(space.Bottom))
                newSpaces.Add(space.Bottom);
            if (VerifySpace(space.Left))
                newSpaces.Add(space.Left);
            if (VerifySpace(space.Right))
                newSpaces.Add(space.Right);
            if (VerifySpace(space.Top))
                newSpaces.Add(space.Top);
        }

        if (newSpaces.Count > 0)
        {
            Space newSpace = BreadthFirstRecursiveSearch(newSpaces);
            return newSpace;
        }

        return null;
    }
    
    #endregion

    #region Dijkstra

    public void Dijkstra()
    {
        
    }

    #endregion

    private bool VerifySpace(Space space)
    {
        if (space is not null && !space.Visited)
            return true;
        return false;
    }
}