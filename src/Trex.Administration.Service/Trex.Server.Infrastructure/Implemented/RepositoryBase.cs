using NHibernate;
using StructureMap;

namespace Trex.Server.Infrastructure.Implemented
{
	public class RepositoryBase
	{
		private readonly ISessionBuilder _sessionBuilder;

		public RepositoryBase(ISessionBuilder sessionFactory)
		{
			_sessionBuilder = sessionFactory;
		}

		public RepositoryBase() : this(ObjectFactory.GetInstance<ISessionBuilder>())
		{
		}

		protected ISession GetSession()
		{
			ISession session = _sessionBuilder.GetSession();
			return session;
		}
	}
}