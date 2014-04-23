using NHibernate;
using NHibernate.Cfg;

namespace Trex.Server.Infrastructure
{
	public interface ISessionBuilder
	{
		ISession GetSession();
		Configuration GetConfiguration();
	}
}