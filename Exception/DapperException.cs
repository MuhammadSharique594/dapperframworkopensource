
namespace DapperFramework.Exception
{
	public class DapperException : System.Exception
	{
		public DapperException(string message)
			: base(message)
		{ }

		public DapperException(string message, System.Exception exception )
			: base(message, exception)
		{ }
	}
}
