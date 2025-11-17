using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Application.DTOs.Schedules;

namespace WebAPI.Controllers.Schedules;

public class SchedulesController : ApiControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSchedules()
    {
        try
        {
            var userRole = GetUserRole();
            var userId = GetUserId();

            IEnumerable<Schedule> schedules;

            if (userRole == UserRole.Customer.ToString())
            {
                schedules = await _scheduleService.GetSchedulesByCustomerAsync(userId);
            }
            else if (userRole == UserRole.Employee.ToString() || userRole == UserRole.Salesman.ToString())
            {
                schedules = await _scheduleService.GetSchedulesByUserAsync(userId);
            }
            else // SuperAdmin
            {
                schedules = await _scheduleService.GetAllSchedulesAsync();
            }

            var scheduleResponses = schedules.Select(schedule => new ScheduleResponse
            {
                Id = schedule.Id,
                Title = schedule.Title,
                Description = schedule.Description,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Location = schedule.Location,
                CreatedById = schedule.CreatedById,
                CreatedByName = $"{schedule.CreatedBy.FirstName} {schedule.CreatedBy.LastName}",
                AssignedToId = schedule.AssignedToId,
                AssignedToName = schedule.AssignedTo != null ? $"{schedule.AssignedTo.FirstName} {schedule.AssignedTo.LastName}" : null,
                CustomerId = schedule.CustomerId,
                CustomerName = schedule.Customer?.Name,
                CreatedAt = schedule.CreatedAt
            });

            return Ok(new { 
                success = true, 
                data = scheduleResponses 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve schedules: {ex.Message}");
        }
    }

       
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetScheduleById(int id)
    {
        try
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
                return HandleError("Schedule not found", 404);

            // Authorization check
            var userRole = GetUserRole();
            var userId = GetUserId();

            if (userRole == UserRole.Customer.ToString() && schedule.CustomerId != userId)
                return HandleError("Access denied", 403);

            if ((userRole == UserRole.Employee.ToString() || userRole == UserRole.Salesman.ToString()) && 
                schedule.AssignedToId != userId && schedule.CreatedById != userId && userRole != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var scheduleResponse = new ScheduleResponse
            {
                Id = schedule.Id,
                Title = schedule.Title,
                Description = schedule.Description,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Location = schedule.Location,
                CreatedById = schedule.CreatedById,
                CreatedByName = $"{schedule.CreatedBy.FirstName} {schedule.CreatedBy.LastName}",
                AssignedToId = schedule.AssignedToId,
                AssignedToName = schedule.AssignedTo != null ? $"{schedule.AssignedTo.FirstName} {schedule.AssignedTo.LastName}" : null,
                CustomerId = schedule.CustomerId,
                CustomerName = schedule.Customer?.Name,
                CreatedAt = schedule.CreatedAt
            };

            return HandleResult(scheduleResponse, "Schedule retrieved successfully");
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to retrieve schedule: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleRequest request)
    {
        try
        {
            var userId = GetUserId();
            var userRole = GetUserRole();

            // Only employees, salesmen, and super admin can create schedules
            if (userRole == UserRole.Customer.ToString())
                return HandleError("Access denied", 403);

            var schedule = new Schedule
            {
                Title = request.Title,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Location = request.Location,
                CreatedById = userId,
                AssignedToId = request.AssignedToId,
                CustomerId = request.CustomerId
            };

            var createdSchedule = await _scheduleService.CreateScheduleAsync(schedule);

            var scheduleResponse = new ScheduleResponse
            {
                Id = createdSchedule.Id,
                Title = createdSchedule.Title,
                Description = createdSchedule.Description,
                StartTime = createdSchedule.StartTime,
                EndTime = createdSchedule.EndTime,
                Location = createdSchedule.Location,
                CreatedById = createdSchedule.CreatedById,
                CreatedByName = $"{createdSchedule.CreatedBy.FirstName} {createdSchedule.CreatedBy.LastName}",
                AssignedToId = createdSchedule.AssignedToId,
                AssignedToName = createdSchedule.AssignedTo != null ? $"{createdSchedule.AssignedTo.FirstName} {createdSchedule.AssignedTo.LastName}" : null,
                CustomerId = createdSchedule.CustomerId,
                CustomerName = createdSchedule.Customer?.Name,
                CreatedAt = createdSchedule.CreatedAt
            };

            return CreatedAtAction(nameof(GetScheduleById), new { id = createdSchedule.Id }, new { 
                success = true, 
                message = "Schedule created successfully", 
                data = scheduleResponse 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to create schedule: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, [FromBody] ScheduleResponse request)
    {
        try
        {
            var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
                return HandleError("Schedule not found", 404);

            // Authorization check - only creator, assigned person, or super admin can update
            var userId = GetUserId();
            var userRole = GetUserRole();

            if (existingSchedule.CreatedById != userId && 
                existingSchedule.AssignedToId != userId && 
                userRole != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            existingSchedule.Title = request.Title;
            existingSchedule.Description = request.Description;
            existingSchedule.StartTime = request.StartTime;
            existingSchedule.EndTime = request.EndTime;
            existingSchedule.Location = request.Location;
            existingSchedule.AssignedToId = request.AssignedToId;
            existingSchedule.CustomerId = request.CustomerId;

            var result = await _scheduleService.UpdateScheduleAsync(existingSchedule);
            if (!result)
                return HandleError("Failed to update schedule");

            return Ok(new { 
                success = true, 
                message = "Schedule updated successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to update schedule: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        try
        {
            var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
                return HandleError("Schedule not found", 404);

            // Authorization check - only creator or super admin can delete
            var userId = GetUserId();
            var userRole = GetUserRole();

            if (existingSchedule.CreatedById != userId && userRole != UserRole.SuperAdmin.ToString())
                return HandleError("Access denied", 403);

            var result = await _scheduleService.DeleteScheduleAsync(id);
            if (!result)
                return HandleError("Failed to delete schedule");

            return Ok(new { 
                success = true, 
                message = "Schedule deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            return HandleError($"Failed to delete schedule: {ex.Message}");
        }
    }
}