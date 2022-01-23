using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SquadMatching.Models;
using Microsoft.AspNetCore.Mvc;

namespace SquadMatching.DAL
{
    public class CRUDDAO
    {
        //static string strCon = "Server=LAPTOP-G4ABSSIA;Database=squadmatch;Trusted_Connection=True;";
        static string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
        public List<UsuarioModel> GetScoreBoard(string Habilidade)
        {
            List<UsuarioModel> ScoreBoard = new List<UsuarioModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_MEDIA_RANKING";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@HABILIDADE", SqlDbType.VarChar).Value = Habilidade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            UsuarioModel Hab = new UsuarioModel();
                            Hab.Nome = sdr["NOME"].ToString();
                            Hab.Media = Convert.ToDouble(sdr["MEDIA"].ToString()/*.Replace(",", ".")*/);
                            ScoreBoard.Add(Hab);
                        }

                    }
                }
                con.Close();
            }

            return ScoreBoard;
        }
        public double GetNotaAlunoAtividade(string cd_atividade,string matricula)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                double teste = 0;
                string query = "SPC_NOTA_ATIVIDADE";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = matricula;
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value =cd_atividade;

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            teste = Convert.ToDouble(sdr["MEDIA"].ToString());

                        }



                        con.Close();
                    }
                }
                return teste;
            }
        }
        public void RecusarPendencia(int CD_ALUNO, int CD_GRUPO)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPX_RECUSAR_PENDENCIA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_ALUNO", SqlDbType.VarChar).Value = CD_ALUNO;
            cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = CD_GRUPO;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();

            }
        }
        public void AceitarPendencia(int CD_ALUNO, int CD_GRUPO)
        {
           // string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPX_ACEITAR_PENDENCIA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_ALUNO", SqlDbType.VarChar).Value = CD_ALUNO;
            cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = CD_GRUPO;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();

            }
        }
        public UsuarioModel Login(CadastroModel cadastro)
        {
            UsuarioModel usuario = new UsuarioModel();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPX_AUTENTICACAO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Login", SqlDbType.VarChar).Value = cadastro.Login;
            cmd.Parameters.Add("@Senha", SqlDbType.VarChar).Value = cadastro.Senha;

            con.Open();
            try
            {
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {

                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            usuario.Nome = sdr["NOME"].ToString();
                            usuario.Matricula = sdr["MATRICULA"].ToString();
                            usuario.CD_Professor = Convert.ToBoolean(sdr["CD_PROFESSOR"].ToString());
                            usuario.imgPath = sdr["PROFILEPIC"].ToString();

                        }
                        con.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                String[] LoginErro = new String[] { e.Message };
                Console.WriteLine(e.Message);
                con.Close();
                return usuario;


            }
            con.Close();
            return usuario;
        }
        public void InsertGrupo(String Nome, String Matricula, String CD_Atividade)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_GRUPO_ALUNO_GRUPO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@NOME_GRUPO", SqlDbType.VarChar).Value = Nome;
            cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
            cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();

            }
        }
        public void AvaliarUsuario(String Matricula, String Descricao, Double Rating, int CD_ATIVIDADE, String OBS)
        {
           // string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_ALUNO_NOTA_HABILIDADE", con);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
            cmd.Parameters.Add("@HABILIDADE", SqlDbType.VarChar).Value = Descricao;
            cmd.Parameters.Add("@NOTA", SqlDbType.Float).Value = Rating.ToString().Replace(".", ",");
            cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.Int).Value = CD_ATIVIDADE;
            if (OBS != null) {
                cmd.Parameters.Add("@OBS", SqlDbType.VarChar).Value = OBS;
            }


            

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();




            }


        }

        public void AvaliarGrupoProf(int CD_GRUPO, Double Rating)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPX_GRUPO_NOTA", con);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = CD_GRUPO;

            cmd.Parameters.Add("@NOTA", SqlDbType.Float).Value = Rating.ToString().Replace(".", ",");


            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();




            }
        }

        public List<HabilidadeModel> MediaAlunoGrupo(String Matricula, String CD_ATIVIDADE)
        {
            List<HabilidadeModel> Habilidade = new List<HabilidadeModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_MEDIA_ALUNO_GRUPO";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_ATIVIDADE;
                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            HabilidadeModel Hab = new HabilidadeModel();
                            Hab.Descricao = sdr["DESCRICAO"].ToString();
                            Hab.Rating = Convert.ToDouble(sdr["MEDIA"].ToString()/*.Replace(",", ".")*/);
                            Habilidade.Add(Hab);
                        }

                    }
                }
                con.Close();
            }

            return Habilidade;
        }

        public List<HabilidadeModel> ScoreAluno(String Matricula)
        {
            List<HabilidadeModel> Habilidade = new List<HabilidadeModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_MEDIA_GERAL";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            HabilidadeModel Hab = new HabilidadeModel();
                            Hab.Descricao = sdr["DESCRICAO"].ToString();
                            Hab.Rating = Convert.ToDouble(sdr["MEDIA"].ToString()/*.Replace(",", ".")*/);
                            Habilidade.Add(Hab);
                        }

                    }
                }
                con.Close();
            }

            return Habilidade;
        }

        public List<HabilidadeModel> DetalheAluno(String cd_aluno)
        {
            List<HabilidadeModel> Habilidade = new List<HabilidadeModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_MEDIA_GERAL";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.Add("@CD_USUARIO", SqlDbType.VarChar).Value = cd_aluno;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            HabilidadeModel Hab = new HabilidadeModel();
                            Hab.Descricao = sdr["DESCRICAO"].ToString();
                            Hab.Rating = Convert.ToDouble(sdr["MEDIA"].ToString()/*.Replace(",", ".")*/);
                            Habilidade.Add(Hab);
                        }

                    }
                }
                con.Close();
            }

            return Habilidade;
        }
        public List<HabilidadeModel> AvaliacoesAluno(string HabilidadeDesc,string Matricula,string cd_atividade)
        {
            List<HabilidadeModel> Habilidade = new List<HabilidadeModel>();
           // string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_NOTA_ALUNO_GRUPO";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = cd_atividade;
                    cmd.Parameters.Add("@HABILIDADE", SqlDbType.VarChar).Value = HabilidadeDesc;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            HabilidadeModel Hab = new HabilidadeModel();
                            Hab.Descricao = sdr["DESCRICAO"].ToString();
                            Hab.Rating = Convert.ToDouble(sdr["NOTA"].ToString()/*.Replace(",", ".")*/);
                            Hab.OBS= sdr["OBS"].ToString();
                            Habilidade.Add(Hab);
                        }

                    }
                }
                con.Close();
            }

            return Habilidade;
        }

       public int AlunoPertenceTurma(String Matricula, int cd_turma, int cd_materia)
        {
            int count = 0;
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_ALUNO_PERTENCE_TURMA";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@Matricula", SqlDbType.VarChar).Value = Matricula;
                    cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = cd_turma;
                    cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = cd_materia;

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            count++;

                        }



                        con.Close();
                    }
                }
                return count;
            }
           
        }

        public void UpdateStatusAlunoAtividade(String Matricula, String CD_Atividade)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPA_ALUNO_ATIVO_ATIVIDADE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
            cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;


            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();

            }
        }
        public List<UsuarioModel> AvaliarGrupo(String Matricula, String CD_Atividade)
        {
            List<HabilidadeModel> HabilidadeLista = new List<HabilidadeModel>();
            HabilidadeModel Hab = new HabilidadeModel();
            List<String> Habs = new List<String>();
            UsuarioModel Aluno = new UsuarioModel();
            List<UsuarioModel> Alunos = new List<UsuarioModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_ATIVIDADE_DETALHES";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        

                        while (sdr.Read())
                        {

                            if (!sdr["HAB1"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB1"].ToString());
                            }
                            if (!sdr["HAB2"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB2"].ToString());
                            }
                            if (!sdr["HAB3"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB3"].ToString());
                            }
                            if (!sdr["HAB4"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB4"].ToString());
                            }
                            if (!sdr["HAB5"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB5"].ToString());
                            }

                        }


                        con.Close();
                    }
                }
                foreach(var habilidade in Habs)
                {
                    String Habilidade = habilidade;
                    Hab.Descricao = Habilidade;
                    HabilidadeLista.Add(Hab);
                    Hab = new HabilidadeModel();
                }

                using (SqlConnection con2 = new SqlConnection(strCon))
                {
                    query = "SPC_LISTA_ALUNOS_GRUPO";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con2;
                        con2.Open();
                        cmd.Parameters.Add("@MATRICULA_ALUNO", SqlDbType.VarChar).Value = Matricula;
                        cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {


                            while (sdr.Read())
                            {
                                UsuarioModel Usuario = new UsuarioModel();
                                Usuario.Nome = sdr["NOME"].ToString();
                                Usuario.Matricula = sdr["MATRICULA"].ToString();
                                Usuario.imgPath = sdr["PROFILEPIC"].ToString();
                                Usuario.Habilidades = HabilidadeLista;
                                Alunos.Add(Usuario);

                            }



                            con.Close();
                        }
                    }
                    
                }


            }

            return Alunos;

        }
        public void ParticiparGrupo(int idg,String Matricula,String CD_Atividade)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_ALUNO_GRUPO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
            cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = idg;
            cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;



            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();

            }
        }
        public String GetAtivo(String matricula,String CD_ATIVIDADE)
        {
            String ativo = "2";
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_ALUNO_ATIVO_ATIVIDADE";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = matricula;
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_ATIVIDADE;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            ativo = sdr["CD_STATUS"].ToString();

                        }



                        con.Close();
                    }
                }

            }
            return ativo;
        }
        public void FinalizarAtividade(String CD_Atividade)
        {
           // string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPA_ENCERRAR_ATIVIDADE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();

            }
        }
        public Boolean Cadastrar(CadastroModel cadastro)
        {
            CRUDDAO CadastroDAO = new CRUDDAO();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_CADASTRO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LOGIN", SqlDbType.VarChar).Value = cadastro.Login;
            cmd.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = cadastro.Senha;
            cmd.Parameters.Add("@NOME", SqlDbType.VarChar).Value = cadastro.Nome;
            cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = cadastro.Matricula;
            cmd.Parameters.Add("@CD_HABILIDADE", SqlDbType.VarChar).Value = cadastro.CD_Habilidade;
            cmd.Parameters.Add("@CD_HABILIDADE2", SqlDbType.VarChar).Value = cadastro.CD_Habilidade2;
            cmd.Parameters.Add("@ProfilePic", SqlDbType.VarChar).Value = cadastro.ImagePath;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return true;

            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();
                cadastro.Habilidades = CadastroDAO.populateHabilidades();
                return false;


            }
        }
        public CadastroModel EditarGET(CadastroModel cadastro)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPC_EDITUSUARIO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = cadastro.Matricula;


            con.Open();
            try
            {
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {

                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            cadastro.Nome = sdr["NOME"].ToString();
                            cadastro.Matricula = sdr["MATRICULA"].ToString();
                            cadastro.ImagePath = sdr["PROFILEPIC"].ToString();
                        }
                        con.Close();
                        return cadastro;

                    }
                }
            }
            catch (SqlException e)
            {
                String[] LoginErro = new String[] { e.Message };
                con.Close();
                return cadastro;

            }
            return cadastro;
        }
        public void InsertAtividadeMateriaTurma(AtividadeModel atividade)
        {
            CRUDDAO CadastroDAO = new CRUDDAO();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_ATIVIDADE_MATERIA_TURMA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = atividade.CD_Turma;
            cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = atividade.CD_Materia;


            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                CadastroDAO.SETAlunoAtividade(atividade);
                //return true;

            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();
                // cadastro.Habilidades = CadastroDAO.populateHabilidades();
                //return false;


            }
        }
        public List<GrupoModel>  getGrupos(String CD_Atividade)
        {
            List<GrupoModel> Grupos = new List<GrupoModel>();
            List<UsuarioModel> Usuarios = new List<UsuarioModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_LISTA_GRUPOS";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        GrupoModel Grupo = new GrupoModel();
                        var Antid = 0;
                        while (sdr.Read())
                        {
                            
                            var GID = sdr["CD_GRUPO"].ToString();
                            if (Antid != 0){
                                if (Antid != Convert.ToInt32(GID))
                                {
                                    Grupos.Add(Grupo);
                                    Usuarios = new List<UsuarioModel>();
                                    Grupo = new GrupoModel();
                                }
                            }
                           
                            
                            Grupo.Nome = sdr["NOME_GRUPO"].ToString();
                                    Grupo.id = sdr["CD_GRUPO"].ToString();
                                    UsuarioModel usuario = new UsuarioModel();
                                    usuario.Nome = sdr["NOME"].ToString();
                                    usuario.CD_aluno = Convert.ToInt32((sdr["CD_ALUNO"].ToString()));
                                    usuario.imgPath = sdr["PROFILEPIC"].ToString();
                                    Usuarios.Add(usuario);
                                Grupo.id = sdr["CD_GRUPO"].ToString();
                            Grupo.Alunos = Usuarios;
                            
                            Antid=  Convert.ToInt32(sdr["CD_GRUPO"].ToString());


                        }
                        Grupos.Add(Grupo);

                        con.Close();
                    }
                }

            }
            return Grupos;
        }
        public AtividadeModel AtividadeDetalhes(String CD_Atividade)
        {
            AtividadeModel AM = new AtividadeModel();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_ATIVIDADE_DETALHES";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        List<String> Habs = new List<String>();

                        while (sdr.Read())
                        {

                            AM.CD_Atividade = sdr["CD_ATIVIDADE"].ToString();
                            AM.Descricao = sdr["DESCRICAO"].ToString();
                            AM.CD_Status = sdr["CD_STATUS"].ToString();
                            /*Habs.Add(sdr["HAB1"].ToString());
                            Habs.Add(sdr["HAB2"].ToString());
                            Habs.Add(sdr["HAB3"].ToString());
                            Habs.Add(sdr["HAB4"].ToString());
                            Habs.Add(sdr["HAB5"].ToString());*/
                            if (!sdr["HAB1"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB1"].ToString());
                            }
                            if (!sdr["HAB2"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB2"].ToString());
                            }
                            if (!sdr["HAB3"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB3"].ToString());
                            }
                            if (!sdr["HAB4"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB4"].ToString());
                            }
                            if (!sdr["HAB5"].ToString().Equals(""))
                            {
                                Habs.Add(sdr["HAB5"].ToString());
                            }

                        }
                        AM.Habilidades = Habs;


                        con.Close();
                    }
                }

            }
           
            return AM;
        }
        public List<UsuarioModel> ListaAlunoAtividade(String CD_Turma,String CD_Materia)
        {
            List<UsuarioModel> Usuarios = new List<UsuarioModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_LISTA_ALUNO_ATIVIDADE";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = CD_Turma;
                    cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = CD_Materia;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            UsuarioModel UM = new UsuarioModel();
                            UM.Nome = sdr["NOME"].ToString();
                            UM.Matricula = sdr["MATRICULA"].ToString();
                            UM.imgPath= sdr["PROFILEPIC"].ToString();
                            Usuarios.Add(UM);

                        }



                        con.Close();
                    }
                }

            }
            return Usuarios;
        }
        /*public bool GetAvaliou()
        {

        }*/
        public List<AtividadeModel> SelectAtividade(String IDT,String IDM) {
            List<AtividadeModel> Atividades = new List<AtividadeModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_ATIVIDADE_MATERIA_TURMA";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = IDM;
                    cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = IDT;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            AtividadeModel AM = new AtividadeModel();
                            AM.CD_Atividade = sdr["CD_ATIVIDADE"].ToString();
                            AM.Descricao = sdr["DESCRICAO"].ToString();
                            Atividades.Add(AM);

                        }



                        con.Close();
                    }
                }

            }

            return Atividades;

        }
        public void InsertAtividade(AtividadeModel atividade)
        {
            CRUDDAO CadastroDAO = new CRUDDAO();
           // string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_ATIVIDADE", con);
            String[] Habilidades = atividade.Habilidade.Split(",");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = atividade.CD_Turma;
            cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = atividade.CD_Materia;
            cmd.Parameters.Add("@NOTA", SqlDbType.VarChar).Value = atividade.Nota;
            cmd.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = atividade.Descricao;
            for(var i = 0; i <Habilidades.Length; i++)
            {
                cmd.Parameters.Add("@HAB"+(i+1), SqlDbType.VarChar).Value = Habilidades[i];
            }




            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                CadastroDAO.InsertAtividadeMateriaTurma(atividade);
                for (var i = 0; i < Habilidades.Length; i++)
                {
                    CadastroDAO.InsertHabilidade(Habilidades[i]);
                }
                //return true;

            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();
                // cadastro.Habilidades = CadastroDAO.populateHabilidades();
                //return false;


            }
           
        }
        public void InsertHabilidade(String Habilidade)
        {
            CRUDDAO CadastroDAO = new CRUDDAO();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPX_HABILIDADE_ATIVIDADE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = Habilidade;




            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //return true;

            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();
                // cadastro.Habilidades = CadastroDAO.populateHabilidades();
                //return false;


            }
        }
        public void InsertAlunoTurma(String Matricula,int cd_turma,int cd_materia)
        {
            CRUDDAO CadastroDAO = new CRUDDAO();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_ALUNO_TURMA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = cd_turma;
            cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = cd_materia;
            cmd.Parameters.Add("@Matricula", SqlDbType.VarChar).Value = Matricula;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                //return true;

            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();
                // cadastro.Habilidades = CadastroDAO.populateHabilidades();
                //return false;


            }
            CRUDDAO Inserir = new CRUDDAO();
            Inserir.SETNovoAlunoAtividade(Matricula, cd_turma, cd_materia);
        }
        public void SETNovoAlunoAtividade(String Matricula, int cd_turma, int cd_materia)
        {
            CRUDDAO CadastroDAO = new CRUDDAO();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPX_INSERE_ALUNO_ATIVIDADE_ALUNO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = cd_turma;
            cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = cd_materia;
            cmd.Parameters.Add("@Matricula", SqlDbType.VarChar).Value = Matricula;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                //return true;

            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();
                // cadastro.Habilidades = CadastroDAO.populateHabilidades();
                //return false;


            }
        }
        public void SETAlunoAtividade(AtividadeModel atividade)
        {
            CRUDDAO CadastroDAO = new CRUDDAO();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
           SqlCommand cmd = new SqlCommand("SPX_INSERE_ALUNO_ATIVIDADE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = atividade.CD_Turma;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                //return true;

            }
            catch (SqlException e)
            {
                // ViewBag.Error = string.Join("", e.Message);
                Console.WriteLine(e.Message);
                con.Close();
                // cadastro.Habilidades = CadastroDAO.populateHabilidades();
                //return false;


            }
        }
        public int AlunoTemGrupo(String Matricula, String CD_Atividade)
        {
            var sdrcount = 0;
            var teste = Matricula;
            var teste2 = CD_Atividade;
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_ALUNO_GRUPO";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA_ALUNO", SqlDbType.VarChar).Value = Matricula;
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            sdrcount++;

                        }



                        con.Close();
                    }
                }
                return sdrcount;
            }

        }
        public List<UsuarioModel> GetAlunosGrupo(String Matricula, String CD_Atividade)
        {
            List<UsuarioModel> Alunos = new List<UsuarioModel>();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_LISTA_ALUNOS_GRUPO";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA_ALUNO", SqlDbType.VarChar).Value = Matricula;
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            UsuarioModel Usuario = new UsuarioModel();
                            Usuario.Nome = sdr["NOME"].ToString();
                            Usuario.Matricula = sdr["MATRICULA"].ToString();
                            Usuario.imgPath = sdr["PROFILEPIC"].ToString();
                            Alunos.Add(Usuario);

                        }



                        con.Close();
                    }
                }
                return Alunos;
            }
        }
        public int GetCD_GRUPO(String Matricula, String CD_Atividade)
        {
            int CD_GRUPO = 0;
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_CDGRUPO";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA_ALUNO", SqlDbType.VarChar).Value = Matricula;
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                           
                            CD_GRUPO = Convert.ToInt32(sdr["CD_GRUPO"].ToString());

                        }



                        con.Close();
                    }
                }
                return CD_GRUPO;
            }
        }
        public List<TurmaModel> populateTurma(String matricula)
        {
            List<TurmaModel> turmas = new List<TurmaModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_TURMA_MATERIA";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = matricula;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            TurmaModel TM = new TurmaModel();
                            TM.CD_Materia = sdr["CD_MATERIA"].ToString();
                            TM.CD_Turma = sdr["CD_TURMA"].ToString();
                            TM.Nome_Turma = sdr["NOME_TURMA"].ToString();
                            TM.Nome_Materia = sdr["NOME_Materia"].ToString();
                            turmas.Add(TM);

                        }



                        con.Close();
                    }
                }

            }

            return turmas;
        }
        public List<HabilidadeModel> populateHabilidades()
        {
            List<HabilidadeModel> items = new List<HabilidadeModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = " SELECT CD_HABILIDADE, DESCRICAO FROM HABILIDADE";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            HabilidadeModel TM = new HabilidadeModel();
                            TM.CD_Habilidade = Convert.ToInt32(sdr["CD_HABILIDADE"].ToString());
                            TM.Descricao = sdr["DESCRICAO"].ToString();
                            items.Add(TM);
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }
        public void SETLINK(String Link, int CD_GRUPO)
        {
           // string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPI_TRAB_GRUPO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LINK", SqlDbType.VarChar).Value = Link;
            cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = CD_GRUPO;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();




            }
            catch (SqlException e)
            {

                con.Close();
                //cadastro.Habilidades = populateHabilidades();



            }
        }
        public List<UsuarioModel> GETSOLICITACAO(int CD_GRUPO)
        {
            List<UsuarioModel> USRPendente = new List<UsuarioModel>();
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_SOLICITAO_PENDENTE";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = CD_GRUPO;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            UsuarioModel Usuario = new UsuarioModel();
                            Usuario.Nome = sdr["NOME"].ToString();
                            Usuario.Matricula = sdr["MATRICULA"].ToString();
                            Usuario.imgPath = sdr["PROFILEPIC"].ToString();
                            Usuario.CD_aluno = Convert.ToInt32(sdr["CD_ALUNO"].ToString());
                            USRPendente.Add(Usuario);

                        }



                        con.Close();
                    }
                }
                return USRPendente;
            }
            return USRPendente;
        }
        public CadastroModel EditarPOST(CadastroModel cadastro)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand("SPA_USUARIO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@NOME", SqlDbType.VarChar).Value = cadastro.Nome;
            cmd.Parameters.Add("@CD_HABILIDADE", SqlDbType.VarChar).Value = cadastro.CD_Habilidade;
            cmd.Parameters.Add("@CD_HABILIDADE2", SqlDbType.VarChar).Value = cadastro.CD_Habilidade2;
            if (cadastro.image == null)
            {
                cmd.Parameters.Add("@ProfilePic", SqlDbType.VarChar).Value = "null.jpg";
            }
            else
                cmd.Parameters.Add("@ProfilePic", SqlDbType.VarChar).Value = cadastro.ImagePath;
            cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = cadastro.Matricula;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                

                return cadastro;

            }
            catch (SqlException e)
            {
               
                con.Close();
                //cadastro.Habilidades = populateHabilidades();
                return cadastro;


            }
        }
        
        public Boolean ESquadMaster(String Matricula,int CD_GRUPO)
        {
            var SquadMaster = false;
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_E_SQUADMASTER";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
                    cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = CD_GRUPO;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {

                            SquadMaster = Convert.ToBoolean(sdr["SQUAD_MASTER"].ToString());

                        }



                        con.Close();
                    }
                }
                return SquadMaster;
            }
        }

        public List<GrupoModel> getGruposAvaliar(String CD_Atividade)
        {
            List<GrupoModel> Grupos = new List<GrupoModel>();
            List<UsuarioModel> Usuarios = new List<UsuarioModel>();
            //string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SPC_LISTA_GRUPOS";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_ATIVIDADE", SqlDbType.VarChar).Value = CD_Atividade;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        GrupoModel Grupo = new GrupoModel();
                        var Antid = 0;
                        while (sdr.Read())
                        {

                            var GID= sdr["CD_GRUPO"].ToString();
                            if (Antid != 0)
                            {
                                if (Antid != Convert.ToInt32(GID))
                                {
                                    Grupos.Add(Grupo);
                                    Usuarios = new List<UsuarioModel>();
                                    Grupo = new GrupoModel();
                                }
                            }


                            Grupo.Nome = sdr["NOME_GRUPO"].ToString();
                            Grupo.id = sdr["CD_GRUPO"].ToString();
                            if(sdr["NOTA"] !=DBNull.Value)
                            {
                                Grupo.Nota = Convert.ToDouble(sdr["NOTA"]);
                            }
                         
                            Grupo.LinkAtividade = sdr["LINK_TAREFA"].ToString();
                            UsuarioModel usuario = new UsuarioModel();
                            usuario.Nome = sdr["NOME"].ToString();
                            usuario.CD_aluno = Convert.ToInt32((sdr["CD_ALUNO"].ToString()));
                            usuario.imgPath = sdr["PROFILEPIC"].ToString();
                            Usuarios.Add(usuario);
                            //Grupo.id = sdr["CD_GRUPO"].ToString();
                            Grupo.Alunos = Usuarios;

                            Antid = Convert.ToInt32(sdr["CD_GRUPO"].ToString());


                        }
                        Grupos.Add(Grupo);

                        con.Close();
                    }
                }

            }
            return Grupos;
        }
        public string GetLinkAtividade(int cd_grupo)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string link = "";
                string query = "SPC_LINK_ATIVIDADE";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    cmd.Parameters.Add("@CD_GRUPO", SqlDbType.VarChar).Value = cd_grupo;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                       
                                link = sdr["LINK_TAREFA"].ToString();

                            




                        }

                    }
                    con.Close();
                    return link;
                }
            }
        }

        public double GetMediaAlunoTurma(String Matricula, String IDT, String IDM)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                double teste = 0;
                string query = "SPC_MEDIA_GRUPOS";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                  
                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = Matricula;
                    cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = IDT;
                    cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = IDM;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            if (sdr["MEDIA"] != DBNull.Value)
                            {
                                teste = Convert.ToDouble(sdr["MEDIA"].ToString());

                            }




                        }

                    }
                    con.Close();
                    return teste;
                }
            }

        }
        public double GetMediaTurma(String IDT, String IDM)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                double teste = 0;
                string query = "SPC_MEDIA_GRUPOS";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@CD_TURMA", SqlDbType.VarChar).Value = IDT;
                    cmd.Parameters.Add("@CD_MATERIA", SqlDbType.VarChar).Value = IDM;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {if((sdr["MEDIA"] != DBNull.Value)){ teste = Convert.ToDouble(sdr["MEDIA"].ToString()); }
                            

                        }



                        con.Close();
                    }
                }
                return teste;
            }
        }

        public double GetScoreAluno(String matricula)
        {
            //string strCon = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                double teste = 0;
                string query = "SPC_SCORE_ALUNO";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.Add("@MATRICULA", SqlDbType.VarChar).Value = matricula;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {


                        while (sdr.Read())
                        {
                            if (sdr["MEDIA2"] != DBNull.Value) { teste = Convert.ToDouble(sdr["MEDIA2"].ToString()); }
                            

                        }



                        con.Close();
                    }
                }
                return teste;
            }
        }

    }

}
