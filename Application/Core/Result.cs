using Microsoft.AspNetCore.Http;

namespace Application.Core
{
	public class Result<T>
	{
		public bool IsSuccess { get; set; }
		public T Value { get; set; }
		public string Error { get; set; }
		public int StatusCode { get; set; }


		public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, StatusCode = StatusCodes.Status200OK, Value = value };
		public static Result<T> CreatedSuccess(T value) => new Result<T> { IsSuccess = true, StatusCode = StatusCodes.Status201Created, Value = value };
		public static Result<T> AcceptedSuccess(T value) => new Result<T> { IsSuccess = true, StatusCode = StatusCodes.Status202Accepted, Value = value };
		public static Result<T> NoContentSuccess(T value) => new Result<T> { IsSuccess = true, StatusCode = StatusCodes.Status204NoContent, Value = value };
		public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest, Error = error };
		public static Result<T> NotFound(string error) => new Result<T> { IsSuccess = false, StatusCode = StatusCodes.Status404NotFound, Error = error };
		public static Result<T> Unauthorized(string error) => new Result<T> { IsSuccess = false, StatusCode = StatusCodes.Status401Unauthorized, Error = error };
		public static Result<T> Forbidden(string error) => new Result<T> { IsSuccess = false, StatusCode = StatusCodes.Status403Forbidden, Error = error };
		

	}
}