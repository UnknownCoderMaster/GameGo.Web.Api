// ============================================
// ApplicationDbContextFactory.cs
// Location: src/GameGo.Infrastructure/Persistence/
// ============================================
using GameGo.Application.Contracts.Identity;
using GameGo.Application.Contracts.Infrastructure;
using GameGo.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		// Configuration path - API papkasidagi appsettings.json
		var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "GameGo.Api");

		var configuration = new ConfigurationBuilder()
			.SetBasePath(basePath)
			.AddJsonFile("appsettings.json", optional: false)
			.AddJsonFile("appsettings.Development.json", optional: true)
			.Build();

		// DbContext options
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseNpgsql(
			configuration.GetConnectionString("DefaultConnection"),
			b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

		// Mock services for design-time
		var mockMediator = new MockMediator();
		var mockDateTime = new DateTimeService();
		var mockCurrentUser = new MockCurrentUserService();

		return new ApplicationDbContext(
			optionsBuilder.Options,
			mockMediator,
			mockDateTime,
			mockCurrentUser);
	}
}

// ============================================
// Mock Services (faqat design-time uchun)
// ============================================

public class MockMediator : IMediator
{
	public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
	{
		return Task.FromResult<TResponse>(default(TResponse));
	}

	public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
	{
		return Task.CompletedTask;
	}

	public Task<object> Send(object request, CancellationToken cancellationToken = default)
	{
		return Task.FromResult<object>(null);
	}

	public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
	{
		return AsyncEnumerable.Empty<TResponse>();
	}

	public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default)
	{
		return AsyncEnumerable.Empty<object>();
	}

	public Task Publish(object notification, CancellationToken cancellationToken = default)
	{
		return Task.CompletedTask;
	}

	public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
	{
		return Task.CompletedTask;
	}
}

public class MockCurrentUserService : ICurrentUserService
{
	public long? UserId => null;
	public string Email => string.Empty;
	public bool IsAuthenticated => false;
}

// ============================================
// AsyncEnumerable helper
// ============================================
public static class AsyncEnumerable
{
	public static async IAsyncEnumerable<T> Empty<T>()
	{
		await Task.CompletedTask;
		yield break;
	}
}