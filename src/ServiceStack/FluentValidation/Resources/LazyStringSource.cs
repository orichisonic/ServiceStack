#region License
// Copyright (c) .NET Foundation and contributors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// The latest version of this file can be found at https://github.com/FluentValidation/FluentValidation
#endregion
namespace ServiceStack.FluentValidation.Resources {
	using System;
	using Validators;

	/// <summary>
	/// Lazily loads the string
	/// </summary>
	[Obsolete("LazyStringSource is deprecated and will be removed in FluentValidation 10. Use a Func<PropertyValidatorContext, string> instead.")]
	public class LazyStringSource : IStringSource {
		readonly Func<ICommonContext, string> _stringProvider;

		/// <summary>
		/// Creates a LazyStringSource
		/// </summary>
		public LazyStringSource(Func<ICommonContext, string> stringProvider) {
			_stringProvider = stringProvider;
		}

		/// <summary>
		/// Gets the value
		/// </summary>
		/// <returns></returns>
		public string GetString(ICommonContext context) {
			try {
				return _stringProvider(context);
			}
			catch (NullReferenceException ex) {
				throw new FluentValidationMessageFormatException("Could not build error message- the message makes use of properties from the containing object, but the containing object was null.", ex);
			}
		}
	}

	[Obsolete("Remove for FV10")]
	internal class BackwardsCompatibleStringSource<TContext> : IStringSource where TContext : class,ICommonContext {
		public Func<TContext, string> Factory { get; }

		public BackwardsCompatibleStringSource(Func<TContext, string> factory) {
			Factory = factory;
		}

		public string GetString(ICommonContext context) {
			return Factory.Invoke(context as TContext);
		}
	}


	public class FluentValidationMessageFormatException : Exception {
		public FluentValidationMessageFormatException(string message) : base(message) {
		}

		public FluentValidationMessageFormatException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}
