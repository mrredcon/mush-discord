using System.Linq;
using System.Threading.Tasks;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Data.Models.Scenes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FalloutRPG.SceneSys.Pages
{
    public class ViewSceneModel : PageModel
    {
        private readonly IRepository<Scene> _sceneRepository;
        public Scene Scene { get; private set; }

        public ViewSceneModel(IRepository<Scene> sceneRepository)
        {
            _sceneRepository = sceneRepository;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Scene = await _sceneRepository.Query.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (Scene == null)
                return NotFound();

            return Page();
        }
    }
}