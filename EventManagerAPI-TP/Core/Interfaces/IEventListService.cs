public interface IEventListService
{
    Task<EventListResult> GetEventsAsync(DateTime? startDate, DateTime? endDate, int? locationId, int? category, int? status, int page, int pageSize);

}