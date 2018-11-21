using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SceneSys.Pages
{
    public class ScenesModel : PageModel
    {
        private readonly IRepository<Scene> _sceneRepository;

        public ICollection<Scene> Scenes { get; private set; }

        public string OwnerName { get; private set; }

        public ScenesModel(IRepository<Scene> sceneRepository)
        {
            _sceneRepository = sceneRepository;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id != null)
            {
                Scenes = await _sceneRepository.Query.Where(x => x.OwnerId == id).ToListAsync();
                // kind of dirty solution?
                OwnerName = Scenes.First().OwnerName;
            }
            else
                Scenes = await _sceneRepository.FetchAllAsync();

            if (Scenes == null)
                return NotFound();

            return Page();
        }
    }
}
