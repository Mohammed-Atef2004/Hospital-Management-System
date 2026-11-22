using Application.Common.Exceptions;
using Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred.");

            var isAppointmentSlotConflict = exception is DbUpdateException updateException &&
                updateException.GetBaseException() is SqlException sqlException &&
                (sqlException.Number is 2601 or 2627) &&
                sqlException.Message.Contains("IX_Appointments_DoctorId_AppointmentDate", StringComparison.OrdinalIgnoreCase);
            var isDepartmentNameConflict = exception is DbUpdateException departmentUpdateException &&
                departmentUpdateException.GetBaseException() is SqlException departmentSqlException &&
                (departmentSqlException.Number is 2601 or 2627) &&
                departmentSqlException.Message.Contains("IX_Departments_Name", StringComparison.OrdinalIgnoreCase);

            var (statusCode, message) = exception switch
            {
                _ when isAppointmentSlotConflict => (StatusCodes.Status409Conflict, "The doctor already has an appointment at that time."),
                _ when isDepartmentNameConflict => (StatusCodes.Status409Conflict, "A department with that name already exists."),
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                ForbiddenAccessException => (StatusCodes.Status403Forbidden, exception.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized."),
                BadRequestException => (StatusCodes.Status400BadRequest, exception.Message),
                FluentValidation.ValidationException validationException =>
                    (StatusCodes.Status400BadRequest, string.Join(" ", validationException.Errors.Select(error => error.ErrorMessage))),
                DomainException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode,
                message,

                details = _env.IsDevelopment() ? exception.StackTrace : null
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
