// See https://aka.ms/new-console-template for more information
using Ventas;

var nivel1Handler = new Nivel1TicketHander();
var nivel2Handler = new Nivel2TicketHander();
var nivel3Handler = new Nivel3TicketHander();

nivel1Handler.SetNextHandler(nivel2Handler);
nivel2Handler.SetNextHandler(nivel3Handler);

var ticket1 = new Ticket { Nivel = Nivel.Bajo };
var ticket2 = new Ticket { Nivel = Nivel.Medio };
var ticket3 = new Ticket { Nivel = Nivel.Avanzado };

nivel1Handler.HandlerTicket(ticket1);
nivel2Handler.HandlerTicket(ticket2);
nivel3Handler.HandlerTicket(ticket3);