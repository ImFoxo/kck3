namespace Biblioteka.Models
{
    public class PseudoQueue
    {
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public PseudoQueue(int id, int count)
        {
            UserId = id;
            Quantity = count;
        }
    }
}
