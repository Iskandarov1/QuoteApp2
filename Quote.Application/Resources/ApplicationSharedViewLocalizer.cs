﻿using Microsoft.Extensions.Localization;
using System.Reflection;
using Quote.Domain.Core.Localizations;

namespace Quote.Application.Resources;

public sealed class ApplicationSharedViewLocalizer : ISharedViewLocalizer
{
	private readonly IStringLocalizer localizer;

	public ApplicationSharedViewLocalizer(IStringLocalizerFactory factory)
	{
		var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
		localizer = factory.Create("SharedResource", assemblyName.Name);
	}

	public LocalizedString this[string key] => localizer[key];

	public LocalizedString GetLocalizedString(string key)
	{
		return localizer[key];
	}

	public IStringLocalizer Localizer => localizer;
}

