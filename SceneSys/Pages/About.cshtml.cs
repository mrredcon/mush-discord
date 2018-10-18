using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FalloutRPG.Data.Repositories;
using FalloutRPG.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SceneSys.Pages
{
    public class AboutModel : PageModel
    {
        public IRepository<Scene> SceneRepository { get; private set; }

        public string Message { get; set; }

        public AboutModel(IRepository<Scene> sceneRepository)
        {
            SceneRepository = sceneRepository;
        }
        
        public void OnGet()
        {
            Message = "Welcome to the about page!";
        }
    }
}
