using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using teste_api.Data;
using teste_api.Models;
using teste_api.Models.Entites;

namespace teste_api.Controllers
{
    public class AlunosController : Controller
    {
        private readonly AppDbContext dbContext;

        public AlunosController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alunos = await dbContext.Alunos.ToListAsync();

            return View(alunos);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Aluno alunoModel)
        {
            var aluno = await dbContext.Alunos.FindAsync(alunoModel.Id);

            var alunoXmateria = dbContext.AlunoXMaterias
                .Where(a => a.AlunoId == alunoModel.Id)
                .Join(dbContext.Materias,
                    alunoMateria => alunoMateria.MateriaId,
                    materia => materia.Id,
                    (alunoMateria, materia) => materia)
                .ToList();

            aluno.Materia = alunoXmateria;

            return View(aluno);
        }

        [HttpGet]
        public IActionResult AlunoCreate()
        {
            AddAlunoViewModel materias = new AddAlunoViewModel();
            materias.ListaMaterias = dbContext.Materias.ToList();
            materias.CheckBoxes = new List<CheckBoxOption>();

            foreach (var materia in materias.ListaMaterias)
            {
                materias.CheckBoxes.Add(new CheckBoxOption()
                {
                    IsChecked = false,
                    Description = materia.Nome,
                    Value = materia.Id
                });
            }
            return View(materias);
        }

        [HttpPost]
        public async Task<IActionResult> AlunoCreate(AddAlunoViewModel viewModel)
        {

            var aluno = new Aluno
            {
                Nome = viewModel.Nome,
                Email = viewModel.Email,
                Telefone = viewModel.Telefone,
            };
            await dbContext.Alunos.AddAsync(aluno);
            await dbContext.SaveChangesAsync();

            if(viewModel.NomeUnico is not null)
            {
                for(var aXm = 0; aXm < viewModel.NomeUnico.Count; aXm++)
                {
                    var alunoXmateria = new AlunoXMaterias
                    {
                        AlunoId = aluno.Id,
                        MateriaId = viewModel.NomeUnico[aXm]
                    };
                    await dbContext.AlunoXMaterias.AddAsync(alunoXmateria);
                    await dbContext.SaveChangesAsync();
                }
            }

            return RedirectToAction("GetAll", "Alunos");
        }

        [HttpGet]
        public async Task<IActionResult> EditarAluno(int id)
        {
            var aluno = await dbContext.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            var materiasAssociadas = await dbContext.AlunoXMaterias
                                                .Where(am => am.AlunoId == id)
                                                .Select(am => am.MateriaId)
                                                .ToListAsync();

            var todasAsMaterias = await dbContext.Materias.ToListAsync();

            var checkBoxes = new List<CheckBoxOption>();
            foreach (var materia in todasAsMaterias)
            {
                checkBoxes.Add(new CheckBoxOption
                {
                    IsChecked = materiasAssociadas.Contains(materia.Id),
                    Description = materia.Nome,
                    Value = materia.Id
                });
            }

            var composedModel = new ComposedModel
            {
                Aluno = new Aluno { Id = aluno.Id, Nome = aluno.Nome, Email = aluno.Email, Telefone = aluno.Telefone },
                AddAlunoViewModel = new AddAlunoViewModel { CheckBoxes = checkBoxes }
            };

            return View(composedModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditarAluno(ComposedModel alunoModel)
        {
            var aluno = await dbContext.Alunos.FindAsync(alunoModel.Aluno.Id);

            if (aluno is not null)
            {
                aluno.Nome = alunoModel.Aluno.Nome;
                aluno.Email = alunoModel.Aluno.Email;
                aluno.Telefone = alunoModel.Aluno.Telefone;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("GetAll", "Alunos");
        }

        [HttpPost]
        public async Task<IActionResult> DeletarAluno(ComposedModel alunoModel)
        {
            var aluno = await dbContext.Alunos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == alunoModel.Aluno.Id);

            if (aluno is not null)
            {
                dbContext.Alunos.Remove(aluno);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("GetAll", "Alunos");
        }
    }
}
