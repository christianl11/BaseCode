using BaseCode.Models.Requests;
using BaseCode.Models.Responses;
using BaseCode.Models.Tables;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BaseCode.Models
{
    public class DBContext
    {
        public string ConnectionString { get; set; }
        public DBContext(string connStr)
        {
            this.ConnectionString = connStr;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public GenericInsertUpdateResponse InsertUpdateData(GenericInsertUpdateRequest r)
        {
            GenericInsertUpdateResponse resp = new GenericInsertUpdateResponse();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlTransaction myTrans;
                    myTrans = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand(r.query, conn);
                    cmd.ExecuteNonQuery();

                    resp.Id = r.isInsert ? int.Parse(cmd.LastInsertedId.ToString()) : -1;
                    myTrans.Commit();
                    conn.Close();
                    resp.isSuccess = true;
                    resp.Message = r.responseMessage;
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.Message = r.errorMessage + ": " + ex.Message;
            }
            return resp;
        }
        public CreateUserTypeResponse CreateUserTypes(CreateUserTypeRequest r)
        {
            CreateUserTypeResponse resp = new CreateUserTypeResponse();

            string query = " INSERT INTO USER_TYPES (USER_TYPE_DESCRIPTION, STATUS)";
            query += "VALUES ('" + r.UserTypeDesc + "','" + r.Status + "')";

            GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

            genReq.query = query;
            genReq.isInsert = true;
            genReq.responseMessage = " Successfully created user types.";
            genReq.errorMessage = " Unable to create user";

            GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
            genResp = InsertUpdateData(genReq);

            resp.Message = genResp.Message;
            resp.isSuccess = genResp.isSuccess;
            resp.UserTypeId = genResp.Id;

            return resp;
        }
        public CreateUserTypeResponse UpdateUserType(CreateUserTypeRequest r)
        {
            CreateUserTypeResponse resp = new CreateUserTypeResponse();

            DateTime theDate = DateTime.Now;
            string crtdt = theDate.ToString("yyyy-MM-dd H:mm:ss");

            string query = "UPDATE USER_TYPES SET ";

            query += !string.IsNullOrEmpty(r.UserTypeDesc) ? " USER_TYPE_DESCRIPTION = '" + r.UserTypeDesc + "'," : "";
            query += !string.IsNullOrEmpty(r.Status) ? " STATUS = '" + r.Status + "'," : "";
            query += " UPDATE_DATE = '" + crtdt + "' WHERE USER_TYPE_ID = " + r.UserTypeId;


            GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

            genReq.query = query;
            genReq.isInsert = false;
            genReq.responseMessage = " Successfully updated user types.";
            genReq.errorMessage = " Unable to update user";

            GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
            genResp = InsertUpdateData(genReq);

            resp.Message = genResp.Message;
            resp.isSuccess = genResp.isSuccess;
            resp.UserTypeId = genResp.Id;

            return resp;
        }
        public CreateUserTypeResponse Delete(CreateUserTypeRequest r)
        {
            CreateUserTypeResponse resp = new CreateUserTypeResponse();

            DateTime theDate = DateTime.Now;
            string crtdt = theDate.ToString("yyyy-MM-dd H:mm:ss");
            string query = "UPDATE USER_TYPES SET ";
            query += !string.IsNullOrEmpty(r.Status) ? " STATUS = '" + r.Status + "'," : "";
            query += " UPDATE_DATE = '" + crtdt + "', STATUS = 'D'"+ r.Status + "  WHERE USER_TYPE_ID = " + r.UserTypeId;


            GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

            genReq.query = query;
            genReq.isInsert = false;
            genReq.responseMessage = " Successfully deleted user types.";
            genReq.errorMessage = " Unable to update user";

            GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
            genResp = InsertUpdateData(genReq);

            resp.Message = genResp.Message;
            resp.isSuccess = genResp.isSuccess;
            resp.UserTypeId = genResp.Id;

            return resp;
        }
        public GetUserTypeListResponse GetUserTypeList()
        {
            GetUserTypeListResponse resp = new GetUserTypeListResponse();
            resp.UsersTypeList = new List<UserTypes>();
            UserTypes u;
            string query = "SELECT * FROM USER_TYPES WHERE STATUS = 'A'";
            GenericGetDataResponse getData = new GenericGetDataResponse();
            getData.Data = new DataTable();

            getData = GetData(query);

            if (getData.Data.Rows.Count > 0)
            {
                foreach (DataRow dr in getData.Data.Rows)
                {
                    u = new UserTypes();
                    u.UserTypeId = int.Parse(dr["USER_TYPE_ID"].ToString());
                    u.UserTypeDesc = dr["USER_TYPE_DESCRIPTION"].ToString();
                    u.Status = dr["STATUS"].ToString() == "A" ? "ACTIVE" : "INACTIVE";
                    resp.UsersTypeList.Add(u);
                }
                resp.isSuccess = true;
                resp.Message = "List Of Users";
                return resp;
            }
            else
            {
                resp.isSuccess = true;
                resp.Message = " No users found.";
                return resp;
            }


        
    }
        public CreateUserResponse CreateUserUsingSqlScript(CreateUserRequest r)
        {
            CreateUserResponse resp = new CreateUserResponse();
            DataTable dt;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    dt = new DataTable();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT USER_NAME FROM USER WHERE USER_NAME ='" + (r.UserName.ToString()) + "'", conn);
                    MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                    sd.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        resp.isSuccess = false;
                        resp.Message = "User username already exist! Unable to create User";
                        return resp;
                    }
                    else
                    {
                        string query = " INSERT INTO USER (FIRST_NAME, LAST_NAME, USER_NAME, PASSWORD, EMAIL_ADDRESS, USER_TYPE_ID )";
                        query += "VALUES ('" + r.FirstName + "','" + r.LastName + "','" + r.UserName + "','" + r.Password + "','" + r.EmailAddress + "','" + r.UserTypeId + "')";

                        GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

                        genReq.query = query;
                        genReq.isInsert = true;
                        genReq.responseMessage = " Successfully created user.";
                        genReq.errorMessage = " Unable to create user";

                        GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
                        genResp = InsertUpdateData(genReq);

                        resp.Message = genResp.Message;
                        resp.isSuccess = genResp.isSuccess;
                        resp.UserId = genResp.Id;

                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public CreateUserResponse UpdateUser(CreateUserRequest r)
        {
            CreateUserResponse resp = new CreateUserResponse();
            DataTable dt;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    dt = new DataTable();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT USER_NAME FROM USER WHERE USER_NAME ='" + (r.UserName.ToString()) + "'", conn);
                    MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                    sd.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        resp.isSuccess = false;
                        resp.Message = "Username already exist! Unable to update User";
                        return resp;
                    }
                    else
                    {
                        DateTime theDate = DateTime.Now;
                        string crtdt = theDate.ToString("yyyy-MM-dd H:mm:ss");

                        string query = "UPDATE USER SET ";
                        query += !string.IsNullOrEmpty(r.FirstName) ? " FIRST_NAME = '" + r.FirstName + "'," : "";
                        query += !string.IsNullOrEmpty(r.LastName) ? " LAST_NAME = '" + r.LastName + "'," : "";
                        query += !string.IsNullOrEmpty(r.UserName) ? " USER_NAME = '" + r.UserName + "'," : "";
                        query += !string.IsNullOrEmpty(r.Password) ? " PASSWORD = '" + r.Password + "'," : "";
                        query += !string.IsNullOrEmpty(r.EmailAddress) ? "EMAIL_ADDRESS = '" + r.EmailAddress + "'," : "";
                        query += !string.IsNullOrEmpty(r.UserTypeId.ToString()) ? "USER_TYPE_ID = '" + r.UserTypeId + "'," : "";
                        query += " UPDATE_DATE = '" + crtdt + "' WHERE USER_ID = " + r.UserId;


                        GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

                        genReq.query = query;
                        genReq.isInsert = false;
                        genReq.responseMessage = " Successfully updated user.";
                        genReq.errorMessage = " Unable to update user";

                        GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
                        genResp = InsertUpdateData(genReq);

                        resp.Message = genResp.Message;
                        resp.isSuccess = genResp.isSuccess;
                        resp.UserId = genResp.Id;

                        return resp;
                    }

                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.Message = ex.Message;
            }
            return resp;
            
        }
        public GetUserListResponse GetUserList()
        {
            GetUserListResponse resp = new GetUserListResponse();
            resp.UsersList = new List<User>();
            User u;
            string query = "SELECT p.USER_ID,p.USER_TYPE_ID, p.FIRST_NAME, p.LAST_NAME, p.USER_NAME, p.PASSWORD, p.STATUS, p.EMAIL_ADDRESS, b.USER_TYPE_DESCRIPTION FROM USER AS p INNER JOIN USER_TYPES AS b ON  b.USER_TYPE_ID=p.USER_TYPE_ID";
            GenericGetDataResponse getData = new GenericGetDataResponse();
            getData.Data = new DataTable();

            getData = GetData(query);

            if (getData.Data.Rows.Count > 0)
            {
                foreach (DataRow dr in getData.Data.Rows)
                {
                    u = new User();
                    u.UserId = int.Parse(dr["USER_ID"].ToString());
                    u.FirstName = dr["FIRST_NAME"].ToString();
                    u.LastName = dr["LAST_NAME"].ToString();
                    u.UserName = dr["USER_NAME"].ToString();
                    u.Status = dr["STATUS"].ToString() == "A" ? "ACTIVE" : "INACTIVE";
                    u.EmailAddress = dr["EMAIL_ADDRESS"].ToString();
                    u.UserTypeId = dr["USER_TYPE_ID"].ToString();
                    u.password = dr["PASSWORD"].ToString();
                    u.UserTypeDesc = dr["USER_TYPE_DESCRIPTION"].ToString();
                    resp.UsersList.Add(u);
                }
                resp.isSuccess = true;
                resp.Message = "List Of Users";
                return resp;
            }
            else
            {
                resp.isSuccess = true;
                resp.Message = " No users found.";
                return resp;
            }


        }
        public CreateModuleTypeResponse CreateModuleType(CreateModuleTypeAccessRequest r)
        {
            CreateModuleTypeResponse resp = new CreateModuleTypeResponse();
            string query = " INSERT INTO USER_TYPE_MODULE_ACCESS (MODULE_ID, USER_TYPE_ID )";
            query += "VALUES ('" + r.ModuleID + "','" + r.UserTypeID + "')";

            GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

            genReq.query = query;
            genReq.isInsert = true;
            genReq.responseMessage = " Successfully created USER TYPE MODULE ACCESS.";
            genReq.errorMessage = " Unable to create USER TYPE MODULE ACCESS";

            GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
            genResp = InsertUpdateData(genReq);

            resp.Message = genResp.Message;
            resp.isSuccess = genResp.isSuccess;
            resp.UTMAID = genResp.Id;

            return resp;
        }
        public CreateModuleTypeResponse UpdateModuleType(CreateModuleTypeAccessRequest r)
        {
            CreateModuleTypeResponse resp = new CreateModuleTypeResponse();

            DateTime theDate = DateTime.Now;
            string crtdt = theDate.ToString("yyyy-MM-dd H:mm:ss");
            string query = "UPDATE USER_TYPE_MODULE_ACCESS SET ";
            query += !string.IsNullOrEmpty(r.UserTypeID.ToString()) ? " USER_TYPE_ID = '" + r.UserTypeID + "'," : "";
            query += !string.IsNullOrEmpty(r.ModuleID.ToString()) ? " MODULE_ID = '" + r.ModuleID + "'," : "";
            query += !string.IsNullOrEmpty(r.Status) ? " STATUS = '" + r.Status + "'," : "";
            query += " STATUS = '" + r.Status + "'  WHERE UTMA_ID = " + r.UtmaID;
            GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

            genReq.query = query;
            genReq.isInsert = false;
            genReq.responseMessage = " Successfully updated USER TYPE MODULE ACCESS.";
            genReq.errorMessage = " Unable to update USER TYPE MODULE ACCESS";

            GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
            genResp = InsertUpdateData(genReq);

            resp.Message = genResp.Message;
            resp.isSuccess = genResp.isSuccess;
            resp.UTMAID = genResp.Id;

            return resp;
        }
        public CreateModuleTypeResponse DeleteModuleType(CreateModuleTypeAccessRequest r)
        {
            CreateModuleTypeResponse resp = new CreateModuleTypeResponse();

            DateTime theDate = DateTime.Now;
            string crtdt = theDate.ToString("yyyy-MM-dd H:mm:ss");
            string query = "UPDATE USER_TYPE_MODULE_ACCESS SET ";
            query += !string.IsNullOrEmpty(r.Status) ? " STATUS = '" + r.Status + "'," : "";
            query += " STATUS = 'D'  WHERE UTMA_ID = " + r.UtmaID;
            GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

            genReq.query = query;
            genReq.isInsert = false;
            genReq.responseMessage = " Successfully deleted USER TYPE MODULE ACCESS.";
            genReq.errorMessage = " Unable to update USER TYPE MODULE ACCESS";

            GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
            genResp = InsertUpdateData(genReq);

            resp.Message = genResp.Message;
            resp.isSuccess = genResp.isSuccess;
            resp.UTMAID = genResp.Id;

            return resp;
        }
        public GetModuleTypeListResponse GetModuleListType()
        {
            GetModuleTypeListResponse resp = new GetModuleTypeListResponse();
            resp.ModuleTypes = new List<ModuleType>();
            ModuleType u;
            string query = "SELECT * FROM USER_TYPE_MODULE_ACCESS WHERE STATUS = 'A'";
            GenericGetDataResponse getData = new GenericGetDataResponse();
            getData.Data = new DataTable();

            getData = GetData(query);

            if (getData.Data.Rows.Count > 0)
            {
                foreach (DataRow dr in getData.Data.Rows)
                {
                    u = new ModuleType();
                    u.UtmaID = int.Parse(dr["UTMA_ID"].ToString());
                    u.ModuleID = int.Parse(dr["MODULE_ID"].ToString());
                    u.User_Type_ID = int.Parse(dr["USER_TYPE_ID"].ToString());
                    u.Status = dr["STATUS"].ToString() == "A" ? "ACTIVE" : "INACTIVE";
                    resp.ModuleTypes.Add(u);
                }
                resp.isSuccess = true;
                resp.Message = "List Of Users";
                return resp;
            }
            else
            {
                resp.isSuccess = true;
                resp.Message = " No users found.";
                return resp;
            }



        }

        public CreateModuleResponse CreateModule(CreateModuleRequest r)
        {
            CreateModuleResponse resp = new CreateModuleResponse();
            DataTable dt;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    dt = new DataTable();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT NAME FROM MODULE WHERE NAME ='" + (r.Name.ToString()) + "'", conn);
                    MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                    sd.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        resp.isSuccess = false;
                        resp.Message = "Module name already exist! Unable to create Module";
                        return resp;
                    }
                    else { 
                    string query = " INSERT INTO MODULE (NAME, DESCRIPTION )";
                    query += "VALUES ('" + r.Name + "','" + r.Description + "')";
                    GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

                    genReq.query = query;
                    genReq.isInsert = true;
                    genReq.responseMessage = " Successfully created user.";
                    genReq.errorMessage = " Unable to create user";

                    GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
                    genResp = InsertUpdateData(genReq);

                    resp.Message = genResp.Message;
                    resp.isSuccess = genResp.isSuccess;
                    resp.Moduleid = genResp.Id;

                    return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
        public CreateModuleResponse UpdateModule(CreateModuleRequest r)
        {
            CreateModuleResponse resp = new CreateModuleResponse();
            DataTable dt;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    dt = new DataTable();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT NAME FROM MODULE WHERE NAME ='" + (r.Name.ToString()) + "'", conn);
                    MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                    sd.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        resp.isSuccess = false;
                        resp.Message = "Module name already exist! Unable to update Module";
                        return resp;
                    }
                    else
                    {
                    DateTime theDate = DateTime.Now;
                    string crtdt = theDate.ToString("yyyy-MM-dd H:mm:ss");

                    string query = "UPDATE MODULE SET ";
                    query += !string.IsNullOrEmpty(r.Name) ? " NAME = '" + r.Name + "'," : "";
                    query += !string.IsNullOrEmpty(r.Description) ? " DESCRIPTION = '" + r.Description + "'," : "";
                    query += !string.IsNullOrEmpty(r.Status) ? " STATUS = '" + r.Status + "'," : "";

                    query += " MODULE_UPDATE_DATE = '" + crtdt + "' WHERE MODULE_ID = " + r.ModuleID;


                    GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

                    genReq.query = query;
                    genReq.isInsert = false;
                    genReq.responseMessage = " Successfully updated Module.";
                    genReq.errorMessage = " Unable to update Module";

                    GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
                    genResp = InsertUpdateData(genReq);

                    resp.Message = genResp.Message;
                    resp.isSuccess = genResp.isSuccess;
                    resp.Moduleid = genResp.Id;

                    return resp;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public CreateModuleResponse DeleteModule(CreateModuleRequest r)
        {
            CreateModuleResponse resp = new CreateModuleResponse();

            DateTime theDate = DateTime.Now;
            string crtdt = theDate.ToString("yyyy-MM-dd H:mm:ss");
            string query = "UPDATE MODULE SET ";
            query += !string.IsNullOrEmpty(r.Status) ? " STATUS = '" + r.Status + "'," : "";
            query += " MODULE_UPDATE_DATE = '" + crtdt + "', STATUS = 'D'" + r.Status + "  WHERE MODULE_ID = " + r.ModuleID;


            GenericInsertUpdateRequest genReq = new GenericInsertUpdateRequest();

            genReq.query = query;
            genReq.isInsert = false;
            genReq.responseMessage = " Successfully deleted user types.";
            genReq.errorMessage = " Unable to update user";

            GenericInsertUpdateResponse genResp = new GenericInsertUpdateResponse();
            genResp = InsertUpdateData(genReq);

            resp.Message = genResp.Message;
            resp.isSuccess = genResp.isSuccess;
            resp.Moduleid = genResp.Id;

            return resp;
        }
        public GetModuleListResponse GetModuleList()
        {
            GetModuleListResponse resp = new GetModuleListResponse();
            resp.ModuleList = new List<Modules>();
            Modules u;
            string query = "SELECT * FROM MODULE WHERE STATUS = 'A'";
            GenericGetDataResponse getData = new GenericGetDataResponse();
            getData.Data = new DataTable();

            getData = GetData(query);

            if (getData.Data.Rows.Count > 0)    
            {
                foreach (DataRow dr in getData.Data.Rows)
                {
                    u = new Modules();
                    u.ModuleID = int.Parse(dr["MODULE_ID"].ToString());
                    u.Name = dr["NAME"].ToString();
                    u.Description = dr["DESCRIPTION"].ToString();
                    u.Status = dr["STATUS"].ToString() == "A" ? "ACTIVE" : "INACTIVE";
                    resp.ModuleList.Add(u);
                }
                resp.isSuccess = true;
                resp.Message = "List Of Users";
                return resp;
            }
            else
            {
                resp.isSuccess = true;
                resp.Message = " No users found.";
                return resp;
            }



        }
        public GenericGetDataResponse GetData(string query)
        {
            GenericGetDataResponse resp = new GenericGetDataResponse();
            DataTable dt;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    dt = new DataTable();
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                        reader.Close();
                    }
                    conn.Close();
                }
                resp.isSuccess = true;
                resp.Message = "Successfully get data";
                resp.Data = dt;

            }
            catch (Exception ex)
            {
                resp.isSuccess = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
        public GetUserListResponse Login(LoginRequest r)
        {
            GetUserListResponse resp = new GetUserListResponse();
            resp.UsersList = new List<User>();
            User u;
            string query = "SELECT * FROM USER WHERE USER_NAME ='" + r.UserName + "' AND PASSWORD ='" + r.Password + "'";
            GenericGetDataResponse getData = new GenericGetDataResponse();
            getData.Data = new DataTable();

            getData = GetData(query);

            if (getData.Data.Rows.Count > 0)
            {
                foreach (DataRow dr in getData.Data.Rows)
                {
                    u = new User();
                    u.UserId = int.Parse(dr["USER_ID"].ToString());
                    u.FirstName = dr["FIRST_NAME"].ToString();
                    u.LastName = dr["LAST_NAME"].ToString();
                    u.UserName = dr["USER_NAME"].ToString();
                    u.Status = dr["STATUS"].ToString() == "A" ? "ACTIVE" : "INACTIVE";
                    u.EmailAddress = dr["EMAIL_ADDRESS"].ToString();
                    resp.UsersList.Add(u);
                }
                resp.isSuccess = true;
                resp.Message = "Login Success";
                return resp;
            }
            else
            {
                resp.isSuccess = false;
                resp.Message = "Invalid username or password";
                return resp;
            }
        }

        public GetUserAccessResponse GetUserAccess()
        {
            GetUserAccessResponse resp = new GetUserAccessResponse();
            resp.UserAccess = new List<UserAccessModule>();
            UserAccessModule u;
            string query = "SELECT p.USER_NAME, p.FIRST_NAME, p.LAST_NAME, p.STATUS, p.EMAIL_ADDRESS, q.USER_TYPE_DESCRIPTION, r.NAME 'MODULE' FROM USER AS p INNER JOIN USER_TYPES AS q ON p.USER_TYPE_ID = q.USER_TYPE_ID INNER JOIN USER_TYPE_MODULE_ACCESS utma ON utma.USER_TYPE_ID = q.USER_TYPE_ID INNER JOIN MODULE AS r ON utma.MODULE_ID = r.MODULE_ID";
            GenericGetDataResponse getData = new GenericGetDataResponse();
            getData.Data = new DataTable();

            getData = GetData(query);

            if (getData.Data.Rows.Count > 0)
            {
                foreach (DataRow dr in getData.Data.Rows)
                {
                    u = new UserAccessModule();
                    u.UserName = dr["USER_NAME"].ToString();
                    u.UserTypeDesc = dr["USER_TYPE_DESCRIPTION"].ToString();
                    u.ModuleName = dr["MODULE"].ToString();  
                    u.FirstName = dr["FIRST_NAME"].ToString();
                    u.LastName = dr["LAST_NAME"].ToString();
                    u.EmailAddress = dr["EMAIL_ADDRESS"].ToString();
                    u.Status = dr["STATUS"].ToString();
                    resp.UserAccess.Add(u);
                }
                resp.isSuccess = true;
                resp.Message = "List Of Users Access";
                return resp;
            }
            else
            {
                resp.isSuccess = true;
                resp.Message = " No users found.";
                return resp;
            }


        }
    }
}
