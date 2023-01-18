namespace FlightConnections.Domain.Models
{
    public class Graph
    {
        public string Origin { get; set; }

        public (Graph, double) Parent { get; set; }

        public Queue<Graph> Neigborhood { get; set; } = new Queue<Graph>();

        public Graph(string nome)
        {
            Origin = nome;
        }

        public override int GetHashCode()
        {
            return Origin.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if(obj is null) return false;
            return Origin == ((Graph)obj).Origin;
        }
    }
}
