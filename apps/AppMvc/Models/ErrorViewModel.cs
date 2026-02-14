// Wrap: Test Apps.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace AppMvc.Models;

public class ErrorViewModel
{
	public string? RequestId { get; set; }

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
