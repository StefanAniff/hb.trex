using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class PriceService : IPriceService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public PriceService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        #region IPriceService Members

        public double GetPrice(double? price, int taskId, int userId)
        {
            if (price.HasValue && price > 0)
            {
                return price.Value;
            }

            var user = _userRepository.GetByUserID(userId);

            return user.GetPricePrHour(_taskRepository.GetById(taskId).Project.Customer);
        }

        #endregion
    }
}