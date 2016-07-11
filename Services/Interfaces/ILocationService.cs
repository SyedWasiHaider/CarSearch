using System;
using System.Threading.Tasks;

namespace CarSearch
{
	public interface ILocationService
	{
		Task<string> getCurrentZipCode();
	}
}

