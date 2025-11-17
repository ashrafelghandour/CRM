using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;

    public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, ICustomerRepository customerRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        return await _ticketRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _ticketRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByCustomerAsync(int customerId)
    {
        // Verify customer exists
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            throw new ArgumentException("Customer not found");

        return await _ticketRepository.GetByCustomerIdAsync(customerId);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByAssigneeAsync(int assigneeId)
    {
        // Verify assignee exists and is an employee/salesman
        var assignee = await _userRepository.GetByIdAsync(assigneeId);
        if (assignee == null || (assignee.Role != UserRole.Employee && assignee.Role != UserRole.Salesman))
            throw new ArgumentException("Assignee not found or not authorized");

        return await _ticketRepository.GetByAssigneeIdAsync(assigneeId);
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        // Validate customer exists
        var customer = await _customerRepository.GetByIdAsync(ticket.CustomerId);
        if (customer == null)
            throw new ArgumentException("Customer not found");

        // Validate assignee if provided
        if (ticket.AssignedToId.HasValue)
        {
            var assignee = await _userRepository.GetByIdAsync(ticket.AssignedToId.Value);
            if (assignee == null || (assignee.Role != UserRole.Employee && assignee.Role != UserRole.Salesman))
                throw new ArgumentException("Assignee not found or not authorized");
        }

        // Validate created by user exists
        var createdByUser = await _userRepository.GetByIdAsync(ticket.CreatedById);
        if (createdByUser == null)
            throw new ArgumentException("Created by user not found");

        return await _ticketRepository.AddAsync(ticket);
    }

    public async Task<bool> UpdateTicketAsync(Ticket ticket)
    {
        var existingTicket = await _ticketRepository.GetByIdAsync(ticket.Id);
        if (existingTicket == null) return false;

        // Validate assignee if provided
        if (ticket.AssignedToId.HasValue && ticket.AssignedToId.Value != existingTicket.AssignedToId)
        {
            var assignee = await _userRepository.GetByIdAsync(ticket.AssignedToId.Value);
            if (assignee == null || (assignee.Role != UserRole.Employee && assignee.Role != UserRole.Salesman))
                throw new ArgumentException("Assignee not found or not authorized");
        }

        existingTicket.Title = ticket.Title;
        existingTicket.Description = ticket.Description;
        existingTicket.Status = ticket.Status;
        existingTicket.Priority = ticket.Priority;
        existingTicket.AssignedToId = ticket.AssignedToId;

        await _ticketRepository.UpdateAsync(existingTicket);
        return true;
    }

    public async Task<bool> AddCommentAsync(int ticketId, string comment, int userId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) return false;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        var ticketComment = new TicketComment
        {
            Comment = comment,
            TicketId = ticketId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _ticketRepository.AddCommentAsync(ticketComment);
        return true;
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null) return false;

       await _ticketRepository.DeleteAsync(ticket);
        return true;
    }

    public async Task<bool> ChangeTicketStatusAsync(int ticketId, TicketStatus status)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) return false;

        ticket.Status = status;
        await _ticketRepository.UpdateAsync(ticket);
        return true;
    }

    public async Task<bool> AssignTicketAsync(int ticketId, int assigneeId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null) return false;

        var assignee = await _userRepository.GetByIdAsync(assigneeId);
        if (assignee == null || (assignee.Role != UserRole.Employee && assignee.Role != UserRole.Salesman))
            return false;

        ticket.AssignedToId = assigneeId;
        await _ticketRepository.UpdateAsync(ticket);
        return true;
    }
}