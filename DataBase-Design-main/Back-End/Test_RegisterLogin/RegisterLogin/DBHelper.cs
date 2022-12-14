using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using RegisterLogin.Models;

namespace RegisterLogin
{
    public class DBHelper
    {
        public const int ID_LEN = 10;
        public static string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=139.196.222.196)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=c##ysjyyds;Password=DBprinciple2022;";
        OracleConnection con = new OracleConnection(connString);
        public void openConn()
        {
            try
            {
                con.Open();
                Console.WriteLine("Successfully connected to Oracle Database");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection Failed!");
                Console.WriteLine(e);
            }
        }

        public string calcID(int tot_num)
        {
            string id = (tot_num + 1).ToString();
            string zeros = "0000000000";
            string id_str = zeros.Substring(0, ID_LEN - id.Length) + id;

            return id_str;
        }

        public LoginResponse checkLogin(LoginRequest req)
        {
            openConn();
            LoginResponse resp = new LoginResponse();
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT PASSWORD FROM GAME_USER WHERE EMAIL = '" + req.email + "'";
            OracleDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                resp.result = -1;       //邮箱不存在
            else
            {
                while (reader.Read())
                {
                    string pwd = reader[0].ToString();
                    if (pwd == req.password)
                    {
                        resp.result = 0;    //成功登录
                        break;
                    }
                    else
                    {
                        resp.result = 1;    //密码错误
                        break;
                    }
                }
            }

            return resp;
        }

        public RegisterResponse register(RegisterRequest req)
        {
            openConn();
            RegisterResponse resp = new RegisterResponse();
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT NAME FROM GAME_USER WHERE EMAIL = '" + req.email + "'";
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                resp.result = 1;
                resp.message = "该email已绑定账号！";
                return resp;
            }

            try
            {
                string id = "";
                cmd.CommandText = "SELECT COUNT(ID) FROM GAME_USER";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = calcID(int.Parse(reader[0].ToString()));
                    break;
                }

                cmd.CommandText = String.Format("SELECT ID FROM GAME_USER WHERE NAME = '{0}'", req.username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    resp.result = -1;
                    resp.message = "用户名已存在";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO GAME_USER VALUES('" + id + "', '', '" + req.password + "', '" + req.username + "', 1, 0, '" + req.email + "', " +
                                "'', '', '', '" + req.area + "', '" + req.time + "')";
                    cmd.ExecuteNonQuery();
                    resp.result = 0;
                    resp.message = "注册成功！";
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                resp.result = 2;
                resp.message = "注册失败！";
            }

            return resp;
        }
    }
}
