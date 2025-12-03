using System.Collections.Generic;
using System.Linq;

namespace GameGo.Application.Common.Models;

public class Result
{
	public bool IsSuccess { get; private set; }
	public string Error { get; private set; }
	public List<string> Errors { get; private set; }

	protected Result(bool isSuccess, string error)
	{
		IsSuccess = isSuccess;
		Error = error;
		Errors = new List<string>();
	}

	protected Result(bool isSuccess, List<string> errors)
	{
		IsSuccess = isSuccess;
		Errors = errors;
		Error = errors.Any() ? string.Join("; ", errors) : string.Empty;
	}

	public static Result Success() => new Result(true, string.Empty);
	public static Result Failure(string error) => new Result(false, error);
	public static Result Failure(List<string> errors) => new Result(false, errors);
}

public class Result<T> : Result
{
	public T Data { get; private set; }

	private Result(bool isSuccess, T data, string error) : base(isSuccess, error)
	{
		Data = data;
	}

	private Result(bool isSuccess, T data, List<string> errors) : base(isSuccess, errors)
	{
		Data = data;
	}

	public static Result<T> Success(T data) => new Result<T>(true, data, string.Empty);
	public static new Result<T> Failure(string error) => new Result<T>(false, default(T), error);
	public static new Result<T> Failure(List<string> errors) => new Result<T>(false, default(T), errors);
}