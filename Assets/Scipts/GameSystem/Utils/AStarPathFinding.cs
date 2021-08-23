

using System.Collections.Generic;
using Utils;

public class AStarPathFinding<TPosition>
{
    public delegate List<TPosition> NeighbourStrategy(TPosition position);
    public delegate float DistanceStrategy(TPosition from, TPosition toNeighbour);
    public delegate float HeuristicStrategy(TPosition from, TPosition to);

    private NeighbourStrategy _neighbours;
    private DistanceStrategy _distance;
    private HeuristicStrategy _heuristic;

    public AStarPathFinding(NeighbourStrategy neighbour, DistanceStrategy distance, HeuristicStrategy heuristic)
    {
        _neighbours = neighbour;
        _distance = distance;
        _heuristic = heuristic;
    }

    public List<TPosition> Path(TPosition from, TPosition to)
    {
        var openSet = new List<TPosition>() { from };

        var cameFrom = new Dictionary<TPosition, TPosition>();

        var gScores = new Dictionary<TPosition, float>() { { from, 0f } };

        var fScore = new Dictionary<TPosition, float> { { from, _heuristic(from, to) } };

        while (openSet.Count > 0)
        {
            TPosition current = FindLowestFScore(fScore, openSet);

            if (current.Equals(to))
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            var neighbours = _neighbours(current);

            foreach (var neighbour in neighbours)
            {
                var tentaticegScore = gScores[current] + _distance(current, neighbour);
                if (tentaticegScore < gScores.GetValueOrDefault(neighbour, float.PositiveInfinity))
                {
                    cameFrom[neighbour] = current;
                    gScores[neighbour] = tentaticegScore;
                    fScore[neighbour] = gScores[neighbour] + _heuristic(neighbour, to);
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<TPosition>(0);
    }

    private List<TPosition> ReconstructPath(Dictionary<TPosition, TPosition> cameFrom, TPosition current)
    {
        var path = new List<TPosition>() { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }

    private TPosition FindLowestFScore(Dictionary<TPosition, float> fScore, List<TPosition> openSet)
    {
        TPosition currentNode = openSet[0];

        foreach (var node in openSet)
        {
            var currnetfScore = fScore.GetValueOrDefault(currentNode, float.PositiveInfinity);
            var newfScore = fScore.GetValueOrDefault(node, float.PositiveInfinity);

            if (newfScore < currnetfScore)
            {
                currentNode = node;
            }
        }

        return currentNode;
    }
}
