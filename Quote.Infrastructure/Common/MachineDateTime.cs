﻿using Quote.Application.Core.Abstractions.Common;

namespace Quote.Infrastructure.Common;
/// <summary>
/// Represents the machine date time service.
/// </summary>
internal sealed class MachineDateTime : IDateTime
{
	/// <inheritdoc />
	public DateTime UtcNow => DateTime.UtcNow;
}
