using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using teste_api.Data;
using teste_api.Models.Entites;
using teste_api.Models;

namespace teste_api.Controllers
{
    public class MateriasController : Controller
    {
        private readonly AppDbContext dbContext;

        public MateriasController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var materias = await dbContext.Materias.ToListAsync();

            return View(materias);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Materia materiaModel)
        {
            var materias = await dbContext.Materias.FindAsync(materiaModel.Id);

            return View(materias);
        }

        [HttpGet]
        public IActionResult MateriaCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MateriaCreate(AddMateriaViewModel viewModel)
        {
            var materia = new Materia
            {
                Nome = viewModel.Nome,
                Periodo = viewModel.Periodo,
                Tutor = viewModel.Tutor,
            };

            await dbContext.Materias.AddAsync(materia);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("GetAll", "Materias");
        }

        [HttpGet]
        public async Task<IActionResult> EditarMateria(int id)
        {
            var materia = await dbContext.Materias.FindAsync(id);

            return View(materia);
        }

        [HttpPost]
        public async Task<IActionResult> EditarMateria(Materia materiaModel)
        {
            var materia = await dbContext.Materias.FindAsync(materiaModel.Id);

            if (materia is not null)
            {
                materia.Nome = materiaModel.Nome;
                materia.Periodo = materiaModel.Periodo;
                materia.Tutor = materiaModel.Tutor;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("GetAll", "Materias");
        }

        [HttpPost]
        public async Task<IActionResult> DeletarMateria(Materia materiaModel)
        {
            var materia = await dbContext.Materias
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == materiaModel.Id);

            if (materia is not null)
            {
                dbContext.Materias.Remove(materia);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("GetAll", "Materias");
        }
    }
}
