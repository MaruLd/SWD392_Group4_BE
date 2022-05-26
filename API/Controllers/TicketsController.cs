using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Tickets;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class TicketsController : BaseApiController
    {

        
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTickets(int id)
        {
            var Tickets = await Mediator.Send(new List.Query{Id = id});
            
            if (Tickets==null) return NotFound();

            return Tickets;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var Ticket = await Mediator.Send(new Details.Query{Id = id});

            if (Ticket==null) return NotFound();

            return Ticket;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket(Ticket Ticket)
        {
            return Ok(await Mediator.Send(new Create.Command {Ticket = Ticket}));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditTicket(int id, Ticket Ticket)
        {
            Ticket.Id = id;
            return Ok(await Mediator.Send(new Edit.Command {Ticket = Ticket}));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            return Ok(await Mediator.Send(new Delete.Command {Id = id}));
        }
    }
}