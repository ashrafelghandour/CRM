using Domain.Entities;

namespace Application.Interfaces;

public interface ITicketService
{
    Task<Ticket?> GetTicketByIdAsync(int id);
    Task<IEnumerable<Ticket>> GetAllTicketsAsync();
    Task<IEnumerable<Ticket>> GetTicketsByCustomerAsync(int customerId);
    Task<IEnumerable<Ticket>> GetTicketsByAssigneeAsync(int assigneeId);
    Task<Ticket> CreateTicketAsync(Ticket ticket);
    Task<bool> UpdateTicketAsync(Ticket ticket);
    Task<bool> AddCommentAsync(int ticketId, string comment, int userId);
    Task<bool> DeleteTicketAsync(int id);
    Task<bool> ChangeTicketStatusAsync(int ticketId, Domain.Enums.TicketStatus status);
    Task<bool> AssignTicketAsync(int ticketId, int assigneeId);
}