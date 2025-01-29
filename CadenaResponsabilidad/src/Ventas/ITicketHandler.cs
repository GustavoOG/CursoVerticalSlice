namespace Ventas
{
    public interface ITicketHandler
    {
        void HandlerTicket(Ticket ticket);

        void SetNextHandler(ITicketHandler handler);
    }

    public class Ticket()
    {
        public Nivel Nivel { get; set; }
    }

    public enum Nivel
    {
        Bajo,
        Medio,
        Avanzado

    }
}
