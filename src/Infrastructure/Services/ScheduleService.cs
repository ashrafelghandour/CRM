using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleService(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task<Schedule?> GetScheduleByIdAsync(int id)
    {
        return await _scheduleRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
    {
        return await _scheduleRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(int userId)
    {
        return await _scheduleRepository.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Schedule>> GetSchedulesByCustomerAsync(int customerId)
    {
        return await _scheduleRepository.GetByCustomerIdAsync(customerId);
    }

    public async Task<IEnumerable<Schedule>> GetUpcomingSchedulesAsync(int userId, string userRole)
    {
        if (userRole == "Customer")
        {
            return await _scheduleRepository.GetByCustomerIdAsync(userId);
        }
        else
        {
            return await _scheduleRepository.GetUpcomingAsync();
        }
    }

    public async Task<Schedule> CreateScheduleAsync(Schedule schedule)
    {
        return await _scheduleRepository.AddAsync(schedule);
    }

    public async Task<bool> UpdateScheduleAsync(Schedule schedule)
    {
        var existingSchedule = await _scheduleRepository.GetByIdAsync(schedule.Id);
        if (existingSchedule == null) return false;

        await _scheduleRepository.UpdateAsync(schedule);
        return true;
    }

    public async Task<bool> DeleteScheduleAsync(int id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null) return false;

        await _scheduleRepository.DeleteAsync(schedule);
        return true;
    }

    public Task<IEnumerable<Schedule>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
       return _scheduleRepository.GetSchedulesByDateRangeAsync(startDate,endDate);
    }
}