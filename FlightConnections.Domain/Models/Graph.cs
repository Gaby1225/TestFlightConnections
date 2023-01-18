namespace FlightConnections.Domain.Models
{
    public class Graph
    {
        public string Nome { get; set; }

        public (Graph, double) Parent { get; set; }

        public Queue<Graph> Neigborhood { get; set; } = new Queue<Graph>();

        public Graph(string nome)
        {
            Nome = nome;
        }

        public override int GetHashCode()
        {
            return Nome.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if(obj is null) return false;
            return Nome == ((Graph)obj).Nome;
        }
    }
}
