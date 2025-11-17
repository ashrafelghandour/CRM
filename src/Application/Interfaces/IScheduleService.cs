using Domain.Entities;

namespace Application;

public interface IScheduleService
{
    Task<Schedule?> GetScheduleByIdAsync(int id);
    Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
    Task<IEnumerable<Schedule>> GetSchedulesByUserAsync(int userId);
    Task<IEnumerable<Schedule>> GetSchedulesByCustomerAsync(int customerId);
    Task<IEnumerable<Schedule>> GetUpcomingSchedulesAsync(int userId, string userRole);
    Task<Schedule> CreateScheduleAsync(Schedule schedule);
    Task<bool> UpdateScheduleAsync(Schedule schedule);
    Task<bool> DeleteScheduleAsync(int id);
}