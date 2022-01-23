using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SquadMatching.Models;
using SquadMatching.DAL;
using System.IO;
using SquadMatching.MLDOTNET;
using Microsoft.ML;

using Newtonsoft.Json;
using System.Linq;
using System.Globalization;
using System.Text;

namespace SquadMatching.Controllers
{
   
    public class HomeController : Controller
    {
        public JsonResult GetDashBoard(string Habilidade)
        {
            CRUDDAO ScoreBoard = new CRUDDAO();
            List<UsuarioModel> DashBoard = new List<UsuarioModel>();
            DashBoard = ScoreBoard.GetScoreBoard(Habilidade);
            return Json(DashBoard.ToList());
        }
        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("Nome") != null)
            {
                return RedirectToAction("Perfil");
            }
            else { return View("Index"); }



        }

        public IActionResult TesteData()
        {
            /*   DataBaseLoader datab = new DataBaseLoader();
               IDataView dataview = datab.GetSrc();
               var teste = dataview.Preview();*/
            DataBaseLoader datab = new DataBaseLoader();
            datab.Main();

            return View("Index");
        }

        [HttpPost]
        public IActionResult Index(CadastroModel cadastro)
        {

            CRUDDAO LoginDAO = new CRUDDAO();
            UsuarioModel usuario = new UsuarioModel();
            usuario = LoginDAO.Login(cadastro);
            if (usuario.Nome != null)
            {
                if (usuario.Nome != null)
                    HttpContext.Session.SetString("Nome", usuario.Nome);
                HttpContext.Session.SetString("Matricula", usuario.Matricula);
                HttpContext.Session.SetString("EProfessor", Convert.ToString(usuario.CD_Professor));
                HttpContext.Session.SetString("imgPath", usuario.imgPath);
                return RedirectToAction("Perfil");

            }
            else
            {
                ViewBag.Error = "Usuario ou Senha não existente";
                return View();
            }


        }
        public JsonResult LoadAvaliacoes(String HabDesc)
        {
           

            CRUDDAO Media = new CRUDDAO();

            List<HabilidadeModel> Habilidades = new List<HabilidadeModel>();
            Habilidades = Media.AvaliacoesAluno(HabDesc,HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            return Json(Habilidades.ToList());
        }
        public IActionResult VerAvaliacoes(string Habilidade)
        {
            ViewBag.HabilidadeDesc = Habilidade.Replace("%"," ");
            return PartialView("_PartialAvaliacoes");
        }
        public IActionResult UPTRAB(String Link,int CD_GRUPO)
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            PerfilDAO.SETLINK(Link, CD_GRUPO);
            return RedirectToAction("Turma");
        }
        public double GetAlunoNotaAtividade()
        {
            CRUDDAO Getnota = new CRUDDAO();
            var nota = Getnota.GetNotaAlunoAtividade(HttpContext.Session.GetString("CD_Atividade"), HttpContext.Session.GetString("Matricula"));
            return nota;
        }
        public ActionResult TesteView()
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            ViewData["Message"] = "Inicio.";
            List<SelectListItem> Lista = new List<SelectListItem>();
            var flag = PerfilDAO.AlunoTemGrupo(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            AtividadeModel AtividadeStatus = new AtividadeModel();
         
           
            if (flag > 0)
            {
              var CD_GRUPO= PerfilDAO.GetCD_GRUPO(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
                /*List<UsuarioModel> Pendencias = new List<UsuarioModel>();
                Pendencias = PerfilDAO.GETSOLICITACAO(CD_GRUPO);
                ViewBag.PendenciasQNT = Pendencias.Count();*/
                GrupoModel grupo = new GrupoModel();
                grupo.id = HttpContext.Session.GetString("CD_Atividade");
                grupo.Alunos = PerfilDAO.GetAlunosGrupo(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
                ViewBag.CD_GRUPO = CD_GRUPO;
                List<UsuarioModel> Pendencias = new List<UsuarioModel>();
                Pendencias = PerfilDAO.GETSOLICITACAO(CD_GRUPO);
                ViewBag.PendenciasQNT = Pendencias.Count();
                ViewBag.ESquadMaster2 = PerfilDAO.ESquadMaster(HttpContext.Session.GetString("Matricula"), CD_GRUPO);
                return PartialView("_PartialTeste", grupo);
                }
            else
            {
                List<GrupoModel> Grupos = new List<GrupoModel>();
                Grupos = PerfilDAO.getGrupos(HttpContext.Session.GetString("CD_Atividade"));
                return PartialView("InsertGrupo");
            }

        }
        public IActionResult AceitarPendencia(int CD_ALUNO,int CD_GRUPO)
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            PerfilDAO.AceitarPendencia(CD_ALUNO, CD_GRUPO);
            return RedirectToAction("Perfil");
        }
        public IActionResult RecusarPendencia(int CD_ALUNO, int CD_GRUPO)
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            PerfilDAO.RecusarPendencia(CD_ALUNO, CD_GRUPO);
            return RedirectToAction("Perfil");
        }
        public ActionResult VerPendencias()
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            var CD_GRUPO = PerfilDAO.GetCD_GRUPO(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            List<UsuarioModel> Pendencias = new List<UsuarioModel>();
            ViewBag.CD_GRUPO = CD_GRUPO;
            Pendencias = PerfilDAO.GETSOLICITACAO(CD_GRUPO);
            return PartialView("_PartialPendencias", Pendencias);
        }
        public ActionResult ListaGrupos()
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            List<GrupoModel> Grupos = new List<GrupoModel>();
            Grupos = PerfilDAO.getGrupos(HttpContext.Session.GetString("CD_Atividade"));
            return PartialView("ListaGrupos", Grupos);
        }

        public ActionResult DashBoard()
        {
            /*CRUDDAO PerfilDAO = new CRUDDAO();
            List<GrupoModel> Grupos = new List<GrupoModel>();
            Grupos = PerfilDAO.getGrupos(HttpContext.Session.GetString("CD_Atividade"));*/
            return PartialView("_PartialDashBoard");
        }
        public ActionResult ScoreBoard()
        {
            /*CRUDDAO PerfilDAO = new CRUDDAO();
            List<GrupoModel> Grupos = new List<GrupoModel>();
            Grupos = PerfilDAO.getGrupos(HttpContext.Session.GetString("CD_Atividade"));*/
            return PartialView("_PartialScoreBoard");
        }
        public ActionResult ProfAvalia()
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            List<GrupoModel> Grupos = new List<GrupoModel>();
            Grupos = PerfilDAO.getGruposAvaliar(HttpContext.Session.GetString("CD_Atividade"));
            return PartialView("_PartialProfAvalia", Grupos);
        }

        public ActionResult ProfAvaliaNota(List<GrupoModel> Grupos)
        {

            CRUDDAO avaliar = new CRUDDAO();
            foreach (var grupos in Grupos)
            {
                avaliar.AvaliarGrupoProf(Convert.ToInt32(grupos.id), grupos.Nota);


            }

            return RedirectToAction("Index");
        }

        public JsonResult MediaAlunoGrupo()
        {
            
            CRUDDAO Media = new CRUDDAO();
            List<HabilidadeModel> Habilidades = new List<HabilidadeModel>();
            Habilidades = Media.MediaAlunoGrupo(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            return Json(Habilidades.ToList());
        }
        public JsonResult RenderScoreAluno()
        {
   
            CRUDDAO Media = new CRUDDAO();
            List<HabilidadeModel> Habilidades = new List<HabilidadeModel>();
            Habilidades = Media.ScoreAluno(HttpContext.Session.GetString("Matricula"));
            return Json(Habilidades.ToList());
        }

        public JsonResult GetLinkTarefa(int cd_grupo)
        {
            CRUDDAO Media = new CRUDDAO();
            string LinkAtividade = Media.GetLinkAtividade(cd_grupo);
            return Json(LinkAtividade);
        }


        public IActionResult CompararAlunoTurma()
        {
            CRUDDAO GetMedia = new CRUDDAO();
            List<Double> Medias = new List<double>();
            Medias.Add(GetMedia.GetMediaAlunoTurma(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("IDT"), HttpContext.Session.GetString("IDM")));
            Medias.Add(GetMedia.GetMediaTurma(HttpContext.Session.GetString("IDT"), HttpContext.Session.GetString("IDM")));
            return PartialView("_PartialCompAlunoTurma", Medias);
        }

        public JsonResult MediaAlunoGrupoAtividade()
        {
            CRUDDAO Media = new CRUDDAO();
            List<Double> Medias = new List<double>();
            Medias.Add(Media.GetMediaAlunoTurma(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("IDT"), HttpContext.Session.GetString("IDM")));
            Medias.Add(Media.GetMediaTurma(HttpContext.Session.GetString("IDT"), HttpContext.Session.GetString("IDM")));
            return Json(Medias.ToList());
        }
        /*  public ActionResult RenderTurmas()
          {
              CRUDDAO PerfilDAO = new CRUDDAO();

          }*/

        [HttpPost]
        public IActionResult InsertGrupo(GrupoModel Grupo)
        {
            CRUDDAO Crud = new CRUDDAO();
            Crud.InsertGrupo(Grupo.Nome, HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            return RedirectToAction("Perfil");
        }
      
        [HttpGet]
        public IActionResult PainelProfessor()
        {
            AtividadeModel atividade = new AtividadeModel();
            ViewBag.ID = HttpContext.Session.GetString("IDT");
            ViewBag.Nome = HttpContext.Session.GetString("NOMET");
            return View(atividade);
        }
      
        [HttpPost]
        public IActionResult PainelProfessor(AtividadeModel atividade)
        {
            HabilidadeModel Habilidade = new HabilidadeModel();
            CRUDDAO InsertAtividade = new CRUDDAO();
          

            atividade.CD_Turma = HttpContext.Session.GetString("IDT");
            atividade.CD_Materia = HttpContext.Session.GetString("IDM");
            InsertAtividade.InsertAtividade(atividade);
            return RedirectToAction("Turma");
        }

        [HttpPost]
        public void SetListaHab(JsonReader lista)
        {
            HabilidadeModel Habilidade = new HabilidadeModel();
            var teste = lista;



        }

        [HttpPost]
        public IActionResult AvaliarGrupo(List<UsuarioModel> Usuarios)
        {
            CRUDDAO avaliar = new CRUDDAO();
            foreach(var user in Usuarios)
            {
                foreach(var hab in user.Habilidades)
                {
                    var SRating = hab.Rating.ToString();
                   

                    avaliar.AvaliarUsuario(user.Matricula, hab.Descricao, hab.Rating, Convert.ToInt32(HttpContext.Session.GetString("CD_Atividade")),hab.OBS);
                }
            }
            avaliar.UpdateStatusAlunoAtividade(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            return RedirectToAction("Turma");
        }

        public IActionResult AvaliarGrupo()
        {
            if (HttpContext.Session.GetString("Matricula") != null)
            {
                CRUDDAO cruddao = new CRUDDAO();
                List<UsuarioModel> Alunos = new List<UsuarioModel>();
                Alunos = cruddao.AvaliarGrupo(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
                return View("AvaliarGrupo", Alunos);
            }
            else
            {
                return RedirectToAction("Index");
            }
               
        }

        public IActionResult ParticiparGrupo(int idg)
        {
            CRUDDAO cruddao = new CRUDDAO();
            cruddao.ParticiparGrupo(idg, HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            return RedirectToAction("Turma");

        }

        [HttpPost]
        public void FinalizarAtividade() {
            CRUDDAO Finalizar = new CRUDDAO();
            Finalizar.FinalizarAtividade(HttpContext.Session.GetString("CD_Atividade"));
            
}
       
        [HttpGet]
        public IActionResult Atividade(String CD_Atividade)
        {
            if (HttpContext.Session.GetString("Matricula") != null)
            {
                CRUDDAO AtivDetalhes = new CRUDDAO();
                AtividadeModel Atividade = new AtividadeModel();
                Atividade = AtivDetalhes.AtividadeDetalhes(CD_Atividade);
                HttpContext.Session.SetString("ATIVIDADESTATUS", Atividade.CD_Status);
                HttpContext.Session.SetString("CD_Atividade", CD_Atividade);
                Atividade.Alunos = AtivDetalhes.ListaAlunoAtividade(HttpContext.Session.GetString("IDT"), HttpContext.Session.GetString("IDM"));
                ViewBag.Ativo = AtivDetalhes.GetAtivo(HttpContext.Session.GetString("Matricula"), CD_Atividade);
                var CD_GRUPO = AtivDetalhes.GetCD_GRUPO(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
                List<UsuarioModel> Pendencias = new List<UsuarioModel>();
                Pendencias = AtivDetalhes.GETSOLICITACAO(CD_GRUPO);
                ViewBag.PendenciasQNT = Pendencias.Count();
                ViewBag.ESquadMaster = AtivDetalhes.ESquadMaster(HttpContext.Session.GetString("Matricula"), CD_GRUPO);
                return View(Atividade);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        [HttpGet]
        public IActionResult Cadastro()
        {

            //CRUDDAO CadastroDAO = new CRUDDAO();

            //CadastroModel cadastro = new CadastroModel();
            // cadastro.Habilidades = CadastroDAO.populateHabilidades();
            //cadastro
            return View();

        }

        public string[] getArray() {
            CRUDDAO CadastroDAO = new CRUDDAO();

            CadastroModel cadastro = new CadastroModel();
            cadastro.Habilidades = CadastroDAO.populateHabilidades();
            List<string> colecao = new List<string>();
                foreach (HabilidadeModel habilidade in cadastro.Habilidades)
                {
                    colecao.Add(habilidade.Descricao);
                }
            return colecao.ToArray();
        }

        public JsonResult MyData(string text)
        {
          
            text = text.ToLower().Trim();

            string[] words = getArray();
           
           
            IEnumerable<string> matched = words.Where(x => x.ToLower().Contains(text));
           
          
         
            //JsonRequestBehavior.AllowGet
            return Json(matched);
        }

        [HttpPost]
        public IActionResult Cadastro(CadastroModel cadastro)
        {
            string fileName;
            UsuarioModel usuario = new UsuarioModel();
            if (cadastro.image != null)
            {
                DateTime dt = new DateTime();
                dt = DateTime.Now;

                string uploadFolder = "wwwroot/images/ProfilePics";
                fileName = String.Format(cadastro.Matricula, "{0:ddMMyyyy}", dt) + "_" + "ProfilePic" + cadastro.image.FileName;
                string filePath = Path.Combine(uploadFolder, fileName); ;
                cadastro.image.CopyTo(new FileStream(filePath, FileMode.Create));
                cadastro.ImagePath = fileName;
            }
            CRUDDAO CadastroDAO = new CRUDDAO();
            Boolean Cadastrado = CadastroDAO.Cadastrar(cadastro);
            if (Cadastrado == true)
            {
                return RedirectToAction("Index");
            }
            else {
                ViewBag.Error = "Usuario ou matricula já cadastrado";
                return View(cadastro);
            }
            
        }

        [HttpPost]
        public JsonResult GetHabilidades(String Prefixo)
        {
            CRUDDAO GetHab = new CRUDDAO();
            List<HabilidadeModel> habilidadesbase = new List<HabilidadeModel>();
            List<HabilidadeModel> habilidadesfiltro = new List<HabilidadeModel>();
            if (habilidadesbase.Count == 0) { habilidadesbase = GetHab.populateHabilidades(); }
            var teste = Prefixo;
            if (Prefixo.Length > 0)
            {
                foreach (var hab in habilidadesbase)
                {
                    if (hab.Descricao.Contains(Prefixo, StringComparison.OrdinalIgnoreCase))
                    {

                        HabilidadeModel hab2 = new HabilidadeModel();
                        hab2.Descricao = hab.Descricao;
                        hab2.CD_Habilidade = hab.CD_Habilidade;
                        habilidadesfiltro.Add(hab2);
                    }
                }

            }


            return Json(habilidadesfiltro.ToList());
        }

        public JsonResult GetScoreAluno()
        {
            CRUDDAO Score = new CRUDDAO();
            double Media = Score.GetScoreAluno(HttpContext.Session.GetString("Matricula"));
            return (Json(Media));
        }

        [HttpGet]
        public IActionResult Turma(String IDT,String IDM)
        {
       
            CRUDDAO Atividades = new CRUDDAO();
            if(IDT!=null && IDM !=null)
            {
                HttpContext.Session.SetString("IDT", IDT);
                HttpContext.Session.SetString("IDM", IDM);
            }
          
            
            return View("Turma", Atividades.SelectAtividade(HttpContext.Session.GetString("IDT"), HttpContext.Session.GetString("IDM")));
            //
        }
       
        public IActionResult Logout() {
                HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("Matricula") != null)
            {
                CRUDDAO PerfilDAO = new CRUDDAO();
                ViewData["Message"] = "Inicio.";
                List<SelectListItem> Lista = new List<SelectListItem>();


                return View(PerfilDAO.populateTurma(HttpContext.Session.GetString("Matricula"))); ;
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        public IActionResult SetAlunoTurma(int cd_turma,int cd_materia)
        {
            if (HttpContext.Session.GetString("Nome") != null)
            {
                CRUDDAO SetAluno = new CRUDDAO();
                var flag = SetAluno.AlunoPertenceTurma(HttpContext.Session.GetString("Matricula"), cd_turma, cd_materia);
                if(flag == 0) {
                SetAluno.InsertAlunoTurma(HttpContext.Session.GetString("Matricula"), cd_turma, cd_materia);
                    return RedirectToAction("Perfil");
                }else return RedirectToAction("Perfil");

            }
            else { return View("Index"); }
        }
        [HttpGet]
        public IActionResult _PartialChat()
        {
            CRUDDAO PerfilDAO = new CRUDDAO();
            var CD_GRUPO = PerfilDAO.GetCD_GRUPO(HttpContext.Session.GetString("Matricula"), HttpContext.Session.GetString("CD_Atividade"));
            ViewBag.CD_GRUPOCHAT = CD_GRUPO;

            return PartialView("_PartialChat");
        }
        public IActionResult _PartialDetalhes(string id)
        {
          
            CRUDDAO Media = new CRUDDAO();
            List<HabilidadeModel> Habilidades = new List<HabilidadeModel>();
            Habilidades = Media.DetalheAluno(id);
            return Json(Habilidades.ToList());

        }
        public IActionResult ShowDetalhes(int id)
        {
            ViewBag.DetalhesId = id;
            return PartialView("_PartialDetalhes");
        }
        [HttpGet]
        public IActionResult Editar()
        {
            CRUDDAO EditarDAO = new CRUDDAO();
            CadastroModel cadastro = new CadastroModel();
            cadastro.Habilidades = EditarDAO.populateHabilidades();
            cadastro.Matricula = HttpContext.Session.GetString("Matricula");
            cadastro = EditarDAO.EditarGET(cadastro);
            return View(cadastro);

        }

        [HttpPost]
        public IActionResult Editar(CadastroModel cadastro)
        {
            cadastro.Matricula = HttpContext.Session.GetString("Matricula");
            if (cadastro.image != null)
            {
                DateTime dt = new DateTime();
                dt = DateTime.Now;

                string uploadFolder = "wwwroot/images/ProfilePics";
                string fileName = String.Format(cadastro.Matricula, "{0:ddMMyyyy}", dt) + "_" + "ProfilePic" + cadastro.image.FileName;
                string filePath = Path.Combine(uploadFolder, fileName); ;
                cadastro.image.CopyTo(new FileStream(filePath, FileMode.Create));
                cadastro.ImagePath = fileName;
            }
            CRUDDAO EditarDAO = new CRUDDAO();
            EditarDAO.EditarPOST(cadastro);


            if (cadastro.image == null)
            {
                HttpContext.Session.SetString("imgPath", "null.jpg");
                cadastro.ImagePath = "null.jpg";
            }
            else
                HttpContext.Session.SetString("imgPath", cadastro.ImagePath);
            HttpContext.Session.SetString("Nome", cadastro.Nome);
            return RedirectToAction("Perfil");

          
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}
