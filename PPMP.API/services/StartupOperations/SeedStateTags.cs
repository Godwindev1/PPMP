
using PPMP.API.Repo;

namespace PPMP.API.Services.Operations
{
    public class SeedStateTags : IStartupOperation
    {
        private readonly StateTagRepo _stateTagRepo;
        public SeedStateTags(StateTagRepo stateTagRepo)
        {
            _stateTagRepo = stateTagRepo;
        }

        public async Task ExecuteOperationAsync()
        {
            if (_stateTagRepo != null)
            {
                await _stateTagRepo.AddDefaultStateTags();
            }
        }
    }
}