using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusCake.Client.Models;

namespace StatusCake.Client.Interfaces
{
    public interface IStatusCakeClient
    {
        Task<TestDetails> GetTestDetailsAsync(long testId);
        Task<List<Test>> GetTestsAsync();
        Task<List<Test>> GetTestsAsync(long? contactGroupId, string status);
        Task<List<Period>> GetPeriodsAsync(long testId);
        Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId);
        Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, int limit);
        Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, string[] fields);
        Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, string[] fields, DateTime? startTime);
        Task<Dictionary<string, CheckResult>> GetCheckResultsAsync(long testId, string[] fields, DateTime? startTime, int? limit);
        Task<List<Alert>> GetAlertsAsync(long testId);
        Task<List<Alert>> GetAlertsAsync(long testId, DateTime? since);
        Task<Models.Auth> GetAuthDetailsAsync();
        Task<List<ContactGroup>> GetContactGroupsAsync();
        Task<SortedDictionary<DateTime, Availability>> GetUptimesAsync(long testId);
        Task<DeleteTest> DeleteTestAsync(long testId);
        Task<DeleteTest> DeleteContactGroupAsync(long contactId);
        Task<PutResponse> UpdateOrCreateContactGroupAsync(ContactGroup contactGroup);
        Task<PutResponse> UpdateOrCreateTestAsync(TestUpdate test);
    }
}
