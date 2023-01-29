using FluentValidation.Results;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;

namespace Ordering.Application.Exceptions;

public class ValidationExpection : ApplicationException
{
    public IDictionary<string, string[]> Error { get; }
	public ValidationExpection()
			:base ("One or more validation failures have occurred.")
	{
		Error = new Dictionary<string, string[]> ();
	}
	public ValidationExpection(IEnumerable<ValidationFailure> failures)
			:this ()
	{
		Error = new Dictionary<string, string[]> ();
	}

}
