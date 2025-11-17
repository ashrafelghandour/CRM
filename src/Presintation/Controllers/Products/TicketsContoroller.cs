using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Application.DTOs.Tickets;

namespace WebAPI.Controllers.Tickets;

public class TicketsController : ApiControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTickets()
    {
        try
        {
            var userRole = GetUserRole();
            var userId = GetUserId();

            IEnumerable<Ticket> tickets;

            if (userRole == UserRole.Customer.ToString())
            {
                tickets = await _ticketService.GetTicketsByCustomerAsync(userId);
            }
            else if (userRole == UserRole.Employee.ToString() || userRole == UserRole.Salesman.ToString())
            {
                tickets = await _ticketService.GetTicketsByAssigneeAsync(userId);
            }
            else // SuperAdmin
            {
                tickets = await _ticketService.GetAllTicketsAsync();
            }

            var ticketResponses = tickets.Select(ticket => new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.Name,
                AssignedToId = ticket.AssignedToId,
                AssignedToName = ticket.AssignedTo != null ? $"{ticket.AssignedTo.FirstName} {ticket.AssignedTo.LastName}" : null,
                CreatedAt = ticket.CreatedAt,
                Comments = ticket.Comments?.Select(c => new TicketCommentResponse
                {
                    Id = c.Id,
                    Comment = c.Comment,
                    UserName = $"{c.User.FirstName} {c.User.LastName}",
                    CreatedAt = c.CreatedAt
                }).ToList() ?? new List<TicketCommentResponse>()
            });

            return Ok(new { 
                success = true, 
                data = ticketResponses 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve tickets: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(int id)
    {
        try
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return HandleError("Ticket not found", 404);

            // Authorization check
            var userRole = GetUserRole();
            var userId = GetUserId();

            if (userRole == UserRole.Customer.ToString() && ticket.CustomerId != userId)
                return HandleError("Access denied", 403);

            if ((userRole == UserRole.Employee.ToString() || userRole == UserRole.Salesman.ToString()) && 
                ticket.AssignedToId != userId && userRole != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var ticketResponse = new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CustomerId = ticket.CustomerId,
                CustomerName = ticket.Customer.Name,
                AssignedToId = ticket.AssignedToId,
                AssignedToName = ticket.AssignedTo != null ? $"{ticket.AssignedTo.FirstName} {ticket.AssignedTo.LastName}" : null,
                CreatedAt = ticket.CreatedAt,
                Comments = ticket.Comments?.Select(c => new TicketCommentResponse
                {
                    Id = c.Id,
                    Comment = c.Comment,
                    UserName = $"{c.User.FirstName} {c.User.LastName}",
                    CreatedAt = c.CreatedAt
                }).ToList() ?? new List<TicketCommentResponse>()
            };

            return HandleResult(ticketResponse, "Ticket retrieved successfully");
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve ticket: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
    {
        try
        {
            var userId = GetUserId();
            var userRole = GetUserRole();

            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = TicketStatus.Open,
                CustomerId = userRole == UserRole.Customer.ToString() ? userId : request.CustomerId,
                AssignedToId = request.AssignedToId,
                CreatedById = userId
            };

            var createdTicket = await _ticketService.CreateTicketAsync(ticket);

            var ticketResponse = new TicketResponse
            {
                Id = createdTicket.Id,
                Title = createdTicket.Title,
                Description = createdTicket.Description,
                Status = createdTicket.Status,
                Priority = createdTicket.Priority,
                CustomerId = createdTicket.CustomerId,
                CustomerName = createdTicket.Customer.Name,
                AssignedToId = createdTicket.AssignedToId,
                AssignedToName = createdTicket.AssignedTo != null ? $"{createdTicket.AssignedTo.FirstName} {createdTicket.AssignedTo.LastName}" : null,
                CreatedAt = createdTicket.CreatedAt
            };

            return CreatedAtAction(nameof(GetTicketById), new { id = createdTicket.Id }, new { 
                success = true, 
                message = "Ticket created successfully", 
                data = ticketResponse 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to create ticket: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, [FromBody] TicketResponse request)
    {
        try
        {
            var existingTicket = await _ticketService.GetTicketByIdAsync(id);
            if (existingTicket == null)
                return HandleError("Ticket not found", 404);

            // Authorization check
            var userRole = GetUserRole();
            var userId = GetUserId();

            if (userRole == UserRole.Customer.ToString() && existingTicket.CustomerId != userId)
                return HandleError("Access denied", 403);

            existingTicket.Title = request.Title;
            existingTicket.Description = request.Description;
            existingTicket.Status = request.Status;
            existingTicket.Priority = request.Priority;
            existingTicket.AssignedToId = request.AssignedToId;

            var result = await _ticketService.UpdateTicketAsync(existingTicket);
            if (!result)
                return HandleError("Failed to update ticket");

            return Ok(new { 
                success = true, 
                message = "Ticket updated successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to update ticket: {ex.Message}");
        }
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentRequest request)
    {
        try
        {
            var userId = GetUserId();
            var result = await _ticketService.AddCommentAsync(id, request.Comment, userId);
            
            if (!result)
                return HandleError("Ticket not found", 404);

            return Ok(new { 
                success = true, 
                message = "Comment added successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to add comment: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        try
        {
            // Only SuperAdmin can delete tickets
            if (GetUserRole() != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var result = await _ticketService.DeleteTicketAsync(id);
            if (!result)
                return HandleError("Ticket not found", 404);

            return Ok(new { 
                success = true, 
                message = "Ticket deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to delete ticket: {ex.Message}");
        }
    }
}

public class AddCommentRequest
{
    public string Comment { get; set; } = string.Empty;
}