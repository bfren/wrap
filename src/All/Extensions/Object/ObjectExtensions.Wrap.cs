// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	/// <inheritdoc cref="ObjectWrap{T}"/>
	public static ObjectWrap<T> Wrap<T>(this T value) =>
		new(value);

	/// <summary>
	/// Fluently wrap a value with various monads.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="value">Value.</param>
	public sealed class ObjectWrap<T>(T value)
	{
		/// <summary>
		/// Return value wrapped in a <see cref="Maybe{T}"/>.
		/// </summary>
		/// <returns>Maybe object.</returns>
		public Maybe<T> AsMaybe() =>
			M.Wrap(value);

		/// <summary>
		/// Return value wrapped in a <see cref="Either{TLeft, TRight}"/>.
		/// </summary>
		/// <typeparam name="TLeft">Left (error / invalid) type.</typeparam>
		/// <returns>Either object.</returns>
		public Either<TLeft, T> AsEither<TLeft>() =>
			E.WrapRight<TLeft, T>(value);

		/// <summary>
		/// Return value wrapped in a <see cref="Result{T}"/>.
		/// </summary>
		/// <returns>Result object</returns>
		public Result<T> AsResult() =>
			R.Wrap(value);

		/// <summary>
		/// Return value wrapped in a <see cref="IUnion{TUnion, TValue}"/>.
		/// </summary>
		/// <typeparam name="TUnion">Union type.</typeparam>
		/// <returns>Union object</returns>
		public TUnion AsUnion<TUnion>()
			where TUnion : IUnion<TUnion, T>, new() =>
			F.Wrap<TUnion, T>(value);
	}
}
