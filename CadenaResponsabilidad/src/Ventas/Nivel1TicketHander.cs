namespace Ventas
{
    public class Nivel1TicketHander : TicketHandlerBase
    {
        protected override bool CanHandlerTicket(Ticket ticket)
        {
            return ticket.Nivel == Nivel.Bajo;
        }

        protected override void Handler(Ticket ticket)
        {
            Console.WriteLine("EL ticket se esta ejcutando en el handler nivel 1");
        }
    }
}
