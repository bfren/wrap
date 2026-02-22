// Wrap: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics;
using AppMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Wrap.Ids;

namespace AppMvc.Controllers;

public class HomeController : Controller
{
	public IActionResult Index() =>
		View();

	public IActionResult Privacy() =>
		View();

	[HttpGet("/monad/{testId}|{pc0}|{shouldBeTrue}/{pc1?}")]
	public IActionResult Monad(TestId testId, Postcode pc0, Maybe<bool> shouldBeTrue, Maybe<Postcode> pc1) =>
		View("Monad", new { testId, pc0, shouldBeTrue, pc1 });

	[HttpGet("/id/{testId}")]
	public IActionResult Id(TestId testId) =>
		View("Monad", testId);

	[HttpGet("/postcode/{postcode}")]
	public IActionResult Postcode(Postcode postcode) =>
		View("Monad", postcode);

	[HttpGet("/maybeBool/{maybe?}")]
	public IActionResult MaybeBool(Maybe<bool> maybe) =>
		View("Monad", maybe);

	[HttpGet("/maybePostcode/{postcode?}")]
	public IActionResult MaybePostcode(Maybe<Postcode> postcode) =>
		View("Monad", postcode);

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() =>
		View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}

public sealed record class TestId : LongId<TestId>;

public sealed record class Postcode : Monad<Postcode, string>;
