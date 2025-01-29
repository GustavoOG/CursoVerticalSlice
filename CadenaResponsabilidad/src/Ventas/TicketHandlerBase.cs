namespace Ventas
{
    public abstract class TicketHandlerBase : ITicketHandler
    {
        private ITicketHandler? _nextHandler;


        public void HandlerTicket(Ticket ticket)
        {
            if (CanHandlerTicket(ticket))
            {
                Handler(ticket);
            }
            else if (_nextHandler is not null)
            {
                _nextHandler.HandlerTicket(ticket);
            }
            else
            {
                Console.WriteLine("El ticket no pued procesarse en ningun handler");
            }
        }

        public void SetNextHandler(ITicketHandler handler)
        {
            _nextHandler = handler;
        }

        protected abstract bool CanHandlerTicket(Ticket ticket);

        protected abstract void Handler(Ticket ticket);
    }
}
