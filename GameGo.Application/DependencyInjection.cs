using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using GameGo.Application.Common.Behaviours;

namespace GameGo.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		// AutoMapper
		services.AddAutoMapper(assembly);

		// FluentValidation
		services.AddValidatorsFromAssembly(assembly);

		// MediatR
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(assembly);

			// Pipeline behaviors
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
		});

		return services;
	}
}